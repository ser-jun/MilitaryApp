using MilitaryApp.Data.Repositories;
using MilitaryApp.DTO;
using MilitaryApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
namespace MilitaryApp.ViewModel
{
    public class MilitaryStructureViewModel : INotifyPropertyChanged
    {
        public ICommand AddItemCommand { get; }
        public ICommand RemoveItemCommand { get; }
        public ICommand UpdateItemCommand { get; }
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _name;
        private Army _selectedArmy;
        private ObservableCollection<Army> _armies;
        private ObservableCollection<MilitaryStructureItem> _militaryStructureItems; 
        private MilitaryStructureRepository<Army> _armyRepository;
       

        public MilitaryStructureViewModel(MilitaryStructureRepository<Army> armyRepository)
        {
            _armyRepository = armyRepository;
            AddItemCommand = new RelayCommand(async () => AddData());
            RemoveItemCommand = new RelayCommand(async () => DeleteData());
            UpdateItemCommand = new RelayCommand(async () => UpdateData());
            LoadMilitaryStructureItems().ConfigureAwait(false);
        }
        public ObservableCollection<Army> Armies
        {
            get { return _armies; }
            set
            {
                _armies = value;
                OnPropertyChanged(nameof(Armies));
            }
        }
        public ObservableCollection<MilitaryStructureItem> MilitaryStructureItems
        {
            get { return _militaryStructureItems; }
            set
            {
                _militaryStructureItems = value;
                OnPropertyChanged(nameof(MilitaryStructureItems));
                (RemoveItemCommand as RelayCommand)?.RaiseCanExecuteChanged();
                //(UpdateItemCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
        public string NewItemName
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(NewItemName));
            }
        }

        public Army SelectedItem
        {
            get => _selectedArmy;
            set
            {
                _selectedArmy = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        #region CRUD operation 
        public async Task LoadMilitaryStructureItems()
        {
            try
            {
                var militaryStructureItems = await _armyRepository.GetMilitaryStructure();
                MilitaryStructureItems = new ObservableCollection<MilitaryStructureItem>(militaryStructureItems);


                if (MilitaryStructureItems.Count == 0)
                {
                    MessageBox.Show("Данные не найдены.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
        }
        public async Task LoadData()
        {
            var armies = await _armyRepository.GetAllAsync();
            Armies = new ObservableCollection<Army>(armies);
        }
        public async void AddData()
        {
            var army = new Army { Name = _name };
            await _armyRepository.AddAsync(army);
           await LoadMilitaryStructureItems();
            ((RelayCommand)AddItemCommand).RaiseCanExecuteChanged();
        }
        public async void DeleteData()
        {
            await _armyRepository.DeleteArmyByIdAsync(SelectedItem.ArmyId);
            Armies.Remove(SelectedItem);
        }
        public async void UpdateData()
        {

            SelectedItem.Name = NewItemName;
            await _armyRepository.UpdateAsync(SelectedItem);
            await LoadData();
        }
        #endregion

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
