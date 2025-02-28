﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MilitaryApp.Data;
using MilitaryApp.Data.Repositories;
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
            var armyRepository = new MilitaryStructureRepository<Army>(context);

            var viewModel = new MilitaryStructureViewModel(armyRepository);
            this.DataContext = viewModel;
            viewModel.LoadData();
        }
    }
}