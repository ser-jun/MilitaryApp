using MilitaryApp.Data.Repositories;
using MilitaryApp.DTO;
using MilitaryApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MilitaryApp.ViewModel
{
    public class MilitaryStructureViewModel : INotifyPropertyChanged
    {
        public ICommand AddItemCommand { get; }
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _name;
        private string _selectedStructureType;
        private MilitaryStructureItem _selectedParentItem;

        private ObservableCollection<MilitaryStructureItem> _militaryStructureItems;
        private ObservableCollection<MilitaryStructureItem> _parentItems;
        private ObservableCollection<string> _structureTypes;

        private readonly MilitaryStructureRepository<Army> _armyRepository;
        private readonly MilitaryStructureRepository<Division> _divisionRepository;
        private readonly MilitaryStructureRepository<Corps> _corpsRepository;
        private readonly MilitaryStructureRepository<Militaryunit> _militaryUnitRepository;

        public MilitaryStructureViewModel(
            MilitaryStructureRepository<Army> armyRepository,
            MilitaryStructureRepository<Division> divisionRepository,
            MilitaryStructureRepository<Corps> corpsRepository,
            MilitaryStructureRepository<Militaryunit> militaryUnitRepository)
        {
            _armyRepository = armyRepository;
            _divisionRepository = divisionRepository;
            _corpsRepository = corpsRepository;
            _militaryUnitRepository = militaryUnitRepository;

            AddItemCommand = new RelayCommand(async () => await AddItem());

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

        #endregion

        #region CRUD operations 

        public async Task LoadMilitaryStructureItems()
        {
            var militaryStructureItems = await _armyRepository.GetMilitaryStructure();
            MilitaryStructureItems = new ObservableCollection<MilitaryStructureItem>(militaryStructureItems);
        }
        #region AddMethods
        private async Task AddItem()
        {
            if (string.IsNullOrEmpty(NewItemName) || string.IsNullOrEmpty(SelectedStructureType))
            {
                return; 
            }

            switch (SelectedStructureType)
            {
                case "Армия":
                    await AddArmy();
                    break;

                case "Дивизия":
                    if (SelectedParentItem?.ArmyId != null)
                    {
                        await AddDivision();
                    }
                    break;

                case "Корпус":
                    if (SelectedParentItem?.DivisionId != null)
                    {
                        await AddCorps();
                    }
                    break;

                case "Военная часть":
                    if (SelectedParentItem?.CorpsId != null)
                    {
                        await AddMilitaryUnit();    
                    }
                    break;
            }

            await LoadMilitaryStructureItems();
        }
        private async Task AddArmy()
        {
            await _armyRepository.AddAsync(new Army { Name = NewItemName });
        }
        private async Task AddDivision()
        {
            await _divisionRepository.AddAsync(new Division { Name = NewItemName, ArmyId = SelectedParentItem.ArmyId });
        }
        private async Task AddCorps ()
        {
            await _corpsRepository.AddAsync(new Corps { Name = NewItemName, DivisionId = SelectedParentItem.DivisionId.Value });
        }
        private async Task AddMilitaryUnit()
        {
            await _militaryUnitRepository.AddAsync(
                new Militaryunit { Name = NewItemName, CorpsId = SelectedParentItem.CorpsId.Value });
        }
        #endregion
        private void UpdateParentItems()
        {
            switch (SelectedStructureType)
            {
                case "Дивизия":
                    ParentItems = new ObservableCollection<MilitaryStructureItem>(
                        MilitaryStructureItems.Where(x => x.ArmyId != null)
                    );
                    break;

                case "Корпус":
                    ParentItems = new ObservableCollection<MilitaryStructureItem>(
                        MilitaryStructureItems.Where(x => x.DivisionId != null)
                    );
                    break;

                case "Военная часть":
                    ParentItems = new ObservableCollection<MilitaryStructureItem>(
                        MilitaryStructureItems.Where(x => x.CorpsId != null)
                    );
                    break;

                default:
                    ParentItems = new ObservableCollection<MilitaryStructureItem>();
                    break;
            }
        }

        #endregion

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
