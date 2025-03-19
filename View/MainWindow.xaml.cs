using System.Windows;
using MilitaryApp.Data;
using MilitaryApp.Data.Repositories;
using MilitaryApp.Data.Repositories.Interfaces;
using MilitaryApp.Models;
using MilitaryApp.ViewModel;

namespace MilitaryApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var context = new MilitaryDbContext();

            IArmyRepository armyRepository = new MilitaryStructureRepository(context);
            IDivisionRepository divisionRepository = new MilitaryStructureRepository(context);
            ICorpsRepository corpsRepository = new MilitaryStructureRepository(context);
            IMilitaryUnitRepository militaryUnitRepository = new MilitaryStructureRepository(context);
            IMilitaryStructureRepository militaryStructureRepository = new MilitaryStructureRepository(context);
            ISubUnitRepository subUnitRepository = new MilitaryStructureRepository(context);

            var viewModel = new MilitaryStructureViewModel(armyRepository, divisionRepository, corpsRepository, militaryUnitRepository, militaryStructureRepository, subUnitRepository);
            this.DataContext = viewModel;
        }
    }
}
