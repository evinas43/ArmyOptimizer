using System.Collections.ObjectModel;
using System.Windows.Media;
using ArmyOptimizer.Models;
using ArmyOptimizer.Services;
using ArmyOptimizer.Utilities;

namespace ArmyOptimizer.ViewModels
{
    public class HomeVM : ViewModelBase
    {
        private readonly NavigationVM _navigation;
        private readonly ArmyService _armyService;
        private readonly UserService _userService;

        public ObservableCollection<ArmySummary> Armies { get; set; } = new();
        public int TokensRemaining { get; set; }
        //colorchanging tokens
        public Brush TokenColor => TokensRemaining > 0 ? Brushes.Gold : Brushes.Red;
        public string WelcomeText { get; }

        public RelayCommand LogoutCommand { get; }
        public RelayCommand OptimizeArmyCommand { get; }
        public RelayCommand<ArmySummary> OpenArmyCommand { get; }
        public RelayCommand SeeAllArmiesCommand { get; }

        public HomeVM(NavigationVM navigation)
        {
            _navigation = navigation;
            _armyService = new ArmyService(HttpService.Client);

            _userService = new UserService(HttpService.Client);

            WelcomeText = $"Welcome back, {SessionUser.Username}";

            LogoutCommand = new RelayCommand(_ => Logout());

            SeeAllArmiesCommand = new RelayCommand(_ => SeeAllArmys());

            OptimizeArmyCommand = new RelayCommand(_=>
            {
                _navigation.CurrentView = new OptimizeArmyVM(_navigation);
            });

            OpenArmyCommand = new RelayCommand<ArmySummary>(army =>
            {
                _navigation.CurrentView = new ArmyDetailVM(_navigation, army.Id);
            });

            LoadTokens();
            LoadArmies();
        }

        private void Logout()
        {
            SessionUser.Username = null;
            SessionUser.token = null;

            _navigation.CurrentView = new LoginVM(_navigation);
        }
        private void SeeAllArmys() {

            _navigation.CurrentView = new CompleteArmyListVM(_navigation);

        }

        private async Task LoadTokens()
        {
            var tokens = await _userService.GetTokensAsync(); //load tokens of the user
            TokensRemaining = tokens;
            OnPropertyChanged(nameof(TokensRemaining));
        }


        private async void LoadArmies()
        {
            var armies = await _armyService.GetUserArmiesAsync();

            if (armies == null)
                return;

            Armies.Clear();

            foreach (var army in armies
                                   .OrderByDescending(a => a.CreatedAt) //order by created date
                                   .Take(3)) //take the 3 most recent armies
            {
                Armies.Add(army);
            }
        }
    }
}