using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ArmyOptimizer.Utilities;

namespace ArmyOptimizer.Models
{
    public class SelectableSpell : ViewModelBase
    {
        
        public string Name { get; set; }
        public int SpellingHousing { get; set; }
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
        public RelayCommand IncreaseCommand { get; set; }
        public RelayCommand DecreaseCommand { get; set; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalSpellHousing));
            }
        }
        //townhall requirment to see the troop 
        public int RequiredTownHall { get; set; }

        public int TotalSpellHousing => Quantity * SpellingHousing;

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

