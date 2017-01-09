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
        private string _friendsListName;

        public Friends()
        {
            InitializeComponent();
            RefreshFriendsList();
        }

        /// <summary>
        /// Aktualisiert die FriendsList und füg die Menuitems Profil und Löschen hinzu,
        /// welche das Profil eines Freundes anzeigt oder einen Freund löscht.
        /// </summary>
        private void RefreshFriendsList()
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

        /// <summary>
        /// Speichert beim rechtsklicken auf einen Freund dessen Namen zwischen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem listBoxItem = sender as ListBoxItem;
            _friendsListName = listBoxItem.Content as string;
        }

        /// <summary>
        /// Öffnet das Profil vom ausgewählten Freund.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemProfil_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FriendsProfilWindow friendsProfilWindow =
                        new FriendsProfilWindow(LoginWindow.UserDatabaseConnector.GetId(_friendsListName));
                friendsProfilWindow.Show();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Löscht den ausgwählten Freund.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoginWindow.UserDatabaseConnector.RemoveFriend(LoginWindow.UserDatabaseConnector.GetId(_friendsListName));
            }
            catch (Exception)
            {
            }
            RefreshFriendsList();
        }

        /// <summary>
        /// Setzt das Objekt vom "Friends"-Window in MainWindow beim Schließen des Fensters gleich null. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Friends_OnClosed(object sender, EventArgs e)
        {
            MainWindow.friends = null;
        }

        /// <summary>
        /// Fügt einen Vorhandenen User als Freund hinzu, welcher in durch die Textbox ausgwählt wurde. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            RefreshFriendsList();
        }
    }
}