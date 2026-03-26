using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ArmyOptimizer.Utilities;

namespace ArmyOptimizer.Models
{
    public class DisplayUnit : ViewModelBase
    {
        public string Name { get; set; }
        public int Quantity { get; set; }

        private BitmapImage _image;
        public BitmapImage Image
        {
            get => _image;
            set { _image = value; OnPropertyChanged(); }
        }
    }
}
