using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmyOptimizer.Helpers;
using ArmyOptimizer.Models;
using ArmyOptimizer.Services;
using ArmyOptimizer.Utilities;

namespace ArmyOptimizer.ViewModels
{
    public class ArmyDetailVM : ViewModelBase
    {
        private readonly NavigationVM _navigation;
        private readonly ArmyService _armyService;

        //image colections for heroes, troops, spells, and siege machines
        public ObservableCollection<DisplayUnit> DisplayTroops { get; set; } = new();
        public ObservableCollection<DisplayUnit> DisplaySpells { get; set; } = new();
        public ObservableCollection<DisplayUnit> DisplaySiege { get; set; } = new();
        public ObservableCollection<DisplayUnit> DisplayHeroes { get; set; } = new();


        public int HeroesCount => Army?.Heroes?.Count ?? 0;
        public int TroopsCount => Army?.Troops?.Sum(t => t.Quantity) ?? 0;
        public int SpellsCount => Army?.Spells?.Sum(s => s.Quantity) ?? 0;
        public int SiegeCount => Army?.SiegeMachines?.Sum(s => s.Quantity) ?? 0;
        public string Title => Army?.Name ?? "Unnamed Army";
        public string Description => Army?.Description ?? "";

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

        public Utilities.RelayCommand GoBackCommand { get; }

        public ArmyDetailVM(NavigationVM navigation, int armyId)
        {
            _navigation = navigation;
            _armyService = new ArmyService(HttpService.Client);

            GoBackCommand = new Utilities.RelayCommand(_ =>
                _navigation.CurrentView = new HomeVM(_navigation));

            LoadArmy(armyId);
            

        }

        private async void LoadArmy(int id)
        {
            Army = await _armyService.GetUserArmiesByIDAsync(id);

            await BuildDisplayData();

            OnPropertyChanged(nameof(HeroesCount));
            OnPropertyChanged(nameof(TroopsCount));
            OnPropertyChanged(nameof(SpellsCount));
            OnPropertyChanged(nameof(SiegeCount));
        }

        private async Task BuildDisplayData()
        {
            foreach (var troop in Army.Troops)
            {
                var item = new DisplayUnit
                {
                    Name = troop.Name,
                    Quantity = troop.Quantity
                };

                if (GameImageLibrary.TroopImages.TryGetValue(troop.Name, out var url))
                {
                    item.Image = await ImageCacheService.LoadAsync(url);
                }

                DisplayTroops.Add(item);
            }

            foreach (var spell in Army.Spells)
            {
                var item = new DisplayUnit
                {
                    Name = spell.Name,
                    Quantity = spell.Quantity
                };

                if (GameImageLibrary.SpellImages.TryGetValue(spell.Name, out var url))
                {
                    item.Image = await ImageCacheService.LoadAsync(url);
                }

                DisplaySpells.Add(item);
            }
            foreach (var siege in Army.SiegeMachines)
            {
                var item = new DisplayUnit
                {
                    Name = siege.Name,
                    Quantity = siege.Quantity
                };

                if (GameImageLibrary.SiegeImages.TryGetValue(siege.Name, out var url))
                {
                    item.Image = await ImageCacheService.LoadAsync(url);
                }

                DisplaySiege.Add(item);
            }
            foreach (var hero in Army.Heroes)
            {
                var item = new DisplayUnit
                {
                    Name = hero,
                    Quantity = 1 // siempre 1
                };

                if (GameImageLibrary.HeroImages.TryGetValue(hero, out var url))
                {
                    item.Image = await ImageCacheService.LoadAsync(url);
                }

                DisplayHeroes.Add(item);
            }


        }
    }
}
