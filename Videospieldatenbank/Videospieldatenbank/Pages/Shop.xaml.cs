using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
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

        private void ButtonAddGame_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName.EndsWith(".exe") &&
                ChromiumWebBrowser.Address.Contains("https://www.igdb.com/games/"))
                LoginWindow.UserDatabaseConnector.AddGame(ChromiumWebBrowser.Address, openFileDialog.FileName);
        }
    }
}