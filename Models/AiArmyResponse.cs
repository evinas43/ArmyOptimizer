using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmyOptimizer.Models
{
    public class AiArmyResponse
    {
        public int townHall { get; set; }
        public List<ArmyUnit> troops { get; set; }
        public List<ArmyUnit> spells { get; set; }
        public List<string> heroes { get; set; }
        public List<HeroLoadout> heroLoadouts { get; set; }
        public string siegeMachine { get; set; }
        public string aiNotes { get; set; }
    }
}
