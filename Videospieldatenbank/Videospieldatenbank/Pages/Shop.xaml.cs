using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
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
using Microsoft.Win32;
using Videospieldatenbank.Database;
using Videospieldatenbank.Windows;

namespace Videospieldatenbank.Pages
{
    /// <summary>
    /// Interaktionslogik für Shop.xaml
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
            if(openFileDialog.FileName.EndsWith(".exe") && ChromiumWebBrowser.Address.Contains("https://www.igdb.com/games/"))
                LoginWindow.UserDatabaseConnector.AddGame(ChromiumWebBrowser.Address, openFileDialog.FileName);
        }
    }
}
