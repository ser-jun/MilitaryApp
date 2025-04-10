using MilitaryApp.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MilitaryApp.ViewModel
{
    public class MainMenuViewModel : INotifyPropertyChanged
    {
        public ICommand ShowMilitaryStructureCommand { get; }
        public ICommand ShowWeaponsCommand { get; }
        public ICommand ShowCombatEquipmentCommand { get; }
        public ICommand ShowPersonnelCommand { get; }
        public ICommand ShowInfrastructureCommand { get; }
        public ICommand ExitCommand { get; }
        public event PropertyChangedEventHandler? PropertyChanged;

        public MainMenuViewModel()
        {
            ShowCombatEquipmentCommand = new RelayCommand(OpenCombatEquipmentWindow);
            ShowMilitaryStructureCommand = new RelayCommand(OpenMilitaryStructureWindow);
            ShowWeaponsCommand = new RelayCommand(OpenWeaponsWindow);
            ShowPersonnelCommand = new RelayCommand(OpenPersonnelWindow);
            ShowInfrastructureCommand = new RelayCommand(OpenInfrastructureWindow);
            ExitCommand = new RelayCommand(Exit);   
        }
        private void OpenMilitaryStructureWindow()
        {
            NavigationService.NavigateTo<MainWindow>();
        }
        private void OpenWeaponsWindow()
        {
            NavigationService.NavigateTo<WeaponWindow>();
        }
        private void OpenCombatEquipmentWindow()
        {
            NavigationService.NavigateTo<EquipmentWindow>();
        }
        private void OpenPersonnelWindow()
        {
            NavigationService.NavigateTo<MilitaryPersonalWindow>();
        }
        private void OpenInfrastructureWindow()
        {
            NavigationService.NavigateTo<InfrastructureWindow>();
        }
        private void Exit()
        {
            System.Windows.Application.Current.Shutdown();
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

