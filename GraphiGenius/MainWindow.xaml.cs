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



    }
}
