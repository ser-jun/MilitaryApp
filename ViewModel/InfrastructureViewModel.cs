using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilitaryApp.Data.Repositories.Interfaces;
using MilitaryApp.Models;

namespace MilitaryApp.ViewModel
{
    public class InfrastructureViewModel : INotifyPropertyChanged
    {
        private IInfrastructureRepository _infrastructureRepository;
        public event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<Infrastructure> _infrastructureItem;
        private Infrastructure _selectedInfrastructure;
        public InfrastructureViewModel(IInfrastructureRepository ifrastructureRepository)
        {
            _infrastructureRepository = ifrastructureRepository;
             GetInfrastructureInfo();
        }

        public ObservableCollection<Infrastructure> InfrastructureItem
        {
            get => _infrastructureItem;
            set
            {
                _infrastructureItem = value;
                OnPropertyChanged(nameof(InfrastructureItem));
            }
        }
        public Infrastructure SelectedInfrastructure
        {
            get => _selectedInfrastructure;
            set
            {
                _selectedInfrastructure = value;
                OnPropertyChanged(nameof(SelectedInfrastructure));
            }
        }




        private async Task GetInfrastructureInfo()
        {
            var data = await _infrastructureRepository.LoadInfrastructureInfo();
            InfrastructureItem = new ObservableCollection<Infrastructure>(data);
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
