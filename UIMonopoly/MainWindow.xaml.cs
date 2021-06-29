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

namespace UIMonopoly
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

        private void RegistrationAlmostDoneButton_OnClick(object sender, RoutedEventArgs e)
        {
            RegistrationPanel.Visibility = Visibility.Collapsed;
            if (IsHostRadioB.IsChecked == true)
            {
                RegistrationHostPanel.Visibility = Visibility.Visible;
            }
            else
            {
                ClientPanel.Visibility = Visibility.Visible;
            }
        }
        
        private void CreateGameButton_OnClick(object sender, RoutedEventArgs e)
        {
            RegistrationHostPanel.Visibility = Visibility.Hidden;
            HostPanel.Visibility = Visibility.Visible;
        }

        private void YesButton_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You pressed 'Yes'");
        }
        
        private void NoButton_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You pressed 'No'");
        }
    }
}