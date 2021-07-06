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
using CommonUI;

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
            TextDataPresenter dataPresenter = new TextDataPresenter();

            RegistrationPanel.Visibility = Visibility.Collapsed;
            if (IsHostRadioB.IsChecked == true)
            {
                HostRegistrationPanel.Visibility = Visibility.Visible;
                m_serverController = new ServerConcurrent.Controller();

                dataPresenter.TextBoxForMessages = TextBoxOnBoardForData;
                m_serverController.DataPresenter = dataPresenter;
                m_serverController.PlayerName = UserNameTextBox.Text;
            }
            else
            {
                ClientPanel.Visibility = Visibility.Visible;
                var quit = false;
                m_client = new Client.ClientController();

                dataPresenter.TextBoxForMessages = TextBoxOnClientFormForData;
                m_client.DataPresenter = dataPresenter;
                m_client.PlayerName = UserNameTextBox.Text;

                while (!quit)
                {
                    await m_client.Run(QuitGame);
                }
            }
        }
        
        private async void CreateGameButton_OnClick(object sender, RoutedEventArgs e)
        {
            HostRegistrationPanel.Visibility = Visibility.Collapsed;
            GameBoard.Visibility = Visibility.Visible;
            m_serverController.NumberOfPlayers = Int32.Parse(PlayersQuantityTextBox.Text);
            m_serverController.ServerKey = KeyTextBox.Text;
            await m_serverController.Run();
        }

        private void YesButton_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You pressed 'Yes'");
        }
        
        private void NoButton_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You pressed 'No'");
        }

        private async void HostQuitGameButton_Click(object sender, RoutedEventArgs e)
        {
            await m_serverController.QuitGame(QuitGame);
        }
        private void QuitGame(bool Quit)
        {
            if (Quit == true)
                Close();
        }
    }
}