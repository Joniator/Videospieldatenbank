using System.Windows;
using Videospieldatenbank.Database;

namespace Videospieldatenbank
{
    /// <summary>
    ///     Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Friends friends;
        private readonly GameInfo _gameInfo = new GameInfo();
        private readonly GameList _gameList = new GameList();
        private readonly OptionsDesign _optionsDesign = new OptionsDesign();

        private object FrameCheck(bool frameLR, bool frameF)
        {
            if (FrameLeft.IsVisible || (FrameLeft.IsVisible && !frameLR))
            {
                FrameLeft.Visibility = Visibility.Collapsed;
                FrameRight.Visibility = Visibility.Collapsed;
                return true;
            }
            if (!FrameLeft.IsVisible || (!FrameLeft.IsVisible && frameLR))
            {
                FrameLeft.Visibility = Visibility.Visible;
                FrameRight.Visibility = Visibility.Visible;
                return true;
            }

            return false;
        }

        public MainWindow()
        {

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
            if ((friends != null) && friends.IsLoaded)
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