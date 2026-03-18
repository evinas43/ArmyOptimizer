using System.Collections.ObjectModel;
using ArmyOptimizer.Models;
using ArmyOptimizer.Services;
using ArmyOptimizer.Utilities;

namespace ArmyOptimizer.ViewModels
{
    public class HomeVM : ViewModelBase
    {
        private readonly NavigationVM _navigation;
        private readonly ArmyService _armyService;

        public ObservableCollection<ArmySummary> Armies { get; set; } = new();

        public string WelcomeText { get; }

        public RelayCommand LogoutCommand { get; }
        
        public RelayCommand OptimizeArmyCommand { get; }
        public RelayCommand<ArmySummary> OpenArmyCommand { get; }

        public HomeVM(NavigationVM navigation)
        {
            _navigation = navigation;
            _armyService = new ArmyService(HttpService.Client);

            WelcomeText = $"Welcome back, {SessionUser.Username}";

            LogoutCommand = new RelayCommand(_ => Logout());

            OptimizeArmyCommand = new RelayCommand(_=>
            {
                _navigation.CurrentView = new OptimizeArmyVM(_navigation);
            });

            OpenArmyCommand = new RelayCommand<ArmySummary>(army =>
            {
                _navigation.CurrentView = new ArmyDetailVM(_navigation, army.Id);
            });

            LoadArmies();
        }

        private void Logout()
        {
            SessionUser.Username = null;
            SessionUser.token = null;

            _navigation.CurrentView = new LoginVM(_navigation);
        }



        private async void LoadArmies()
        {
            var armies = await _armyService.GetUserArmiesAsync();

            if (armies == null)
                return;

            Armies.Clear();

            foreach (var army in armies
                                   .OrderByDescending(a => a.CreatedAt)
                                   .Take(5))
            {
                Armies.Add(army);
            }
        }
    }
}