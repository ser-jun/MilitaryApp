using MilitaryApp.Data.Repositories;
using MilitaryApp.DTO;
using MilitaryApp.Models;
using MilitaryApp.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using MilitaryApp.View;
using MilitaryApp;
using System.Windows;

public class MilitaryStructureViewModel : INotifyPropertyChanged
{
    public ICommand AddItemCommand { get; }
    public ICommand DeleteItemCommand { get; }
    public ICommand UpdateItemCommand { get; }  

    public ICommand OpenPersonnelWindow { get; set; }   
    public event PropertyChangedEventHandler? PropertyChanged;

    private string _name;
    private string _selectedStructureType;
    private MilitaryStructureItem _selectedParentItem;
    private MilitaryStructureItem _selectedItem;

    private ObservableCollection<MilitaryStructureItem> _militaryStructureItems;
    private ObservableCollection<MilitaryStructureItem> _parentItems;
    private ObservableCollection<string> _structureTypes;


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
        DeleteItemCommand = new RelayCommand(async () => await DeleteItem());
        UpdateItemCommand = new RelayCommand(async () => await UpdateItem());
        OpenPersonnelWindow = new RelayCommand(OpenWindow);

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
    public MilitaryStructureItem SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            OnPropertyChanged(nameof(SelectedItem));
        }
    }
    #region Properties collection
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
    #endregion

    #region CRUD operation
    private async Task LoadMilitaryStructureItems()
    {
        Armies = new ObservableCollection<Army>(await _armyRepository.GetAllAsync());
        Divisions = new ObservableCollection<Division>(await _divisionRepository.GetAllAsync());
        Corps = new ObservableCollection<Corps>(await _corpsRepository.GetAllAsync());
        MilitaryUnits = new ObservableCollection<Militaryunit>(await _militaryUnitRepository.GetAllAsync());
        MilitaryStructureItems = new ObservableCollection<MilitaryStructureItem>(await _armyRepository.GetMilitaryStructure());

        UpdateParentItems();
    }
    private async Task DeleteItem()
    {
        if (SelectedItem == null)
            return;

        switch (SelectedStructureType)
        {
            case "Армия":
                await _armyRepository.DeleteArmy(SelectedItem?.ArmyId.Value ?? 0);
                break;
            case "Дивизия":
                await _divisionRepository.DeleteDivision(SelectedItem?.DivisionId.Value ?? 0);
                break;
            case "Корпус":
                await _corpsRepository.DeleteCorps(SelectedItem?.CorpsId.Value ?? 0);
                break;
            case "Военная часть":
                await _militaryUnitRepository.DeleteUnit(SelectedItem?.UnitId.Value ?? 0);
                break;
        }

        await LoadMilitaryStructureItems();
    }
    private async Task AddItem()
    {
        if (string.IsNullOrEmpty(NewItemName) || string.IsNullOrEmpty(SelectedStructureType))
        {
            return;
        }

        switch (SelectedStructureType)
        {
            case "Армия":
                await _armyRepository.AddArmy(NewItemName);
                break;

            case "Дивизия":
                if (SelectedParentItem?.ArmyId != null)
                {
                    await _divisionRepository.AddDivision(NewItemName, SelectedParentItem.ArmyId.Value);
                }
                break;

            case "Корпус":
                if (SelectedParentItem?.DivisionId != null)
                {
                    await _corpsRepository.AddCorps(NewItemName, SelectedParentItem.DivisionId.Value);
                }
                break;

            case "Военная часть":
                if (SelectedParentItem?.CorpsId != null)
                {
                    await _militaryUnitRepository.AddMilitaryUnit(NewItemName, SelectedParentItem.CorpsId.Value);
                }
                break;
        }

        await LoadMilitaryStructureItems();
    }
    private async Task UpdateItem()
    {
        if (SelectedItem == null)
            return;
        switch  (SelectedStructureType)
        {
            case "Армия":
                await _armyRepository.UpdateArmy(SelectedItem?.ArmyId.Value ?? 0, NewItemName);
                break;
            case "Дивизия":
                await _divisionRepository.UpdateDivision(SelectedItem?.DivisionId.Value ?? 0, NewItemName, SelectedParentItem.ArmyId.Value);
                break;
            case "Корпус":
                await _corpsRepository.UpdateCorps(SelectedItem?.CorpsId.Value ?? 0, NewItemName, SelectedParentItem.DivisionId.Value);
                break;
            case "Военная часть":
                await _militaryUnitRepository.UpdateMilitaryUnit(SelectedItem?.UnitId.Value ?? 0, NewItemName, SelectedParentItem.CorpsId.Value);
                break;
        }
       await LoadMilitaryStructureItems();
    }
    #endregion
    private void UpdateParentItems()
    {
        switch (SelectedStructureType)
        {
            case "Дивизия":
                ParentItems = new ObservableCollection<MilitaryStructureItem>(
                    Armies.Select(a => new MilitaryStructureItem { ArmyId = a.ArmyId, ArmyName = a.Name }).ToList());
                break;

            case "Корпус":
                ParentItems = new ObservableCollection<MilitaryStructureItem>(
                    Divisions.Select(d => new MilitaryStructureItem { DivisionId = d.DivisionId, DivisionName = d.Name }).ToList());
                break;

            case "Военная часть":
                ParentItems = new ObservableCollection<MilitaryStructureItem>(
                    Corps.Select(c => new MilitaryStructureItem { CorpsId = c.CorpsId, CorpsName = c.Name }).ToList());
                break;

            default:
                ParentItems = new ObservableCollection<MilitaryStructureItem>();
                break;
        }
    }

    private void OpenWindow()
    {
        MilitaryPersonalWindow mpWindow = new MilitaryPersonalWindow();
        Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)?.Close();
        mpWindow.Show();
    }
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
