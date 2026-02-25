using ArmyOptimizer.Utilities;

namespace ArmyOptimizer.ViewModels
{
    public class NavigationVM : ViewModelBase
    {
        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public RelayCommand ShowLoginCommand { get; }
        public RelayCommand ShowRegisterCommand { get; }

        public NavigationVM()
        {
            ShowLoginCommand = new RelayCommand(_ => CurrentView = new LoginVM(this));
            ShowRegisterCommand = new RelayCommand(_ => CurrentView = new RegisterVM(this));

            CurrentView = new LoginVM(this);
        }
    }
}