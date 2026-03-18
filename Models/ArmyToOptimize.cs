using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmyOptimizer.Models
{
    public class ArmyToOptimize
    {
        public int TownHallLevel { get; set; }

        public List<string> SelectedHeroes { get; set; } = new();

        public List<ArmyUnit> SelectedTroops { get; set; } = new();

        public List<ArmyUnit> SelectedSpells { get; set; } = new();

        public List<ArmyUnit> SelectedSiegeMachines { get; set; } = new();
    }
}
