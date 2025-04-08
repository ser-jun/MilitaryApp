using MilitaryApp.Data.Repositories.Interfaces;
using MilitaryApp.DTO;
using MilitaryApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace MilitaryApp.ViewModel
{
    public class EquipmentViewModel : INotifyPropertyChanged
    {
        public ICommand AddItem { get; }
        public ICommand DeleteItem { get; }
        public ICommand UpdateItem { get; }
        public ICommand ApplyFilterByTypeOrUnitCommand { get; } 
        public ICommand ApplyFilterByQuantityCommand { get; }
        public ICommand ResetFiltersCommand { get; }
        public ICommand NavigateToMainPageCommand { get; }

        private string _nameEquipment;
        private string _typeEquipment;
        private int _quantity;

        private readonly IEquipmentRepository _equipmentRepository;
        private ICrudRepository<Militaryunit> _crudRepositoryMilitaryUnit;

        private EquipmentItem _selectedEquipment;
        private Militaryunit _selectedMilitaryUnit;
        private Militaryunit _selectedFilterMilitaryUnit;
        private EquipmentItem _selectedEquipmentType;
        private int _minimalQuantityEquipments;

        public ObservableCollection<EquipmentItem> _equipmentTypes;
        private ObservableCollection<EquipmentItem> _equipmentItems;
        private ObservableCollection<Militaryunit> _militaryUnit;
        public event PropertyChangedEventHandler? PropertyChanged;
        public EquipmentViewModel(IEquipmentRepository equipmentRepository, ICrudRepository<Militaryunit> crudRepositoryMilitaryUnit)
        {
            _equipmentRepository = equipmentRepository;
            _crudRepositoryMilitaryUnit = crudRepositoryMilitaryUnit;
            AddItem = new RelayCommand(async () => await AddEquipment());
            DeleteItem = new RelayCommand(async () => await DeleteEquipment());
            UpdateItem = new RelayCommand(async () => await UpdateEquipment());
            ApplyFilterByTypeOrUnitCommand = new RelayCommand(async () => await GetEquipmentsFilteredByTypeOrUnit());
            ApplyFilterByQuantityCommand = new RelayCommand(async () => await GetEquipmentsFilteredByQuantity());   
            ResetFiltersCommand = new RelayCommand(async  () => await ResetFilters());
            NavigateToMainPageCommand = new RelayCommand(NavigateToMainPage);

            InitiliazeFileds().ConfigureAwait(false);
        }
        public ObservableCollection<EquipmentItem> EquipmentItem
        {
            get => _equipmentItems;
            set
            {
                _equipmentItems = value;
                OnPropertyChanged(nameof(EquipmentItem));
            }
        }
        public EquipmentItem SelectedEquipment
        {
            get => _selectedEquipment;
            set
            {
                _selectedEquipment = value;
                FillFields();
                OnPropertyChanged(nameof(SelectedEquipment));
            }
        }
        public string NameEquipment
        {
            get => _nameEquipment;
            set
            {
                _nameEquipment = value;
                OnPropertyChanged(nameof(NameEquipment));
            }
        }
        public string TypeEquipment
        {
            get => _typeEquipment;
            set
            {
                _typeEquipment = value;
                OnPropertyChanged(nameof(TypeEquipment));
            }
        }
        public ObservableCollection<Militaryunit> MilitaryUnit
        {
            get => _militaryUnit;
            set
            {
                _militaryUnit = value;
                OnPropertyChanged(nameof(MilitaryUnit));
            }
        }
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }
        public Militaryunit SelectedMilitaryUnit
        {
            get => _selectedMilitaryUnit;
            set
            {
                _selectedMilitaryUnit = value;
                OnPropertyChanged(nameof(SelectedMilitaryUnit));
            }
        }
        public Militaryunit SelectedFilterMilitaryUnit
        {
            get => _selectedFilterMilitaryUnit;
            set
            {
                _selectedFilterMilitaryUnit = value;
                OnPropertyChanged(nameof(SelectedFilterMilitaryUnit));
            }
        }
        public ObservableCollection<EquipmentItem> EquipmentTypes
        {
            get => _equipmentTypes;
            set
            {
                _equipmentTypes = value;
                OnPropertyChanged(nameof(EquipmentTypes));
            }
        }
        public EquipmentItem SelectedEquipmentType
        {
            get => _selectedEquipmentType;
            set
            {
                _selectedEquipmentType = value;
                OnPropertyChanged(nameof(SelectedEquipmentType));
            }
        }
        public int MinimalQuantityEquipments
        {
            get => _minimalQuantityEquipments;
            set
            {
                _minimalQuantityEquipments = value;
                OnPropertyChanged(nameof(MinimalQuantityEquipments));
            }
        }

        private async Task InitiliazeFileds()
        {
            await InitiliazeListMilitaryUnit();
            await LoadEquipmentInfo();
        }
        private async Task InitiliazeListMilitaryUnit()
        {
            MilitaryUnit = new ObservableCollection<Militaryunit>(await _crudRepositoryMilitaryUnit.GetAllAsync());
            SelectedMilitaryUnit = MilitaryUnit.FirstOrDefault();
        }
        private async Task AddEquipment()
        {
            if (!InputValidation.ValidationAddMethod(NameEquipment, TypeEquipment, Quantity, out var error))
            {
                MessageBox.Show(error);
                return;
            }
            await _equipmentRepository.AddEquipment(NameEquipment, TypeEquipment, SelectedMilitaryUnit.UnitId.Value, Quantity);
            await LoadEquipmentInfo();
        }
        private async Task DeleteEquipment()
        {
            if (!InputValidation.ValidationUpdateDeleteMethod(SelectedEquipment, out var error))
            {
                MessageBox.Show(error);
                return;
            }
            await _equipmentRepository.DeleteEquipment(SelectedEquipment.EquipmentId);
            await LoadEquipmentInfo();
        }
        private async Task UpdateEquipment()
        {
            if (!InputValidation.ValidationUpdateDeleteMethod(SelectedEquipment, out var error))
            {
                MessageBox.Show(error);
                return;
            }
            await _equipmentRepository.UpdateEquipment(SelectedEquipment.EquipmentId, NameEquipment, TypeEquipment,
                SelectedMilitaryUnit.UnitId.Value, Quantity);
            await LoadEquipmentInfo();
        }
        private async Task LoadEquipmentInfo()
        {
            var data = await _equipmentRepository.GetEquipmentInfoAsync();
            EquipmentItem = new ObservableCollection<EquipmentItem>(data);

            EquipmentTypes = new ObservableCollection<EquipmentItem>(
            data.GroupBy(e => e.TypeEquipment)
            .Select(g => g.First())
            .OrderBy(e => e.TypeEquipment));
        }
        private async Task GetEquipmentsFilteredByTypeOrUnit()
        {
            if (SelectedEquipmentType == null && SelectedFilterMilitaryUnit == null) return;
            string? type = SelectedEquipmentType?.TypeEquipment;
            int? unitId = SelectedFilterMilitaryUnit?.UnitId; 
            var data = await _equipmentRepository.GetFilterCombatEquipment(type, unitId);
            EquipmentItem = new ObservableCollection<EquipmentItem>(data);
        }
        private async Task GetEquipmentsFilteredByQuantity()
        {
            string type = SelectedEquipmentType?.TypeEquipment;
            int minimalQuantity = MinimalQuantityEquipments;
            var data = await _equipmentRepository.GetFilterEqupmentByQuantity(type, minimalQuantity);
            EquipmentItem = new ObservableCollection<EquipmentItem>(data);
        }
        private async Task ResetFilters()
        {
            SelectedEquipmentType = null;
            SelectedFilterMilitaryUnit = null;
            MinimalQuantityEquipments = 0;
            ClearFields();
            await LoadEquipmentInfo();  
        }
        private void NavigateToMainPage()
        {
            NavigationService.NavigateTo<MainWindow>();
        }
        private void FillFields()
        {
            NameEquipment = SelectedEquipment.NameEquipment;
            TypeEquipment = SelectedEquipment.TypeEquipment;
            Quantity = SelectedEquipment.Quantity.Value;
            SelectedMilitaryUnit = MilitaryUnit.FirstOrDefault(x => x.UnitId == SelectedEquipment.UnitId);
        }
        private void ClearFields()
        {
            NameEquipment = null;
            TypeEquipment = null;
            Quantity = 0;
            SelectedMilitaryUnit = null;
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
