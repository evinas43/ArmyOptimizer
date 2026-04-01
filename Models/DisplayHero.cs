using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ArmyOptimizer.Utilities;

namespace ArmyOptimizer.Models
{
    public class DisplayHero : ViewModelBase
    {
        public string Name { get; set; }

        private BitmapImage _image;
        public BitmapImage Image
        {
            get => _image;
            set { _image = value; OnPropertyChanged(); }
        }

        public List<string> Equipment { get; set; } = new();
        public string PetName { get; set; }
        public BitmapImage PetImage { get; set; }
    }
}
