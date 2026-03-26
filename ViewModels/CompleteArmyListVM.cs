using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmyOptimizer.Models;
using ArmyOptimizer.Services;
using ArmyOptimizer.Utilities;

namespace ArmyOptimizer.ViewModels
{
    public class CompleteArmyListVM : ViewModelBase
    {
        private readonly NavigationVM _navigation;
        private readonly ArmyService _armyService;
        public ObservableCollection<ArmySummary> Armies { get; set; } = new();
        public RelayCommand GoBackHomeCommand { get; }
        public RelayCommand<ArmySummary> OpenArmyCommand { get; }

        public CompleteArmyListVM(NavigationVM navigation)
        {
            _navigation = navigation;
            _armyService = new ArmyService(HttpService.Client);

            GoBackHomeCommand= new RelayCommand(_ => GoBackHome());

            OpenArmyCommand = new RelayCommand<ArmySummary>(army =>
            {
                _navigation.CurrentView = new ArmyDetailVM(_navigation, army.Id);
            });

            LoadArmies();
        }


        private async void LoadArmies()
        {
            var armies = await _armyService.GetUserArmiesAsync();

            if (armies == null)
                return;

            Armies.Clear();

            foreach (var army in armies
                                   .OrderByDescending(a => a.CreatedAt)) //order by created date
            {
                Armies.Add(army);
            }
        }

        private async void GoBackHome()
        {
            _navigation.CurrentView = new HomeVM(_navigation);
        }
    }
}
