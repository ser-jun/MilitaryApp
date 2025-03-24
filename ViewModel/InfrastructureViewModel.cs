using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MilitaryApp.Data.Repositories.Interfaces;
using MilitaryApp.Models;

namespace MilitaryApp.ViewModel
{
    public class InfrastructureViewModel : INotifyPropertyChanged
    {
        public ICommand AddItemCommand { get; }
        public ICommand DeleteItemCommand { get; }
        public ICommand UpdateItemCommand { get; }
        private IInfrastructureRepository _infrastructureRepository;
        private ICrudRepository<Militaryunit> _militaryUnitCrud;
        public event PropertyChangedEventHandler? PropertyChanged;
        private string _name;
        private int _year;

        private Militaryunit _selectedMilitaryUnit;
        private ObservableCollection<Infrastructure> _infrastructureItem;
        private ObservableCollection<Militaryunit> _militaryUnit;
        private Infrastructure _selectedInfrastructure;
        public InfrastructureViewModel(IInfrastructureRepository ifrastructureRepository, ICrudRepository<Militaryunit> militaryUnitCrud)
        {
            _infrastructureRepository = ifrastructureRepository;
            _militaryUnitCrud= militaryUnitCrud;
            AddItemCommand = new RelayCommand(async () => await AddInfrastructure());
            DeleteItemCommand = new RelayCommand(async () => await DeleteIfrastructure());
            UpdateItemCommand = new RelayCommand(async () => await UpdateInfrastructure());
            LoadData().ConfigureAwait(false);
        }
        private async Task LoadData()
        {
            await GetInfrastructureInfo();
            await LoadMilitaryUnitList();
        }

        public ObservableCollection<Infrastructure> InfrastructureItem
        {
            get => _infrastructureItem;
            set
            {
                _infrastructureItem = value;
                OnPropertyChanged(nameof(InfrastructureItem));
            }
        }
        public Infrastructure SelectedInfrastructure
        {
            get => _selectedInfrastructure;
            set
            {
                _selectedInfrastructure = value;
                OnPropertyChanged(nameof(SelectedInfrastructure));
     
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
        public Militaryunit SelectedMilitaryUnit
        {
            get => _selectedMilitaryUnit;
            set
            {
                _selectedMilitaryUnit = value;
                OnPropertyChanged(nameof(SelectedMilitaryUnit));   

            }
        }
        public string NameInfrastructure
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(NameInfrastructure));
            }
        }
        public int YearBuild
        {
            get => _year;
            set
            {
                _year = value;
                OnPropertyChanged(nameof(YearBuild));
            }
        }


        private async Task GetInfrastructureInfo()
        {
            var data = await _infrastructureRepository.LoadInfrastructureInfo();
            InfrastructureItem = new ObservableCollection<Infrastructure>(data);
        }
        private async Task LoadMilitaryUnitList()
        {
            MilitaryUnit = new ObservableCollection<Militaryunit>(await _militaryUnitCrud.GetAllAsync());
            SelectedMilitaryUnit = MilitaryUnit.FirstOrDefault();
        }
        private async Task AddInfrastructure()
        {
            if (SelectedMilitaryUnit == null) return;

            await _infrastructureRepository.AddInfrastructureItem(NameInfrastructure, SelectedMilitaryUnit.UnitId.Value, YearBuild);
            await GetInfrastructureInfo();
        }
        private async Task DeleteIfrastructure()
        {
            if (SelectedInfrastructure == null) return;
            await _infrastructureRepository.DeleteInfrastructureItem(SelectedInfrastructure.BuildingId);
            await GetInfrastructureInfo();
        }
        private async Task UpdateInfrastructure()
        {
            if (SelectedInfrastructure == null) return;
            await _infrastructureRepository.UpdateInfrastructureItem(SelectedInfrastructure.BuildingId, NameInfrastructure, 
                SelectedMilitaryUnit.UnitId.Value, YearBuild);
            await GetInfrastructureInfo();
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
