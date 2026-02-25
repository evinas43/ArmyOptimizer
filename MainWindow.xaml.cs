using System.Windows;
using ArmyOptimizer.ViewModels;

namespace ArmyOptimizer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new NavigationVM();
        }
    }
}
