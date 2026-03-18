using System.Windows;
using ArmyOptimizer.Models;
using ArmyOptimizer.Services;
using ArmyOptimizer.Utilities;

namespace ArmyOptimizer.ViewModels
{
    public class LoginVM : ViewModelBase
    {
        private readonly NavigationVM _navigation;
        private readonly AuthService _auth;

        public LoginVM(NavigationVM navigation)
        {
            _navigation = navigation;
            _auth = new AuthService(HttpService.Client);

            LoginCommand = new RelayCommand(async _ => await Login());
        }

        public RelayCommand LoginCommand { get; }
        public RelayCommand GoToRegisterCommand { get; }

        public string Username { get; set; }
        public string Password { get; set; }

        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Fill all fields");
                return;
            }

            try
            {
                IsLoading = true;  

                var token = await _auth.LoginAsync(Username, Password);

                if (token == null)
                {
                    MessageBox.Show("Invalid credentials");
                    return;
                }

                SessionUser.Username = Username;
                SessionUser.token = token;

                HttpService.SetToken(token);

                _navigation.CurrentView = new HomeVM(_navigation);
            }
            finally
            {
                IsLoading = false;  
            }
        }

        // spinner
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

    }

}