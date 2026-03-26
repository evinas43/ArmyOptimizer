using System.Text.Json;
using System.Windows;
using ArmyOptimizer.Models;
using ArmyOptimizer.Services;
using ArmyOptimizer.Utilities;
using ArmyOptimizer.Views;

namespace ArmyOptimizer.ViewModels
{
    public class LoginVM : ViewModelBase
    {
        private readonly NavigationVM _navigation;
        private readonly AuthService _auth;
        private readonly UserService _userService;
        private readonly ToastService _toastService;

        public LoginVM(NavigationVM navigation)
        {
            _navigation = navigation;
            _auth = new AuthService(HttpService.Client);
            _userService = new UserService(HttpService.Client);

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
                ToastService.Instance.Show("Fill all fields", "warning");
                return;
            }

            try
            {
                IsLoading = true;  

                var token = await _auth.LoginAsync(Username, Password);

                if (token == null)
                {
                    ToastService.Instance.Show("Invalid credentials", "error");
                    return;
                }

                SessionUser.Username = Username;
                SessionUser.token = token;

                HttpService.SetToken(token);

                var me = await _userService.ME();

                if (me is JsonElement json && json.TryGetProperty("role", out var roleProp))
                {
                    SessionUser.Role = roleProp.GetString();
                }

                // navegar al home
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