using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmyOptimizer.Utilities;

namespace ArmyOptimizer.Models
{
    public class SelectableSpell : ViewModelBase
    {
        
        public string Name { get; set; }
        public int SpellingHousing { get; set; }
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

