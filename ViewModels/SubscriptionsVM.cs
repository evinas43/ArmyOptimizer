using System.Collections.ObjectModel;
using System.Windows.Input;
using ArmyOptimizer.Models;
using ArmyOptimizer.Services;
using ArmyOptimizer.Utilities;

namespace ArmyOptimizer.ViewModels;

public class SubscriptionsVM : ViewModelBase
{
    private readonly NavigationVM _navigation;
    private readonly SubscriptionsService _subscriptionsService;

    public ObservableCollection<PaymentHistory> Payments { get; set; } = new();

    public RelayCommand BackCommand { get; }
    public ICommand BuyTokensCommand { get; }

    public SubscriptionsVM(NavigationVM navigation)
    {
        _navigation = navigation;
        _subscriptionsService = new SubscriptionsService(HttpService.Client);

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

        _ = LoadPayments();
    }

    private async Task LoadPayments()
    {
        var data = await _subscriptionsService.GetPayments();

        Payments.Clear();

        if (data != null)
        {
            foreach (var p in data)
                Payments.Add(p);
        }
    }
}