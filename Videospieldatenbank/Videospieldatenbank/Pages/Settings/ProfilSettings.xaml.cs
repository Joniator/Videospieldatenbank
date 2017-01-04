using System;
using System.Collections.Generic;
using System.Globalization;
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
using Microsoft.Win32;
using Videospieldatenbank.Windows;
using Xceed.Wpf.AvalonDock.Converters;
using Xceed.Wpf.DataGrid.Converters;
using System.Drawing;
using Videospieldatenbank.Utils;

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

            UserInfos();
        }

        public void UserInfos(int userId)
        {
            if (userId != LoginWindow.UserDatabaseConnector.UserId)
            {
                StackPanelSettings.IsEnabled = false;
                StackPanelSettings.Visibility = Visibility.Collapsed;
            }
            else if (userId == LoginWindow.UserDatabaseConnector.UserId)
            {
                StackPanelSettings.IsEnabled = true;
                StackPanelSettings.Visibility = Visibility.Visible;
            }

            try

            {
                ImageProfil.Source = ImageUtils.BytesToImageSource(LoginWindow.UserDatabaseConnector.GetProfilePicture(userId));
            }
            catch (Exception)
            {

            }

            ListBoxItemUserName.Content = "Username: " +
                                          LoginWindow.UserDatabaseConnector.GetUsername(userId);

            if (LoginWindow.UserDatabaseConnector.OnlineStatus)
                ListBoxItemOnlineStatus.Content = "Online: Yes";
            else
                ListBoxItemOnlineStatus.Content = "Online: No";

            ListBoxItemFriends.Content = "Friends: " + LoginWindow.UserDatabaseConnector.GetFriendsList().Count;
            List<string> games = LoginWindow.UserDatabaseConnector.GetGames();
            ListBoxItemTotalGames.Content = "Total games: " + games.Count;
            ListBoxItemTotalPlaytime.Content = "Total gametime: " + games.Select(n => LoginWindow.UserDatabaseConnector.GetPlayTime(n).TotalMinutes).Sum() + " Minutes";
        }

        public void UserInfos()
        {
            UserInfos(LoginWindow.UserDatabaseConnector.UserId);
        }
        
        private void ButtonSetPicture_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            //openFileDialog.Filter = "jpg";
            openFileDialog.ShowDialog();

            try
            {
                LoginWindow.UserDatabaseConnector.ProfilePicture = File.ReadAllBytes(openFileDialog.FileName);
                UserInfos();
            }
            catch (Exception)
            {

            }
        }

        private void ButtonChangeUsername_Click(object sender, RoutedEventArgs e)
        {
            ChangeUsernameDialog dialog = new ChangeUsernameDialog();
            if (dialog.ShowDialog() == true) MessageBox.Show("Username changed!");
            UserInfos();
        }

        private void ButtonGoOnOff_Click(object sender, RoutedEventArgs e)
        {
            if (LoginWindow.UserDatabaseConnector.OnlineStatus)
            {
                LoginWindow.UserDatabaseConnector.OnlineStatus = false;
                (sender as Button).Content = "Go online";
            }
            else
            {
                LoginWindow.UserDatabaseConnector.OnlineStatus = true;
                (sender as Button).Content = "Go offline";
            }
            UserInfos();
        }

        private void ButtonChangePassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordDialog dialog = new ChangePasswordDialog();
            if (dialog.ShowDialog() == true) MessageBox.Show("Password changed!");
            UserInfos();
        }
    }
}
