using GraphiGenius.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace GraphiGenius
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void PreviewNumericInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }
        private void PreviewNumericInputMonths(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(((TextBox)sender).Text + e.Text, out int newValue))
            {
                e.Handled = true;
            }
            else if (newValue < 1 || newValue > 12)
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }
        private void PreviewNumericInputMinutes(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(((TextBox)sender).Text + e.Text, out int newValue))
            {
                e.Handled = true;
            }
            else if (newValue < 0 || newValue > 59)
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }
        private void PreviewNumericInputHours(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(((TextBox)sender).Text + e.Text, out int newValue))
            {
                e.Handled = true;
            }
            else if (newValue < 0 || newValue > 23)
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }
        private void BorderMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
        private void MaximizeClick(object sender, RoutedEventArgs e)
        {
            if(Application.Current.MainWindow.WindowState != WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current?.MainWindow?.Close();
        }


    }
}
