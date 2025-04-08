using MilitaryApp.Data.Repositories.Interfaces;
using MilitaryApp.DTO;
using MilitaryApp.Models;
using MilitaryApp.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace MilitaryApp.ViewModel
{
    public class MilitaryStructureViewModel : INotifyPropertyChanged
    {
        public ICommand AddItemCommand { get; }
        public ICommand DeleteItemCommand { get; }
        public ICommand UpdateItemCommand { get; }
        public ICommand OpenPersonnelWindowCommand { get; }
        public ICommand OpenEquipmentWindowCommand { get; }
        public ICommand OpenWeaponWindowCommand { get; }
        public ICommand OpenInfrastructureWindowCommand { get; }
        public ICommand ApplyFilterCommand { get; } 
        public ICommand GetMinMaxCountunitCommand { get; }
        public ICommand ResetFiltersCommand { get; }


        public event PropertyChangedEventHandler? PropertyChanged;

        private string _name;
        private string _selectedStructureType;
        private string _selectedFilterType;
        public ObservableCollection<string> FilterTypes { get; } = new ObservableCollection<string>
    {
        "По армии",
        "По дивизии",
        "По корпусу"
    };
        private MilitaryStructureItem _selectedParentItem;
        private MilitaryStructureItem _selectedItem;


        private ObservableCollection<Army> _armies;
        private ObservableCollection<Division> _divisions;
        private ObservableCollection<Corps> _corps;
        private int? _selectedArmyId;
        private int? _selectedDivisionId;
        private int? _selectedCorpsId;

        private ObservableCollection<MilitaryStructureItem> _militaryStructureItems;
        private ObservableCollection<MilitaryStructureItem> _parentItems;
        private ObservableCollection<string> _structureTypes;
        private ObservableCollection<string> _parameters;
        private string _selectedParameter;

        private readonly IArmyRepository _armyRepository;
        private readonly IDivisionRepository _divisionRepository;
        private readonly ICorpsRepository _corpsRepository;
        private readonly IMilitaryUnitRepository _unitRepository;
        private readonly IMilitaryStructureRepository _structureRepository;
        private readonly ISubUnitRepository _subUnitRepository;

        public MilitaryStructureViewModel(
             IArmyRepository armyRepository,
             IDivisionRepository divisionRepository,
             ICorpsRepository corpsRepository,
             IMilitaryUnitRepository unitRepository,
             IMilitaryStructureRepository structureRepository,
             ISubUnitRepository subUnitRepository)
        {
            _armyRepository = armyRepository;
            _divisionRepository = divisionRepository;
            _corpsRepository = corpsRepository;
            _unitRepository = unitRepository;
            _structureRepository = structureRepository;
            _subUnitRepository = subUnitRepository;

            AddItemCommand = new RelayCommand(async () => await AddItem());
            DeleteItemCommand = new RelayCommand(async () => await DeleteItem());
            UpdateItemCommand = new RelayCommand(async () => await UpdateItem());
            ApplyFilterCommand = new RelayCommand(async () => await ApllyFilter());
            GetMinMaxCountunitCommand = new RelayCommand(async () => await LoadMinMaxCountUnit());
            ResetFiltersCommand = new RelayCommand(async () => await ResetFilters());   

            OpenPersonnelWindowCommand = new RelayCommand(OpenPersonnelWindow);
            OpenEquipmentWindowCommand = new RelayCommand(OpenEquipmentWindow);
            OpenWeaponWindowCommand = new RelayCommand(OpenWeaponWindow);
            OpenInfrastructureWindowCommand = new RelayCommand(OpenInfrastructureWindow);

            StructureTypes = new ObservableCollection<string>
            {
                "Армия", "Дивизия", "Корпус", "Военная часть", "Подразделение части"
            };
            Parameters = new ObservableCollection<string>
            {
                "Минимально", "Максимально"
            };

            ParentItems = new ObservableCollection<MilitaryStructureItem>();
            LoadFields().ConfigureAwait(false);
        }

        private async Task LoadFields()
        {
            await LoadMilitaryStructureItems();
            await LoadData();
            SelectedFilterType = null;
        }
        #region Properties

        public ObservableCollection<MilitaryStructureItem> MilitaryStructureItems
        {
            get => _militaryStructureItems;
            set
            {
                _militaryStructureItems = value;
                OnPropertyChanged(nameof(MilitaryStructureItems));
            }
        }
        public ObservableCollection<string> Parameters
        {
            get => _parameters;
            set
            {
                _parameters = value;
                OnPropertyChanged(nameof(Parameters));
            }
        }
        public string SelectedParameter
        {
            get => _selectedParameter;
            set
            {
                _selectedParameter = value;
                OnPropertyChanged(nameof(SelectedParameter));
            }
        }



        public ObservableCollection<string> StructureTypes
        {
            get => _structureTypes;
            set
            {
                _structureTypes = value;
                OnPropertyChanged(nameof(StructureTypes));
            }
        }

        public ObservableCollection<MilitaryStructureItem> ParentItems
        {
            get => _parentItems;
            set
            {
                _parentItems = value;
                OnPropertyChanged(nameof(ParentItems));
            }
        }

        public string NewItemName
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(NewItemName));
            }
        }

        public string SelectedStructureType
        {
            get => _selectedStructureType;
            set
            {
                _selectedStructureType = value;
                OnPropertyChanged(nameof(SelectedStructureType));
                UpdateParentItems();
            }
        }

        public MilitaryStructureItem SelectedParentItem
        {
            get => _selectedParentItem;
            set
            {
                _selectedParentItem = value;
                OnPropertyChanged(nameof(SelectedParentItem));
            }
        }

        public MilitaryStructureItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        public string SelectedFilterType
        {
            get => _selectedFilterType;
            set
            {
                _selectedFilterType = value;
                OnPropertyChanged(nameof(SelectedFilterType));
                if (value != "По армии") SelectedArmyId = null;
                if (value != "По дивизии") SelectedDivisionId = null;
                if (value != "По корпусу") SelectedCorpsId = null;
            }
        }
        public ObservableCollection<Army> Armies
        {
            get => _armies;
            set
            {
                _armies = value;
                OnPropertyChanged(nameof(Armies));
            }
        }
        public ObservableCollection<Division> Divisions
        {
            get => _divisions;
            set
            {
                _divisions = value;
                OnPropertyChanged(nameof(Divisions));
            }
        }
        public ObservableCollection<Corps> Corps
        {
            get => _corps;
            set
            {
                _corps = value;
                OnPropertyChanged(nameof(Corps));
            }
        }
        public int? SelectedArmyId
        {
            get => _selectedArmyId;
            set
            {
                _selectedArmyId = value;
                OnPropertyChanged(nameof(SelectedArmyId));
            }
        }
        public int? SelectedDivisionId
        {
            get => _selectedDivisionId;
            set
            {
                _selectedDivisionId = value;
                OnPropertyChanged(nameof(SelectedDivisionId));
            }
        }
        public int? SelectedCorpsId
        {
            get => _selectedCorpsId;
            set
            {
                _selectedCorpsId = value;
                OnPropertyChanged(nameof(SelectedCorpsId));
            }
        }
        private bool _disableTriggers;
        public bool DisableTriggers
        {
            get => _disableTriggers;
            set
            {
                _disableTriggers = value;
                OnPropertyChanged(nameof(DisableTriggers));
                UpdateTriggersState(value);
            }
        }

        #endregion

        #region CRUD Operations

        private async Task LoadMilitaryStructureItems()
        {
            MilitaryStructureItems = new ObservableCollection<MilitaryStructureItem>(
                await _structureRepository.GetMilitaryStructure());

            UpdateParentItems();
        }
        private async Task LoadData()
        {
            Armies = (new ObservableCollection<Army>(await _armyRepository.GetArmy()));
            Divisions = new ObservableCollection<Division>(await _divisionRepository.GetDivision());
            Corps = new ObservableCollection<Corps>(await _corpsRepository.GetCorps());
        }

        private async Task DeleteItem()
        {
            if (!InputValidation.ValidationUpdateDeleteMethod(SelectedItem, out var error))
            {
                MessageBox.Show(error);
                return;
            }

            switch (SelectedStructureType)
            {
                case "Армия":
                    await _armyRepository.DeleteArmy(SelectedItem.ArmyId ?? 0);
                    break;
                case "Дивизия":
                    await _divisionRepository.DeleteDivision(SelectedItem.DivisionId ?? 0);
                    break;
                case "Корпус":
                    await _corpsRepository.DeleteCorps(SelectedItem.CorpsId ?? 0);
                    break;
                case "Военная часть":
                    await _unitRepository.DeleteUnit(SelectedItem.UnitId ?? 0);
                    break;
                case "Подразделение части":
                    await _subUnitRepository.DeleteSubUnit(SelectedItem.SubUnitId ?? 0);
                    break;
            }
            await LoadMilitaryStructureItems();

        }

        private async Task AddItem()
        {
            if (!InputValidation.CheckAddMethodStructure(NewItemName, SelectedStructureType, out var error))
            {
                MessageBox.Show(error);
                return;
            }
            switch (SelectedStructureType)
            {
                case "Армия":
                    await _armyRepository.AddArmy(NewItemName);
                    break;
                case "Дивизия":
                    if (SelectedParentItem?.ArmyId != null)
                        await _divisionRepository.AddDivision(NewItemName, SelectedParentItem.ArmyId.Value);
                    break;
                case "Корпус":
                    if (SelectedParentItem?.DivisionId != null)
                        await _corpsRepository.AddCorps(NewItemName, SelectedParentItem.DivisionId.Value);
                    break;
                case "Военная часть":
                    if (SelectedParentItem?.CorpsId != null)
                        await _unitRepository.AddMilitaryUnit(NewItemName, SelectedParentItem.CorpsId.Value);
                    break;
                case "Подразделение части":
                    if (SelectedParentItem?.UnitId != null)
                        await _subUnitRepository.AddSubUnit(NewItemName, SelectedParentItem.UnitId.Value);
                    break;
            }
            await LoadMilitaryStructureItems();
        }
        private async void UpdateTriggersState(bool disable)
        {
                await _structureRepository.SetTriggersState(!disable);
        }

        private async Task UpdateItem()
        {
            if (!InputValidation.ValidationUpdateDeleteMethod(SelectedItem, out var error))
            {
                MessageBox.Show(error);
                return;
            }

            switch (SelectedStructureType)
            {
                case "Армия":
                    await _armyRepository.UpdateArmy(SelectedItem.ArmyId ?? 0, NewItemName);
                    break;
                case "Дивизия":
                    await _divisionRepository.UpdateDivision(SelectedItem.DivisionId ?? 0, NewItemName, SelectedParentItem.ArmyId ?? 0);
                    break;
                case "Корпус":
                    await _corpsRepository.UpdateCorps(SelectedItem.CorpsId ?? 0, NewItemName, SelectedParentItem.DivisionId ?? 0);
                    break;
                case "Военная часть":
                    await _unitRepository.UpdateMilitaryUnit(SelectedItem.UnitId ?? 0, NewItemName, SelectedParentItem.CorpsId ?? 0);
                    break;
                case "Подразделение части":
                    await _subUnitRepository.UpdateSubUnit(SelectedItem.SubUnitId ?? 0, NewItemName, SelectedParentItem.UnitId ?? 0);
                    break;
            }
            await LoadMilitaryStructureItems();

        }
        #endregion
        private void UpdateParentItems()
        {
            switch (SelectedStructureType)
            {
                case "Дивизия":
                    ParentItems = new ObservableCollection<MilitaryStructureItem>(
                        MilitaryStructureItems
                            .Where(item => item.ArmyId.HasValue)
                            .GroupBy(item => item.ArmyId) 
                            .Select(group => group.First()) 
                            .Select(item => new MilitaryStructureItem
                            {
                                ArmyId = item.ArmyId,
                                ArmyName = item.ArmyName
                            }).ToList());
                    break;

                case "Корпус":
                    ParentItems = new ObservableCollection<MilitaryStructureItem>(
                        MilitaryStructureItems
                            .Where(item => item.DivisionId.HasValue)
                            .GroupBy(item => item.DivisionId)
                            .Select(group => group.First())
                            .Select(item => new MilitaryStructureItem
                            {
                                DivisionId = item.DivisionId,
                                DivisionName = item.DivisionName
                            }).ToList());
                    break;

                case "Военная часть":
                    ParentItems = new ObservableCollection<MilitaryStructureItem>(
                        MilitaryStructureItems
                            .Where(item => item.CorpsId.HasValue)
                            .GroupBy(item => item.CorpsId)
                            .Select(group => group.First())
                            .Select(item => new MilitaryStructureItem
                            {
                                CorpsId = item.CorpsId,
                                CorpsName = item.CorpsName
                            }).ToList());
                    break;

                case "Подразделение части":
                    ParentItems = new ObservableCollection<MilitaryStructureItem>(
                        MilitaryStructureItems
                            .Where(item => item.UnitId.HasValue)
                            .GroupBy(item => item.UnitId)
                            .Select(group => group.First())
                            .Select(item => new MilitaryStructureItem
                            {
                                UnitId = item.UnitId,
                                UnitName = item.UnitName
                            }).ToList());
                    break;

                default:
                    ParentItems = new ObservableCollection<MilitaryStructureItem>();
                    break;
            }
        }

        private void OpenPersonnelWindow()
        {
            NavigationService.NavigateTo<MilitaryPersonalWindow>();
        }
        private void OpenEquipmentWindow()
        {
            NavigationService.NavigateTo<EquipmentWindow>();
        }
        private void OpenWeaponWindow()
        {
            NavigationService.NavigateTo<WeaponWindow>();
        }
        private void OpenInfrastructureWindow()
        {
            NavigationService.NavigateTo<InfrastructureWindow>();
        }

        private async Task ApllyFilter()
        {
            switch (SelectedFilterType)
            {
                case "По армии":
                    if (SelectedArmyId.HasValue)
                        await LoadFilterMilitaryStructureItem(SelectedArmyId, null, null);
                    else
                        MessageBox.Show("Выберите армию для фильтрации");
                    break;
                case "По дивизии":
                    if (SelectedDivisionId.HasValue)
                        await LoadFilterMilitaryStructureItem(null, SelectedDivisionId, null);
                    else
                        MessageBox.Show("Выберите дивизию для фильтрации");
                    break;
                case "По корпусу":
                    if (SelectedCorpsId.HasValue)
                        await LoadFilterMilitaryStructureItem(null, null, SelectedCorpsId);
                    else
                        MessageBox.Show("Выберите корпус для фильтрации");
                    break;
            }
        }
        private async Task LoadFilterMilitaryStructureItem(int? armyId, int? divisionId, int? corpsId)
        {

            MilitaryStructureItems = new ObservableCollection<MilitaryStructureItem>(
                await _structureRepository.GetFilterMilitaryStructure(armyId, divisionId, corpsId));
        }

        private async Task LoadMinMaxCountUnit()
        {
            string mode = SelectedParameter == "Максимально" ? "max" : "min";
            MilitaryStructureItems = new ObservableCollection<MilitaryStructureItem>(
                await _structureRepository.GetMinMaxCountUnit(mode));
        }
        private async Task ResetFilters()
        {
            SelectedParameter = null;
            SelectedFilterType = null;
            await LoadMilitaryStructureItems();
        }
        private void FillFields()
        {
            NewItemName = SelectedItem.Name;
            SelectedStructureType = SelectedItem == null ? null :
                SelectedItem.ArmyId.HasValue ? "Армия" :
                SelectedItem.DivisionId.HasValue ? "Дивизия" :
                SelectedItem.CorpsId.HasValue ? "Корпус" :
                SelectedItem.UnitId.HasValue ? "Военная часть" :
                SelectedItem.SubUnitId.HasValue ? "Подразделение части" : string.Empty;
           
            //SelectedParentItem = SelectedItem == null ? null :
            //    SelectedItem.ArmyId.HasValue ? 
        }
        private void UpdateSelectedType()
        {
           
        }
        
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}