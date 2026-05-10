using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ArmyOptimizer.Utilities;

namespace ArmyOptimizer.Models
{
    public class SelectableHero : ViewModelBase
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        // bitmap for the UI to display, loaded from ImageUrl

        private BitmapImage _image;
        public BitmapImage Image
        {
            get => _image;
            set
            {
                if (_image == value) return;

                _image = value;
                OnPropertyChanged();
            }
        }
        //townhall requirment to see the troop 
        public int RequiredTownHall { get; set; }


        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }
    }
}
