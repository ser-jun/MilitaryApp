using MilitaryApp.Data.Repositories;
using MilitaryApp.Data.Repositories.Interfaces;
using MilitaryApp.DTO;
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

        public event PropertyChangedEventHandler? PropertyChanged;

        private string _name;
        private string _selectedStructureType;
        private MilitaryStructureItem _selectedParentItem;
        private MilitaryStructureItem _selectedItem;
        private ObservableCollection<MilitaryStructureItem> _militaryStructureItems;
        private ObservableCollection<MilitaryStructureItem> _parentItems;
        private ObservableCollection<string> _structureTypes;

        private readonly IArmyRepository _armyRepository;
        private readonly IDivisionRepository _divisionRepository;
        private readonly ICorpsRepository _corpsRepository;
        private readonly IMilitaryUnitRepository _unitRepository;
        private readonly IMilitaryStructureRepository _structureRepository;

        public MilitaryStructureViewModel(
             IArmyRepository armyRepository,
             IDivisionRepository divisionRepository,
             ICorpsRepository corpsRepository,
             IMilitaryUnitRepository unitRepository,
             IMilitaryStructureRepository structureRepository)
        {
            _armyRepository = armyRepository;
            _divisionRepository = divisionRepository;
            _corpsRepository = corpsRepository;
            _unitRepository = unitRepository;
            _structureRepository = structureRepository;

            AddItemCommand = new RelayCommand(async () => await AddItem());
            DeleteItemCommand = new RelayCommand(async () => await DeleteItem());
            UpdateItemCommand = new RelayCommand(async () => await UpdateItem());
            OpenPersonnelWindowCommand = new RelayCommand(OpenPersonnelWindow);

            StructureTypes = new ObservableCollection<string>
            {
                "Армия", "Дивизия", "Корпус", "Военная часть"
            };

            ParentItems = new ObservableCollection<MilitaryStructureItem>();
            LoadMilitaryStructureItems().ConfigureAwait(false);
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

        #endregion

        #region CRUD Operations

        private async Task LoadMilitaryStructureItems()
        {
            MilitaryStructureItems = new ObservableCollection<MilitaryStructureItem>(
                await _structureRepository.GetMilitaryStructure());

            UpdateParentItems();
        }

        private async Task DeleteItem()
        {
            if (SelectedItem == null)
                return;

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
                }
                await LoadMilitaryStructureItems();
  
        }

        private async Task AddItem()
        {
            if (string.IsNullOrEmpty(NewItemName) || string.IsNullOrEmpty(SelectedStructureType))
                return;
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
                }
                await LoadMilitaryStructureItems();
        }

        private async Task UpdateItem()
        {
            if (SelectedItem == null)
                return;

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
                        MilitaryStructureItems.Where(item => item.ArmyId.HasValue)
                                              .Select(item => new MilitaryStructureItem
                                              {
                                                  ArmyId = item.ArmyId,
                                                  ArmyName = item.ArmyName
                                              }).ToList());
                    break;
                case "Корпус":
                    ParentItems = new ObservableCollection<MilitaryStructureItem>(
                        MilitaryStructureItems.Where(item => item.DivisionId.HasValue)
                                              .Select(item => new MilitaryStructureItem
                                              {
                                                  DivisionId = item.DivisionId,
                                                  DivisionName = item.DivisionName
                                              }).ToList());
                    break;
                case "Военная часть":
                    ParentItems = new ObservableCollection<MilitaryStructureItem>(
                        MilitaryStructureItems.Where(item => item.CorpsId.HasValue)
                                              .Select(item => new MilitaryStructureItem
                                              {
                                                  CorpsId = item.CorpsId,
                                                  CorpsName = item.CorpsName
                                              }).ToList());
                    break;
                default:
                    ParentItems = new ObservableCollection<MilitaryStructureItem>();
                    break;
            }
        }

        private void OpenPersonnelWindow()
        {
            MilitaryPersonalWindow mpWindow = new MilitaryPersonalWindow();
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)?.Close();
            mpWindow.Show();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}