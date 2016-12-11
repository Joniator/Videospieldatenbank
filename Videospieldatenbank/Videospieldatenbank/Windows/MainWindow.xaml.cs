using System.Windows;
using Videospieldatenbank.Pages.Settings;

namespace Videospieldatenbank
{
    /// <summary>
    ///     Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static Friends friends;
        private readonly GameInfo _gameInfo = new GameInfo();
        private readonly GameList _gameList = new GameList();
        private readonly Profil _profil = new Profil();

        /// <summary>
        /// Disables unused frames and enables used frames.
        /// </summary>
        /// <param name="frameLR">true = enable frameLR; false = disable frameLR</param>
        private void FrameCheck(bool frameLR)
        {
            bool frameF = !frameLR;

            //Checks for frameLR
            if (FrameLeft.IsVisible && !frameLR)
            {
                FrameLeft.Visibility = Visibility.Collapsed;
                FrameRight.Visibility = Visibility.Collapsed;
                GridSplitter.Visibility = Visibility.Collapsed;
            }
            if (!FrameLeft.IsVisible && frameLR)
            {
                FrameLeft.Visibility = Visibility.Visible;
                FrameRight.Visibility = Visibility.Visible;
                GridSplitter.Visibility = Visibility.Visible;
            }

            //Checks for frameF
            if (FrameFull.IsVisible && !frameF)
            {
                FrameFull.Visibility = Visibility.Collapsed;
            }
            if (!FrameFull.IsVisible && frameF)
            {
                FrameFull.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Open Library.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonLibrary_OnClick(object sender, RoutedEventArgs e)
        {
            FrameCheck(true);           

            if (FrameLeft.Content != _gameList)
                FrameLeft.Content = _gameList;

            if (FrameRight.Content != _gameInfo)
                FrameRight.Content = _gameInfo;
        }

        /// <summary>
        /// Open FrindsList.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Open Profil/Settings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonContent_OnClick(object sender, RoutedEventArgs e)
        {
            FrameCheck(false);
            if (FrameFull.Content != _profil)
                FrameFull.Content = _profil;
        }
    }
}