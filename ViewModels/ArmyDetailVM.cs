using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmyOptimizer.Models;
using ArmyOptimizer.Services;
using ArmyOptimizer.Utilities;

namespace ArmyOptimizer.ViewModels
{
    public class ArmyDetailVM : ViewModelBase
    {
        private readonly NavigationVM _navigation;
        private readonly ArmyService _armyService;

        public int HeroesCount => Army?.Heroes?.Count ?? 0;
        public int TroopsCount => Army?.Troops?.Sum(t => t.Quantity) ?? 0;
        public int SpellsCount => Army?.Spells?.Sum(s => s.Quantity) ?? 0;
        public int SiegeCount => string.IsNullOrEmpty(Army?.SiegeMachine) ? 0 : 1;

        private Army? _army;
        public Army? Army
        {
            get => _army;
            set
            {
                _army = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand GoBackCommand { get; }

        public ArmyDetailVM(NavigationVM navigation, int armyId)
        {
            _navigation = navigation;
            _armyService = new ArmyService(HttpService.Client);

            GoBackCommand = new RelayCommand(_ =>
                _navigation.CurrentView = new HomeVM(_navigation));

            LoadArmy(armyId);
        }

        private async void LoadArmy(int id)
        {
            Army = await _armyService.GetUserArmiesByIDAsync(id);

            OnPropertyChanged(nameof(HeroesCount));
            OnPropertyChanged(nameof(TroopsCount));
            OnPropertyChanged(nameof(SpellsCount));
            OnPropertyChanged(nameof(SiegeCount));
        }
    }
}
