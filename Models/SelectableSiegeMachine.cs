using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ArmyOptimizer.Utilities;

namespace ArmyOptimizer.Models
{
    public class SelectableSiegeMachine : ViewModelBase
    {
        public string Name { get; set; }
        public int MachineHousing { get; set; }
        public string ImageUrl { get; set; }

        // Bitmap image loaded asynchronously to avoid UI delays and missing images

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
                OnPropertyChanged(nameof(TotalHousing));
            }
        }

        public int TotalHousing => Quantity * MachineHousing;

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
