using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Videospieldatenbank.Database;
using Videospieldatenbank.Windows;

namespace Videospieldatenbank.Pages
{
    /// <summary>
    ///     Interaktionslogik für Shop.xaml
    /// </summary>
    public partial class Shop : Page
    {
        public Shop()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Fügt ein Spiel vom Browser der Datenbank hinzu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAddGame_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName.EndsWith(".exe") &&
                ChromiumWebBrowser.Address.Contains("https://www.igdb.com/games/"))
            {
                LoginWindow.UserDatabaseConnector.AddGame(ChromiumWebBrowser.Address, openFileDialog.FileName);
                using (GameDatabaseConnector gameDatabaseConnector = new GameDatabaseConnector())
                {
                    if (LoginWindow.UserDatabaseConnector.OwnsGame(ChromiumWebBrowser.Address))
                        MessageBox.Show(
                                        gameDatabaseConnector.GetGameInfo(ChromiumWebBrowser.Address).Name +
                                        " erfolgreich hinzugefügt!", "");
                }
            }
        }
    }
}