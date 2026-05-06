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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ArmyOptimizer.Services;

namespace ArmyOptimizer.Views
{
    /// <summary>
    /// Lógica de interacción para SaveArmyView.xaml
    /// </summary>
    public partial class SaveArmyView : UserControl
    {
        public SaveArmyView()
        {
            InitializeComponent();

            ToastService.Instance.OnShowToast += ShowToast;

            Unloaded += (s, e) =>
            {
                ToastService.Instance.OnShowToast -= ShowToast;
            };
        }

        public async void ShowToast(string message, string type = "info")
        {
            ToastText.Text = message;

            switch (type)
            {
                case "error":

                    Toast.Background = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#CCF87171"));

                    Toast.BorderBrush = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#DC2626"));

                    ToastIcon.Text = "❌";
                    break;

                case "warning":

                    Toast.Background = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#CCFB923C"));

                    Toast.BorderBrush = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#EA580C"));

                    ToastIcon.Text = "⚠️";
                    break;

                case "success":

                    Toast.Background = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#CC4ADE80"));

                    Toast.BorderBrush = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#16A34A"));

                    ToastIcon.Text = "✅";
                    break;

                default:

                    Toast.Background = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#CC93C5FD"));

                    Toast.BorderBrush = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#1D4ED8"));

                    ToastIcon.Text = "ℹ️";
                    break;
            }

            var show = (Storyboard)Resources["ShowToast"];
            show.Begin();

            await Task.Delay(2500);

            var hide = (Storyboard)Resources["HideToast"];
            hide.Begin();
        }
    }
}
