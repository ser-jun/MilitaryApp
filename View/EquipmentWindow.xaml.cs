using MilitaryApp.Data;
using MilitaryApp.Data.Repositories;
using MilitaryApp.Data.Repositories.Interfaces;
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
using MilitaryApp.ViewModel;
using MilitaryApp.Models;

namespace MilitaryApp.View
{
    /// <summary>
    /// Логика взаимодействия для EquipmentWindow.xaml
    /// </summary>
    public partial class EquipmentWindow : Window
    {
        
        public EquipmentWindow()
        {
            InitializeComponent();
            var context = new MilitaryDbContext();
            IEquipmentRepository equpmentRepository = new EquipmentRepository(context);
            ICrudRepository<Militaryunit> crudRepositoryMilitaryUnit = new BaseCrudAbstract<Militaryunit>(context);
            DataContext = new EquipmentViewModel(equpmentRepository, crudRepositoryMilitaryUnit);
        }
    }
}
