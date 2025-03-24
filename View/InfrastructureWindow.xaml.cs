using Microsoft.EntityFrameworkCore.Infrastructure;
using MilitaryApp.Data;
using MilitaryApp.Data.Repositories;
using MilitaryApp.Data.Repositories.Interfaces;
using MilitaryApp.Models;
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

namespace MilitaryApp.View
{
    /// <summary>
    /// Логика взаимодействия для InfrastructureWindow.xaml
    /// </summary>
    public partial class InfrastructureWindow : Window
    {
        public InfrastructureWindow()
        {
            var context = new MilitaryDbContext();
            IInfrastructureRepository infrastructureRepository = new InfrastructureRepository(context);
            ICrudRepository<Militaryunit> miltaryUnitCrud = new BaseCrudAbstract<Militaryunit>(context);
            DataContext = new InfrastructureViewModel(infrastructureRepository, miltaryUnitCrud);
            InitializeComponent();
        }
    }
}
