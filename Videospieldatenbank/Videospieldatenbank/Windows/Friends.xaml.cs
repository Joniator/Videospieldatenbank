using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Videospieldatenbank.Pages.Settings;
using Videospieldatenbank.Windows;

namespace Videospieldatenbank
{
    /// <summary>
    ///     Interaktionslogik für Friends.xaml
    /// </summary>
    public partial class Friends : Window
    {
        private readonly ProfilSettings _profilSettings = new ProfilSettings();
        private string FriendsListName;

        public Friends()
        {
            InitializeComponent();
            refreshFriendsList();
        }

        private void refreshFriendsList()
        {
            ContextMenu contextMenu = new ContextMenu();
            MenuItem menuItemProfil = new MenuItem {Header = "Profil"};
            menuItemProfil.Click += MenuItemProfil_Click;
            contextMenu.Items.Add(menuItemProfil);

            MenuItem menuItemDelete = new MenuItem {Header = "Löschen"};
            menuItemDelete.Click += MenuItemDelete_Click;
            contextMenu.Items.Add(menuItemDelete);

            try
            {
                ListBoxFriends.Items.Clear();
                foreach (int friend in LoginWindow.UserDatabaseConnector.GetFriendsList())
                {
                    ListBoxItem listBoxItem = new ListBoxItem
                    {
                        Content = LoginWindow.UserDatabaseConnector.GetUsername(friend),
                        Foreground = Brushes.Black,
                        ContextMenu = contextMenu
                    };
                    listBoxItem.PreviewMouseRightButtonDown += ListBoxItem_MouseRightButtonDown;
                    ListBoxFriends.Items.Add(listBoxItem);
                }
            }
            catch (Exception)
            {
            }
        }

        private void ListBoxItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem listBoxItem = sender as ListBoxItem;
            FriendsListName = listBoxItem.Content as string;
        }

        private void MenuItemProfil_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FriendsProfilWindow friendsProfilWindow =
                        new FriendsProfilWindow(LoginWindow.UserDatabaseConnector.GetId(FriendsListName));
                friendsProfilWindow.Show();
            }
            catch (Exception)
            {
            }
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoginWindow.UserDatabaseConnector.RemoveFriend(LoginWindow.UserDatabaseConnector.GetId(FriendsListName));
            }
            catch (Exception)
            {
            }
            refreshFriendsList();
        }

        private void Friends_OnClosed(object sender, EventArgs e)
        {
            MainWindow.friends = null;
        }

        private void ButtonAddFriend_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoginWindow.UserDatabaseConnector.AddFriend(
                                                            LoginWindow.UserDatabaseConnector.GetId(
                                                                                                    TextBoxFriendName
                                                                                                            .Text));
            }
            catch (Exception)
            {
            }
            refreshFriendsList();
        }
    }
}