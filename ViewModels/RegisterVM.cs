using System.Threading.Tasks;
using System.Windows;
using ArmyOptimizer.Services;
using ArmyOptimizer.Utilities;
using ArmyOptimizer.ViewModels;

namespace ArmyOptimizer.ViewModels 
{ 
    public class RegisterVM : ViewModelBase
    {
        private readonly NavigationVM _navigation;
        private readonly HttpService _api;
        private readonly AuthService _authService;
        public RelayCommand GoToLoginCommand { get; }
        public RegisterVM(NavigationVM navigation)
        {
            _navigation = navigation;
            _api = new HttpService();
            _authService = new AuthService(HttpService.Client);
            RegisterCommand = new RelayCommand(async _ => await Register());
            GoToLoginCommand = new RelayCommand(_ =>
                _navigation.CurrentView = new LoginVM(_navigation));
        }

        public RelayCommand RegisterCommand { get; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        private async Task Register()
        {
            if (string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Fill all fields");
                return;
            }

            if (Password != ConfirmPassword)
            {
                MessageBox.Show("Passwords do not match");
                return;
            }

            var success = await _authService.RegisterAsync(Username, Password);

            if (!success)
            {
                MessageBox.Show("Registration failed");
                return;
            }

            MessageBox.Show("Account created successfully");

            _navigation.CurrentView = new LoginVM(_navigation);
        }
    }
}