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
using ArmyOptimizer.ViewModels;

namespace ArmyOptimizer.Views
{
    /// <summary>
    /// Lógica de interacción para LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
            ToastService.Instance.OnShowToast += ShowToast;

            Unloaded += (s, e) =>
            {
            ToastService.Instance.OnShowToast -= ShowToast;
            };
        }
        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginVM vm)
                vm.Password = ((PasswordBox)sender).Password;
        }

        public async void ShowToast(string message, string type = "info")
        {
            ToastText.Text = message;

            // icon + color según tipo
            switch (type)
            {
             
                case "error":
                    Toast.Background = new SolidColorBrush(
                       (Color)ColorConverter.ConvertFromString("#CCF87171"));
                    Toast.BorderBrush = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#DC2626"));
                    ToastIcon.Text = "❌";
                    ToastIcon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
                    ToastIcon.FontSize = 12;

                    var shake = new DoubleAnimation
                    {
                        From = -5,
                        To = 5,
                        Duration = TimeSpan.FromMilliseconds(50),
                        AutoReverse = true,
                        RepeatBehavior = new RepeatBehavior(3)
                    };

                    Toast.RenderTransform = new TranslateTransform();
                    Toast.RenderTransform.BeginAnimation(TranslateTransform.XProperty, shake);

                    break;


                case "warning": 
                    Toast.Background = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#CCFB923C"));
                    Toast.BorderBrush = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#EA580C"));
                    ToastIcon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff"));
                    ToastIcon.Text = "⚠️";
                    ToastIcon.FontSize = 12;

                    var shakeWarning = new DoubleAnimation
                    {
                        From = -2,
                        To = 2,
                        Duration = TimeSpan.FromMilliseconds(50),
                        AutoReverse = true,
                        RepeatBehavior = new RepeatBehavior(3)
                    };

                    Toast.RenderTransform = new TranslateTransform();
                    Toast.RenderTransform.BeginAnimation(TranslateTransform.XProperty, shakeWarning);


                    break;

                default:
                    Toast.Background = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#CC93C5FD"));
                    Toast.BorderBrush = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#1D4ED8"));
                    ToastIcon.Text = "⚠️";
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
