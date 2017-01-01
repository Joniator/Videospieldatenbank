using System;
using System.Collections.Generic;
using System.IO;
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
using Videospieldatenbank.Database;
using Videospieldatenbank.Windows;
using Xceed.Wpf.DataGrid.Converters;

namespace Videospieldatenbank.Pages.Settings
{
    /// <summary>
    /// Interaktionslogik für ProfilSettings.xaml
    /// </summary>
    public partial class ProfilSettings : Page
    {
        public ProfilSettings()
        {
            InitializeComponent();

            UserInfos(LoginWindow.UserDatabaseConnector.UserId);
        }

        public void UserInfos(int userId)
        {
            try
            {
                ImageProfil.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(LoginWindow.UserDatabaseConnector.GetProfilePicture(userId));
            }
            catch (Exception)
            {

            }

            ListBoxItemUserName.Content = "Username: " +
                LoginWindow.UserDatabaseConnector.GetUsername(userId);

            if (LoginWindow.UserDatabaseConnector.Connected)
                ListBoxItemOnlineStatus.Content = "Online: Yes";
            else
                ListBoxItemOnlineStatus.Content += "Online: No";

            ListBoxItemFriends.Content = "Friends: " + LoginWindow.UserDatabaseConnector.GetFriendsList().Count;
            ListBoxItemTotalGames.Content = "Total games: " + LoginWindow.UserDatabaseConnector.GetGames().Count;
        }

        private void ButtonSetPicture_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
