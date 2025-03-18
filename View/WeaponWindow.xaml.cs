using MilitaryApp.Data;
using MilitaryApp.Data.Repositories;
using MilitaryApp.Data.Repositories.Interfaces;
using MilitaryApp.Models;
using MilitaryApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MilitaryApp.View
{
    /// <summary>
    /// Логика взаимодействия для WeaponWindow.xaml
    /// </summary>
    public partial class WeaponWindow : Window
    {
        public WeaponWindow()
        {
            var context = new MilitaryDbContext();
            IWeaponRepository weaponRepository = new WeaponRepository(context);
            ICrudRepository<Militaryunit> militaryUnitRepository = new BaseCrudAbstract<Militaryunit>(context);
            DataContext = new WeaponViewModel(weaponRepository, militaryUnitRepository);
            InitializeComponent();
        }
    }
}
