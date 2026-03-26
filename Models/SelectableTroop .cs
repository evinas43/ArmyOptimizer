using System.Windows.Input;
using System.Windows.Media.Imaging;
using ArmyOptimizer.Utilities;

namespace ArmyOptimizer.Models
{
    public class SelectableTroop : ViewModelBase
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int HousingSpace { get; set; }
        public RelayCommand IncreaseCommand { get; set; }
        public RelayCommand DecreaseCommand { get; set; }

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

        public int TotalHousing => Quantity * HousingSpace;

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