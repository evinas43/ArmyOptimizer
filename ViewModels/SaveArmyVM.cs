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
            MessageBox.Show($"TH: {ArmyData?.townHall}\nTroops: {ArmyData?.troops?.Count}\nSpells: {ArmyData?.spells?.Count}");

            foreach (var h in ArmyData.heroLoadouts)
            {
                MessageBox.Show($"{h.HeroName} → Equipments: {h.Equipment?.Count}");
            }
            MessageBox.Show($"SiegeMachine: {ArmyData.siegeMachine ?? "NULL"}");
            var request = new
            {
                name = Name,
                description = Description,
                townHall = ArmyData.townHall,

                // Troops saved explicitly
                troops = ArmyData.troops.Select(t => new
                {
                    name = t.Name,
                    quantity = t.Quantity
                }),

                //spells saved explicitly
                spells = ArmyData.spells.Select(s => new
                {
                    name = s.Name,
                    quantity = s.Quantity
                }),

                heroes = ArmyData.heroes,

                //heroe loadouts saved with details 
                heroLoadouts = ArmyData.heroLoadouts.Select(h => new
                {
                    heroName = h.HeroName,
                    equipment = h.Equipment,
                    petName = h.PetName
                }),

                siegeMachine = ArmyData.siegeMachine,

                aiNotes = ArmyData.aiNotes
            };

            var result = await _armyService.SaveUserOptimizedArmy(request);

            if (result == null)
            {
                MessageBox.Show("Error saving army");
                return;
            }

            MessageBox.Show("Army saved ✔️");

            _navigation.CurrentView = new HomeVM(_navigation);
        }
    }
}
