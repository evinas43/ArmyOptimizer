using ArmyOptimizer.Utilities;
using System.Windows.Input;

namespace ArmyOptimizer.Models
{
    public class SelectableTroop : ViewModelBase
    {
        public string Name { get; set; }
        public int HousingSpace { get; set; }
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