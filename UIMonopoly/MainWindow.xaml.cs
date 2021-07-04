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
        private ServerConcurrent.Controller m_serverController;
        private Client.ClientController m_client;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void RegistrationAlmostDoneButton_OnClick(object sender, RoutedEventArgs e)
        {
            RegistrationPanel.Visibility = Visibility.Collapsed;
            if (IsHostRadioB.IsChecked == true)
            {
                HostRegistrationPanel.Visibility = Visibility.Visible;
                m_serverController = new ServerConcurrent.Controller();
                //await m_serverController.Run();
            }
            else
            {
                ClientPanel.Visibility = Visibility.Visible;
                var quit = false;
                m_client = new Client.ClientController();
                /*while (!quit)
                {
                    await m_client.Run();
                }*/
            }
        }
        
        private void CreateGameButton_OnClick(object sender, RoutedEventArgs e)
        {
            HostRegistrationPanel.Visibility = Visibility.Collapsed;
            GameBoard.Visibility = Visibility.Visible;
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