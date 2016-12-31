using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Videospieldatenbank.Database;
using Videospieldatenbank.Windows;

namespace Videospieldatenbank
{
    /// <summary>
    ///     Interaktionslogik für Friends.xaml
    /// </summary>
    public partial class Friends : Window
    {
        public Friends()
        {
            InitializeComponent();
            refreshFriendsList();
        }

        private void refreshFriendsList()
        {
            try
            {
                ListBoxFriends.Items.Clear();
                foreach (var friend in LoginWindow.UserDatabaseConnector.GetFriendsList())
                {
                    ListBoxFriends.Items.Add(new ListBoxItem
                    {
                        Content = LoginWindow.UserDatabaseConnector.GetUsername(friend),
                        Foreground = Brushes.WhiteSmoke
                    });
                }
            }
            catch (Exception)
            {
                
            }
        }

        private void Friends_OnClosed(object sender, EventArgs e)
        {
            MainWindow.friends = null;
        }

        private void ButtonAddFriend_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                LoginWindow.UserDatabaseConnector.AddFriend(LoginWindow.UserDatabaseConnector.GetId(TextBoxFriendName.Text));
            }
            catch (Exception)
            {
                
            }
            refreshFriendsList();
        }
    }
}