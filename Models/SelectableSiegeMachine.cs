using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmyOptimizer.Utilities;

namespace ArmyOptimizer.Models
{
    public class SelectableSiegeMachine : ViewModelBase
    {
        public string Name { get; set; }
        public int MachineHousing { get; set; }
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
