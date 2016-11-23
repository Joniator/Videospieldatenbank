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
using MySql.Data.MySqlClient;

namespace Videospieldatenbank
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly GameList _gameList = new GameList();
        private readonly GameInfo _gameInfo = new GameInfo();
        public static Friends friends = null;

        public MainWindow()
        {
            //new DatabaseInteraction("tazed.tk", 3306, "gamedatabase", "igdb", "1337");
        }

        private void ButtonLibrary_OnClick(object sender, RoutedEventArgs e)
        {
            if (FrameLeft.Content != _gameList)
                FrameLeft.Content = _gameList;

            if (FrameRight.Content != _gameInfo)
                FrameRight.Content = _gameInfo;
        }

        private void ButtonFriends_OnClick(object sender, RoutedEventArgs e)
        {
            if(friends != null && friends.IsLoaded)
            {
              friends.Activate();
            }
            else
            {
                if (friends == null)
                    friends = new Friends();

                friends.Show();
            }
        }
    }
}