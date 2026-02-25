using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ArmyOptimizer.Models
{
    public class Army
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int TownHall { get; set; }

        public List<Troop>? Troops { get; set; }
        public List<Spell>? Spells { get; set; }
        public List<string>? Heroes { get; set; }
        public List<HeroLoadout>? HeroLoadouts { get; set; }

        public string? SiegeMachine { get; set; }
        public string? AiNotes { get; set; }

    }
}
