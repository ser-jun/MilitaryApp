using MilitaryApp.DTO;
using MilitaryApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MilitaryApp.Data.Repositories;
using MilitaryApp.Data.Repositories.Interfaces;
using System.Windows;
using System.Windows.Input;

namespace MilitaryApp.ViewModel
{
    public class MilitaryPersonalViewModel : INotifyPropertyChanged
    {
        public ICommand AddEntry { get; }
        public ICommand DeleteEntry { get; }
        public ICommand UpdateEntry { get; }
        public ICommand ApplyFilterCommand { get; }
        public ICommand ResetFiltersCommand { get; }    

        private string _firstName;
        private string _lastName;
        private string _position;

        private MilitaryRank _selectedRank;
        private MilitaryRank? _selectedRankFilter;
        private MilitaryPersonnelItem _selectedItemPersonnel;
        private Militaryspecialty _selectedSpecialty;
        private Militaryunit _selectedUnit;
        private Militaryunit _selectedUnitId;
        private Militaryspecialty _selectedMilitarySpecialty;
        private Army _selectedArmyFilter;
        private Division _selectedDivisionFilter;
        private Corps _selectedCorpsFilter;
        private Militaryunit _selectedUnitFilter;

        private ObservableCollection<MilitaryRank> _rankList;
        private ObservableCollection<Militaryspecialty> _militaryspecialties;
        private ObservableCollection<Militaryunit> _militaryUnits;
        private ObservableCollection<Army> _armyFilter;
        private ObservableCollection<Division> _divisionFilter;
        private ObservableCollection<Corps> _corpsFilter;
        private ObservableCollection<Militaryunit> _unitFilter;

        

        public ObservableCollection<MilitaryPersonnelItem> _militaryPersonalItem;
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly IMilitaryPersonnelRepository _personnelRepository;
        private readonly ICrudRepository<Militaryspecialty> _crudRepositoryMilitarySpelty;
        private readonly ICrudRepository<Militaryunit> _crudRepositoryMilitaryUnit;
        private readonly ICrudRepository<Army> _crudRepositoryArmy;
        private readonly ICrudRepository<Division> _crudRepositoryDivision;
        private readonly ICrudRepository<Corps> _crudRepositoryCorps;
        

        public MilitaryPersonalViewModel(IMilitaryPersonnelRepository personnelrepository, ICrudRepository<Militaryspecialty> crudRepository, ICrudRepository<Militaryunit> crudRepositoryMilitaryUnit)
        {
            _personnelRepository = personnelrepository;
            _crudRepositoryMilitarySpelty = crudRepository;
            _crudRepositoryMilitaryUnit = crudRepositoryMilitaryUnit;
            AddEntry = new RelayCommand(async () => await AddEntries());
            DeleteEntry = new RelayCommand(async () => await DeleteEnties());
            UpdateEntry = new RelayCommand(async () => await UpdateEnies());
            ApplyFilterCommand = new RelayCommand (async () => await SearchPersonnelFilter());
            ResetFiltersCommand = new RelayCommand(async () => await ResetFilter());

             InitializationFields().ConfigureAwait(false);
        }
        private void InitializationRankList()
        {
            RankList = new ObservableCollection<MilitaryRank>(
                    Enum.GetValues(typeof(MilitaryRank)).Cast<MilitaryRank>());
            SelectedRank = RankList.FirstOrDefault();
        }

        private async Task InitialisationMilitarySpetialties()
        {
            MilitarySpecialties = new ObservableCollection<Militaryspecialty>(await _crudRepositoryMilitarySpelty.GetAllAsync());
            SelectedSpecialties = MilitarySpecialties.First();
        }
        private async Task InitializationUnitList()
        {
            MilitaryUnit = new ObservableCollection<Militaryunit>(await _crudRepositoryMilitaryUnit.GetAllAsync());
            SelectedUnit  = MilitaryUnit.FirstOrDefault();
        }
        private async Task InitializationFields()
        {
             InitializationRankList();
            await InitialisationMilitarySpetialties();
            await InitializationUnitList();
            await LoadMilitaryPersonnelItem();
        }

        #region Properties
        public ObservableCollection<MilitaryPersonnelItem> MilitaryPersonnelItem
        {
            get => _militaryPersonalItem;
            set
            {
                _militaryPersonalItem = value;
                OnPropertyChanged(nameof(MilitaryPersonnelItem));
            }
        }
        public MilitaryPersonnelItem SelectedItemPersonnel
        {
            get => _selectedItemPersonnel;
            set
            {
                _selectedItemPersonnel = value;
                OnPropertyChanged(nameof(SelectedItemPersonnel));
            }
        }
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        public ObservableCollection<MilitaryRank> RankList
        {
            get => _rankList;
            set
            {
                _rankList = value;
                OnPropertyChanged(nameof(RankList));
            }
        }
        public MilitaryRank SelectedRank
        {
            get => _selectedRank;
            set
            {
                _selectedRank = value;
                OnPropertyChanged(nameof(SelectedRank));
            }
        }
        public string Position
        {
            get => _position;
            set
            {
                _position = value;
                OnPropertyChanged(nameof(Position));
            }
        }
        public ObservableCollection<Militaryspecialty> MilitarySpecialties
        {
            get => _militaryspecialties;
            set
            {
                _militaryspecialties = value;
                OnPropertyChanged(nameof(MilitarySpecialties));
            }
        }
        public Militaryspecialty SelectedSpecialties
        {
            get => _selectedSpecialty;
            set
            {
                _selectedSpecialty = value;
                OnPropertyChanged(nameof(SelectedSpecialties));
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
        public Militaryunit SelectedUnit
        {
            get => _selectedUnit;
            set
            {
                _selectedUnit = value;
                OnPropertyChanged(nameof(SelectedUnit));
            }
        }
        public Militaryunit SelectedUnitId
        {
            get => _selectedUnitId;
            set
            {
                _selectedUnitId = value;
                OnPropertyChanged(nameof(SelectedUnitId));
            }
        }
        public MilitaryRank? SelectedRankFilter
        {
            get => _selectedRankFilter;
            set
            {
                _selectedRankFilter = value;
                OnPropertyChanged(nameof(SelectedRankFilter));
            }
        }
        public Militaryspecialty SelectedSpecialtyFilter
        {
            get => _selectedMilitarySpecialty;
            set
            {
                _selectedMilitarySpecialty = value;
                OnPropertyChanged(nameof(SelectedSpecialtyFilter));
            }
        }
        public ObservableCollection<Army> ArmyFilter
        {
            get => _armyFilter;
            set
            {
                _armyFilter = value;
                OnPropertyChanged(nameof(ArmyFilter));
            }
        }
        public ObservableCollection<Division> DivisionFilter
        {
            get => _divisionFilter;
            set
            {
                _divisionFilter = value;
                OnPropertyChanged(nameof(DivisionFilter));
            }
        }
        public ObservableCollection<Corps> CorpsFilter
        {
            get => _corpsFilter;
            set
            {
                _corpsFilter = value;
                OnPropertyChanged(nameof(CorpsFilter));
            }
        }
        public ObservableCollection<Militaryunit> UnitFilter
        {
            get => _unitFilter;
            set
            {
                _unitFilter = value;
                OnPropertyChanged(nameof(UnitFilter));
            }
        }
        public Army SelectedArmy
        {
            get => _selectedArmyFilter;
            set
            {
                _selectedArmyFilter = value;
                OnPropertyChanged(nameof(SelectedArmy));
            }
        }
        public Division SelectedDivision
        {
            get => _selectedDivisionFilter;
            set
            {
                _selectedDivisionFilter = value;
                OnPropertyChanged(nameof(SelectedDivision));
            }
        }
        public Corps SelectedCorps
        {
            get => _selectedCorpsFilter;
            set
            {
                _selectedCorpsFilter = value;
                OnPropertyChanged(nameof(SelectedCorps));
            }
        }
        public Militaryunit SelectedUnitFilter
        {
            get => _selectedUnitFilter;
            set
            {
                _selectedUnitFilter = value;
                OnPropertyChanged(nameof(SelectedUnitFilter));
            }
        }

        #endregion
        private string GetMeaningFromEnum()
        {
            MilitaryRank rank = (MilitaryRank)SelectedRank;
                 return rank.ToString();
        }
        private async Task AddEntries()
        {
            if (Position.ToLower().Trim() == "командир части" && SelectedUnit.CommanderId != null)
            {
                MessageBox.Show("У данной части есть командир");
                return;
            }
            await _personnelRepository.AddPersonnel(FirstName, LastName,GetMeaningFromEnum(),
                Position, SelectedSpecialties.SpecialtyId, SelectedUnit.UnitId.Value);
            await LoadMilitaryPersonnelItem();
        }
        private async Task LoadMilitaryPersonnelItem()
        {  
                var data = await _personnelRepository.GetMilitaryPersonnel();        
                MilitaryPersonnelItem = new ObservableCollection<MilitaryPersonnelItem>(data);
        }
        private async Task DeleteEnties()
        {
            if (SelectedItemPersonnel == null)
            {
                MessageBox.Show("Выберите запись которую хотите удалить");
                return;
            }
            await _personnelRepository.DeletePersonnel(SelectedItemPersonnel.PersonnelId ?? 0);
            await LoadMilitaryPersonnelItem();
        }
        private async Task UpdateEnies()
        {
            if (SelectedItemPersonnel== null)
            {
                MessageBox.Show("Выберите запись которую хотите редактировать");
                return;
            }
            await _personnelRepository.UpdateItem(SelectedItemPersonnel.PersonnelId ?? 0, FirstName, LastName, GetMeaningFromEnum(),
                Position,SelectedSpecialties.SpecialtyId,SelectedUnit.UnitId.Value);
            await LoadMilitaryPersonnelItem();
        }
        private async Task SearchPersonnelFilter()
        {
            if (SelectedRankFilter.ToString() == null && SelectedUnitId == null) return;
            string? rankFilter = SelectedRankFilter?.ToString(); 
            int? unitId = SelectedUnitId?.UnitId;

            var filteredPersonnel = await _personnelRepository.SearchMilitaryPersonnel(rankFilter, unitId);

            MilitaryPersonnelItem = new ObservableCollection<MilitaryPersonnelItem>(filteredPersonnel);
        }
        private async Task SearchMilitaryPersonnelBySpecialty()
        {

        }
        private async Task ResetFilter()
        {
            SelectedRankFilter = null;
            SelectedUnitId = null;
            await LoadMilitaryPersonnelItem();
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
