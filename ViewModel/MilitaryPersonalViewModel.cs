using MilitaryApp.DTO;
using MilitaryApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MilitaryApp.Data.Repositories;

namespace MilitaryApp.ViewModel
{
    public class MilitaryPersonalViewModel : INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private string _position;

        private MilitaryRank _selectedRank;
        private MilitaryPersonnelItem _selectedItemPersonnel;
        private Militaryspecialty _selectedSpecialty;

        private ObservableCollection<MilitaryRank> _rankList;
        private ObservableCollection<Militaryspecialty> _militaryspecialties;
        private ObservableCollection<MilitaryPersonnelItem> _militaryPersonalItem;
        public event PropertyChangedEventHandler? PropertyChanged;

        private MilitaryPersonnelRepository<Militaryspecialty> _militarySpeltiesRepository;

        public MilitaryPersonalViewModel(MilitaryPersonnelRepository<Militaryspecialty> militarySpeltiesRepository)
        {
            _militarySpeltiesRepository = militarySpeltiesRepository;
            InitializationRankList();
            LoadMilitarySpetialties();
        }
        private void InitializationRankList()
        {
            RankList = new ObservableCollection<MilitaryRank>(
                    Enum.GetValues(typeof(MilitaryRank)).Cast<MilitaryRank>());
            SelectedRank = RankList.FirstOrDefault();
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

        #endregion

        public async Task LoadMilitarySpetialties()
        {
           MilitarySpecialties = new ObservableCollection<Militaryspecialty>(await _militarySpeltiesRepository.GetAllAsync());
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
