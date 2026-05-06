using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ArmyOptimizer.Models
{
    public class ArmyUnit
    {
        public string Name { get; set; }
        public int Quantity { get; set; }

        public BitmapImage Image { get; set; }

    }
}
