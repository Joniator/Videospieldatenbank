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
        private readonly OptionsDesign _optionsDesign = new OptionsDesign();
        public static Friends friends = null;

        public MainWindow()
        {
            //new DatabaseInteraction("tazed.tk", 3306, "gamedatabase", "igdb", "1337");
        }

        private object FrameCheck(bool frameLR, bool frameF)
        {
            if (FrameLeft.IsVisible || FrameLeft.IsVisible && !frameLR)
            {
                FrameLeft.Visibility = Visibility.Collapsed;
                FrameRight.Visibility = Visibility.Collapsed;
                return true;
            }
            if (!FrameLeft.IsVisible || !FrameLeft.IsVisible && frameLR)
            {
                FrameLeft.Visibility = Visibility.Visible;
                FrameRight.Visibility = Visibility.Visible;
                return true;
            }

            return false;
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

        private void ButtonContent_OnClick(object sender, RoutedEventArgs e)
        {
            if (FrameLeft.Content != _optionsDesign)
                FrameLeft.Content = _optionsDesign;
        }
    }
}