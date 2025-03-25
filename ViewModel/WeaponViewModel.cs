using MilitaryApp.Data.Repositories.Interfaces;
using MilitaryApp.DTO;
using MilitaryApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MilitaryApp.ViewModel
{
    public class WeaponViewModel : INotifyPropertyChanged
    {
        public ICommand AddEntry { get; }
        public ICommand DeleteEntry { get; }
        public ICommand UpdateEntry { get; }
        public ICommand ApplyFilterByTypeOrUnitCommand { get; }
        public ICommand ResetFiltersCommand { get; }


        private readonly IWeaponRepository _weaponRepository;
        private readonly ICrudRepository<Militaryunit> _militaryUnitRepository;

        private int _quantity;
        private string _nameweapon;
        private string _typeWeapon;

        private Militaryunit _selectedMilitaryUnit;
        private WeaponItem _selectedWeaponItem;
        private WeaponItem _selectedWeaponType;
        private Militaryunit _selectedFilterMilitaryUnit;
        private string _nameWeaponSearch;

        private ObservableCollection<Militaryunit> _militaryUnits;
        private ObservableCollection<WeaponItem> _weaponItem;
        private ObservableCollection<WeaponItem> _weaponTypes;
        public event PropertyChangedEventHandler? PropertyChanged;
        public WeaponViewModel(IWeaponRepository weaponRepository, ICrudRepository<Militaryunit> militaryUnitRepository)
        {
            _weaponRepository = weaponRepository;
            _militaryUnitRepository = militaryUnitRepository;
            AddEntry = new RelayCommand(async () => await AddWeapon());
            DeleteEntry = new RelayCommand(async () => await DeleteWeapon());
            UpdateEntry = new RelayCommand(async () => await UpdateWeapon());
            ApplyFilterByTypeOrUnitCommand =new RelayCommand (async () => await GetWeaponsFilteredByTypeOrUnit());
            ResetFiltersCommand =new RelayCommand(async () => await ResetFilters());
            InitializeField().ConfigureAwait(false);
        }

        public ObservableCollection<WeaponItem> WeaponItem
        {
            get => _weaponItem;
            set
            {
                _weaponItem = value;
                OnPropertyChanged(nameof(WeaponItem));
            }
        }
        public WeaponItem SelectedWeaponItem
        {
            get => _selectedWeaponItem;
            set
            {
                _selectedWeaponItem = value;
                OnPropertyChanged(nameof(SelectedWeaponItem));
            }
        }
        public string NameWeapon
        {
            get => _nameweapon;
            set
            {
                _nameweapon = value;
                OnPropertyChanged(nameof(NameWeapon));
            }
        }
        public string TypeWeapon
        {
            get => _typeWeapon;
            set
            {
                _typeWeapon = value;
                OnPropertyChanged(nameof(TypeWeapon));
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
        public ObservableCollection<Militaryunit> MilitaryUnit
        {
            get => _militaryUnits;
            set
            {
                _militaryUnits = value;
                OnPropertyChanged(nameof(MilitaryUnit));
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
        public WeaponItem SelectedWeaponType
        {
            get => _selectedWeaponType;
            set
            {
                _selectedWeaponType = value;
                OnPropertyChanged(nameof(SelectedWeaponType));
            }
        }
        public Militaryunit SelectedFilterMilitaryUnit
        {
            get => _selectedFilterMilitaryUnit;
            set
            {
                _selectedFilterMilitaryUnit= value;
                OnPropertyChanged(nameof(SelectedFilterMilitaryUnit));
            }
        }
        public ObservableCollection<WeaponItem> WeaponTypes
        {
            get => _weaponTypes;
            set
            {
                _weaponTypes = value;
                OnPropertyChanged(nameof(WeaponTypes));
            }
        }
        public string NameWeaponSearch
        {
            get => _nameWeaponSearch;
            set
            {
                _nameWeaponSearch = value;
                OnPropertyChanged(nameof(NameWeaponSearch));
                _ = SearchWeaponByName();
            }
        }
        private async Task InitializeField()
        {
            await InitializeMiltaryUnitsList();
            await LoadInfoWeapon();
        }
        private async Task InitializeMiltaryUnitsList()
        {
            MilitaryUnit = new ObservableCollection<Militaryunit>(await _militaryUnitRepository.GetAllAsync());
            SelectedMilitaryUnit = MilitaryUnit.First();
        }
        private async Task LoadInfoWeapon()
        {
            var data = await _weaponRepository.GetWeaponInfoAsync();
            WeaponItem = new ObservableCollection<WeaponItem>(data);

            WeaponTypes = new ObservableCollection<WeaponItem>(
                data.GroupBy(d =>d.TypeWeapon)
                .Select(e =>e.First())
                .OrderBy(g =>g.TypeWeapon));

        }
        private async Task AddWeapon()
        {
            if (!InputValidation.ValidationAddMethod(NameWeapon, TypeWeapon, Quantity, out var error))
            {
                MessageBox.Show(error);
                return;
            }
            await _weaponRepository.AddWeapon(SelectedMilitaryUnit.UnitId.Value, NameWeapon, TypeWeapon, Quantity);
            try
            {
            await  LoadInfoWeapon();

            }
            catch (InvalidCastException ex)
            {
                MessageBox.Show(ex.Message);
            }   
        }
        private async Task DeleteWeapon()
        {
            if (!InputValidation.ValidationUpdateDeleteMethod(SelectedWeaponItem, out var error))
            {
                MessageBox.Show(error);
                return;
            }
                await _weaponRepository.DeleteWeapon(SelectedWeaponItem.WeaponId);
            await LoadInfoWeapon();
        }
        private async Task UpdateWeapon()
        {
            if (!InputValidation.ValidationUpdateDeleteMethod(SelectedWeaponItem, out var error))
            {
                MessageBox.Show(error);
                return;
            }
            await _weaponRepository.UpdateWeapon(SelectedWeaponItem.WeaponId, SelectedMilitaryUnit.UnitId.Value, NameWeapon, TypeWeapon, Quantity);
            await LoadInfoWeapon();
        }
        private async Task GetWeaponsFilteredByTypeOrUnit()
        {
            string? typeOfWeapon = SelectedWeaponType?.TypeWeapon;
            int? unitId = SelectedFilterMilitaryUnit?.UnitId;   
            var data  = await _weaponRepository.GetWeaponByTypeOrUnit(typeOfWeapon, unitId);
            WeaponItem = new ObservableCollection<WeaponItem>(data);
        }
        private async Task ResetFilters()
        {
            SelectedWeaponType = null;
            SelectedFilterMilitaryUnit = null;
            await LoadInfoWeapon();
        }
        private async Task SearchWeaponByName()
        {
            if (NameWeaponSearch == null)
            {
                await LoadInfoWeapon();
                return;
            }
            var data = await _weaponRepository.SearchWeapon(NameWeaponSearch);
            WeaponItem = new ObservableCollection<WeaponItem>(data);
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
