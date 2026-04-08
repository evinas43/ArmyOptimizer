using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ArmyOptimizer.Services;
using ArmyOptimizer.Utilities;
namespace ArmyOptimizer.ViewModels
{
    public class SubscriptionsVM : ViewModelBase
    {
        private readonly NavigationVM _navigation;
       
        private readonly SubscriptionsService _subscriptionsService;
        public RelayCommand BackCommand { get; }
        public ICommand BuyTokensCommand { get; }

        //constructor
        public SubscriptionsVM(NavigationVM navigation)
        {
            _navigation = navigation;
            _subscriptionsService = new SubscriptionsService();

            BackCommand = new RelayCommand(_ =>
            {
                var homeVM = new HomeVM(_navigation);

                _navigation.CurrentView = homeVM;

                _ = homeVM.RefreshTokens();
            });

            BuyTokensCommand = new RelayCommand(async (param) =>
            {
                int tokens = int.Parse(param.ToString());
                await _subscriptionsService.BuyTokens(tokens);
            });

        }
       
    }
}
