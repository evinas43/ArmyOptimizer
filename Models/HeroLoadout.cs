using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ArmyOptimizer.Models
{
    public class HeroLoadout
    {
        public string? HeroName { get; set; }
        public List<string>? Equipment { get; set; }
        public string? PetName { get; set; }

        public BitmapImage Image { get; set; }

        public BitmapImage PetImage { get; set; }


    }
}
