using MilitaryApp.Data;
using MilitaryApp.Data.Repositories;
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
using MilitaryApp.Models;
using MilitaryApp.Data.Repositories;

namespace MilitaryApp.View
{
    /// <summary>
    /// Логика взаимодействия для MilitaryPersonalWindow.xaml
    /// </summary>
    public partial class MilitaryPersonalWindow : Window
    {
        public MilitaryPersonalWindow()
        {
            InitializeComponent();
            var context = new MilitaryDbContext();
            var militarySpeltiesRepository = new MilitaryPersonnelRepository<Militaryspecialty>(context);
            DataContext = new MilitaryPersonalViewModel(militarySpeltiesRepository);
        }
    }
}
