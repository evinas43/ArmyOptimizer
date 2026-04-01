using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ArmyOptimizer.ViewModels;

namespace ArmyOptimizer.Views
{
    /// <summary>
    /// Lógica de interacción para OptimizeArmyView.xaml
    /// </summary>
    public partial class OptimizeArmyView : UserControl
    {
        public OptimizeArmyView()
        {
            InitializeComponent();

            Loaded += async (s, e) =>
            {
                if (DataContext is OptimizeArmyVM vm)
                {
                    await vm.InitializeAsync();
                }
            };
        }


    }
}
