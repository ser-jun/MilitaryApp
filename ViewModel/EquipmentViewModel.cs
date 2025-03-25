using Microsoft.VisualBasic;
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

        private string _nameEquipment;
        private string _typeEquipment;
        private int _quantity;

        private readonly IEquipmentRepository _equipmentRepository;
        private ICrudRepository<Militaryunit> _crudRepositoryMilitaryUnit;

        private EquipmentItem _selectedEquipment;
        private Militaryunit _selectedMilitaryUnit;

        private ObservableCollection<EquipmentItem> _equipmentItems;
        private ObservableCollection<Militaryunit> _militaryUnit;
        public event PropertyChangedEventHandler? PropertyChanged;
        public  EquipmentViewModel(IEquipmentRepository equipmentRepository, ICrudRepository<Militaryunit> crudRepositoryMilitaryUnit)
        {
            _equipmentRepository = equipmentRepository;
            _crudRepositoryMilitaryUnit = crudRepositoryMilitaryUnit;
            AddItem = new RelayCommand(async () => await AddEquipment());
            DeleteItem = new RelayCommand(async () => await DeleteEquipment());
            UpdateItem = new RelayCommand(async () => await UpdateEquipment());

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
            get=> _typeEquipment;
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

        private async Task InitiliazeFileds()
        {
            await InitiliazeListMilitaryUnit();
            await LoadEquipmentInfo();
        }
        private async Task InitiliazeListMilitaryUnit()
        {
            MilitaryUnit = new ObservableCollection<Militaryunit>(await _crudRepositoryMilitaryUnit.GetAllAsync());
            SelectedMilitaryUnit =  MilitaryUnit.FirstOrDefault();
        }
        private async Task AddEquipment()
        {
            if (string.IsNullOrWhiteSpace(NameEquipment) || string.IsNullOrWhiteSpace(TypeEquipment) || Quantity <=0  || Quantity >=999)
            {
                MessageBox.Show("Введите корректные данные");
                return;
            }
           await _equipmentRepository.AddEquipment(NameEquipment, TypeEquipment, SelectedMilitaryUnit.UnitId.Value, Quantity);
            await LoadEquipmentInfo();
        }
        private async Task DeleteEquipment()
        {
            if (SelectedEquipment == null)
            {
                MessageBox.Show("Выберите элемент для удаления");
                return;
            }
          await  _equipmentRepository.DeleteEquipment(SelectedEquipment.EquipmentId);
            await LoadEquipmentInfo();
        }
        private async Task UpdateEquipment()
        {
            if (SelectedEquipment == null)
            {
                MessageBox.Show("Выберите элемент для редактирования");
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
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
