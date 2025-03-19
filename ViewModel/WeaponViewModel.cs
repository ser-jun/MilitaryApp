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
        private readonly IWeaponRepository _weaponRepository;
        private readonly ICrudRepository<Militaryunit> _militaryUnitRepository;

        private int _quantity;
        private string _nameweapon;
        private string _typeWeapon;

        public Militaryunit _selectedMilitaryUnit;
        public WeaponItem _selectedWeaponItem;

        public ObservableCollection<Militaryunit> _militaryUnits;
        public ObservableCollection<WeaponItem> _weaponItem;
        public event PropertyChangedEventHandler? PropertyChanged;
        public WeaponViewModel(IWeaponRepository weaponRepository, ICrudRepository<Militaryunit> militaryUnitRepository)
        {
            _weaponRepository = weaponRepository;
            _militaryUnitRepository = militaryUnitRepository;
            AddEntry = new RelayCommand(async () => await AddWeapon());
            DeleteEntry = new RelayCommand(async () => await DeleteWeapon());
            UpdateEntry = new RelayCommand(async () => await UpdateWeapon());   
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
            WeaponItem = new ObservableCollection<WeaponItem>(await _weaponRepository.GetWeaponInfoAsync());
        }
        private async Task AddWeapon()
        {
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
            if (SelectedMilitaryUnit == null) return;
            await _weaponRepository.DeleteWeapon(SelectedWeaponItem.WeaponId);
            await LoadInfoWeapon();
        }
        private async Task UpdateWeapon()
        {
            if (SelectedWeaponItem == null) return;
            await _weaponRepository.UpdateWeapon(SelectedWeaponItem.WeaponId, SelectedMilitaryUnit.UnitId.Value, NameWeapon, TypeWeapon, Quantity);
            await LoadInfoWeapon();
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
