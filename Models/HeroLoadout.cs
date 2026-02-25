using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmyOptimizer.Models
{
    public class HeroLoadout
    {
        public string? HeroName { get; set; }
        public List<string>? Equipment { get; set; }
        public string? PetName { get; set; }
    }
}
