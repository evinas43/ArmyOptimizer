using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ArmyOptimizer.Models;
using ArmyOptimizer.Services;
using ArmyOptimizer.Utilities;

namespace ArmyOptimizer.ViewModels
{
    public class SaveArmyVM : ViewModelBase
    {
        private readonly NavigationVM _navigation;
        private readonly ArmyService _armyService;
        private readonly ToastService _toastService;

        public string Name { get; set; }
        public string Description { get; set; }

        public AiArmyResponse ArmyData { get; set; }


        //commands
        public RelayCommand SaveCommand { get; }

        //navigation command and usables of the previous state of the optimize form
        public RelayCommand BackCommand { get; }
        private readonly OptimizeArmyVM _previousVM;
        //-------------------------------------------------
        //Constructor 
        public SaveArmyVM(NavigationVM navigation, AiArmyResponse army, OptimizeArmyVM previousVM)
        {
            _navigation = navigation;
            _armyService = new ArmyService(HttpService.Client);
            _toastService = ToastService.Instance;

            ArmyData = army;
            _previousVM = previousVM;

            SaveCommand = new RelayCommand(async _ => await Save());

            BackCommand = new RelayCommand(_ =>
            {
                _navigation.CurrentView = _previousVM;
            });
        }

        private async Task Save()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                _toastService.Show("Army name is required", "warning");
                return;
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                _toastService.Show("Army description is required", "warning");
                return;
            }

            var request = new
            {
                name = Name,
                description = Description,
                townHall = ArmyData.townHall,

                troops = ArmyData.troops.Select(t => new
                {
                    name = t.Name,
                    quantity = t.Quantity
                }).ToList(),

                spells = ArmyData.spells.Select(s => new
                {
                    name = s.Name,
                    quantity = s.Quantity
                }).ToList(),

                heroes = ArmyData.heroes,

                heroLoadouts = ArmyData.heroLoadouts.Select(h => new
                {
                    heroName = h.HeroName,
                    equipment = h.Equipment,
                    petName = h.PetName
                }).ToList(),

                siegeMachines = ArmyData.siegeMachines.Select(s => new
                {
                    name = s.Name,
                    quantity = s.Quantity
                }).ToList(),

                aiNotes = ArmyData.aiNotes
            };

            var result = await _armyService.SaveUserOptimizedArmy(request);

            if (result == null)
            {
                _toastService.Show("Error saving army", "error");
                return;
            }

            _navigation.CurrentView = new HomeVM(_navigation);
        }
    }
}
