using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Videospieldatenbank.Utils;
using Videospieldatenbank.Windows;

namespace Videospieldatenbank.Pages.Settings
{
    /// <summary>
    ///     Interaktionslogik für ProfilSettings.xaml
    /// </summary>
    public partial class ProfilSettings : Page
    {
        public ProfilSettings()
        {
            InitializeComponent();

            UserInfos();
        }

        /// <summary>
        ///     Füllt die Informationen der entsprechenden User aus.
        /// </summary>
        /// <param name="userId">Die Id des entsprechenden User</param>
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
                ImageProfil.Source =
                        ImageUtils.BytesToImageSource(LoginWindow.UserDatabaseConnector.GetProfilePicture(userId));
            }
            catch (Exception)
            {
            }

            ListBoxItemUserName.Content = "Username: " + LoginWindow.UserDatabaseConnector.GetUsername(userId);

            ListBoxItemOnlineStatus.Content = LoginWindow.UserDatabaseConnector.IsOnline(userId)
                                                  ? "Online"
                                                  : "Offline";

            ListBoxItemFriends.Content = "Freunde: " + LoginWindow.UserDatabaseConnector.GetFriendsList(userId).Count;
            List<string> games = LoginWindow.UserDatabaseConnector.GetGames(userId);
            ListBoxItemTotalGames.Content = "Spiele: " + games.Count;
            ListBoxItemTotalPlaytime.Content = "Spielzeit: " +
                                               games.Select(
                                                            n =>
                                                                LoginWindow.UserDatabaseConnector.GetPlayTime(n, userId)
                                                                           .TotalMinutes).Sum() + " Minutes";
        }

        /// <summary>
        ///     Wird nur für den Mainuser verwendet.
        /// </summary>
        public void UserInfos()
        {
            UserInfos(LoginWindow.UserDatabaseConnector.UserId);
        }

        /// <summary>
        ///     Setzt das Profilbild des aktuellen Users.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSetPicture_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
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

        /// <summary>
        ///     Ändert den Username des aktuellen Users.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonChangeUsername_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChangeUsernameDialog dialog = new ChangeUsernameDialog();
                if (dialog.ShowDialog() == true) MessageBox.Show("Username geändert!");
                UserInfos();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        ///     Setzt den Onlinestatus des aktuellen Users auf Offline.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonGoOnOff_Click(object sender, RoutedEventArgs e)
        {
            if (LoginWindow.UserDatabaseConnector.OnlineStatus)
            {
                LoginWindow.UserDatabaseConnector.OnlineStatus = false;
                (sender as Button).Content = "Online anzeigen";
            }
            else
            {
                LoginWindow.UserDatabaseConnector.OnlineStatus = true;
                (sender as Button).Content = "Offline anzeigen";
            }
            UserInfos();
        }

        /// <summary>
        ///     Ändert das Passwort des aktuellen Users.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonChangePassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordDialog dialog = new ChangePasswordDialog();
            if (dialog.ShowDialog() == true) MessageBox.Show("Passwort geändert!");
            UserInfos();
        }
    }
}