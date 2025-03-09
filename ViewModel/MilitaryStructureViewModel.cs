using MilitaryApp.Data.Repositories;
using MilitaryApp.DTO;
using MilitaryApp.Models;
using MilitaryApp.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

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

    // Коллекции для каждой структуры
    private ObservableCollection<Army> _armies;
    private ObservableCollection<Division> _divisions;
    private ObservableCollection<Corps> _corps;
    private ObservableCollection<Militaryunit> _militaryUnits;

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

    private async Task LoadMilitaryStructureItems()
    {
        // Загружаем данные моделей через репозитории
        //var armies = await _armyRepository.GetAllAsync();
        //var divisions = await _divisionRepository.GetAllAsync();
        //var corps = await _corpsRepository.GetAllAsync();
        //var militaryUnits = await _militaryUnitRepository.GetAllAsync();

        //Armies = new ObservableCollection<Army>(await _armyRepository.GetAllAsync());
        //Divisions = new ObservableCollection<Division>(await _divisionRepository.GetAllAsync());
        //Corps = new ObservableCollection<Corps>(await _corpsRepository.GetAllAsync());
        //MilitaryUnits = new ObservableCollection<Militaryunit>(await _militaryUnitRepository.GetAllAsync());
        //var militaryStructureItems = await _armyRepository.GetMilitaryStructure();
        MilitaryStructureItems = new ObservableCollection<MilitaryStructureItem>(await _armyRepository.GetMilitaryStructure());

        UpdateParentItems();
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
        await _divisionRepository.AddAsync(new Division { Name = NewItemName, ArmyId = SelectedParentItem.ArmyId.Value });
    }

    private async Task AddCorps()
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
                // Используем уже загруженные коллекции армий
                ParentItems = new ObservableCollection<MilitaryStructureItem>(
                    Armies.Select(a => new MilitaryStructureItem { ArmyId = a.ArmyId, ArmyName = a.Name }).ToList());
                break;

            case "Корпус":
                // Используем уже загруженные коллекции дивизий
                ParentItems = new ObservableCollection<MilitaryStructureItem>(
                    Divisions.Select(d => new MilitaryStructureItem { DivisionId = d.DivisionId, DivisionName = d.Name }).ToList());
                break;

            case "Военная часть":
                // Используем уже загруженные коллекции корпусов
                ParentItems = new ObservableCollection<MilitaryStructureItem>(
                    Corps.Select(c => new MilitaryStructureItem { CorpsId = c.CorpsId, CorpsName = c.Name }).ToList());
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

    #region Collections

    public ObservableCollection<Army> Armies
    {
        get => _armies;
        set
        {
            _armies = value;
            OnPropertyChanged(nameof(Armies));
        }
    }

    public ObservableCollection<Division> Divisions
    {
        get => _divisions;
        set
        {
            _divisions = value;
            OnPropertyChanged(nameof(Divisions));
        }
    }

    public ObservableCollection<Corps> Corps
    {
        get => _corps;
        set
        {
            _corps = value;
            OnPropertyChanged(nameof(Corps));
        }
    }

    public ObservableCollection<Militaryunit> MilitaryUnits
    {
        get => _militaryUnits;
        set
        {
            _militaryUnits = value;
            OnPropertyChanged(nameof(MilitaryUnits));
        }
    }

    #endregion
}
