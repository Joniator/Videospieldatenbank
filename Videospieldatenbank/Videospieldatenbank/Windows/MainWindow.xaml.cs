using System.ComponentModel;
using System.Data.SqlTypes;
using System.Windows;
using Videospieldatenbank.Database;
using Videospieldatenbank.Pages;
using Videospieldatenbank.Pages.Settings;
using Videospieldatenbank.Windows;

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
            @this = this;
        }

        private static MainWindow @this;
        public static Friends friends;
        public static GameInfo GameInfo;
        private  GameList _gameList;
        private  Profil _profil;
        private  Shop _shop;

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
            if (_gameList == null || GameInfo == null)
            {
                _gameList = new GameList();
            }

            FrameCheck(true);           

            if (FrameLeft.Content != _gameList)
                FrameLeft.Content = _gameList;
        }

        public static void RefreshLibrary()
        {
            GameList.RefreshGameList();
            @this.FrameRight.Content = GameInfo;
        }

        /// <summary>
        ///     Zeigt das Spiel rechts in der GameInfo-Spalte an.
        /// </summary>
        /// <param name="igdbUrl"></param>
        public static void SetGameInfo(string igdbUrl)
        {
            GameInfo = new GameInfo(igdbUrl);
            RefreshLibrary();
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
            if (_profil == null)
                _profil = new Profil();

            _profil.ReloadProfil();
            FrameCheck(false);
            if (FrameFull.Content != _profil)
                FrameFull.Content = null; FrameFull.Content = _profil;
        }

        private bool ExitMessageBox()
        {
            string text = "Do you really want to exit the program?";
            string caption = "Exit";
            MessageBoxResult result = MessageBox.Show(text, caption, MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                LoginWindow.UserDatabaseConnector.Logout();
                Application.Current.Shutdown();
                return true;
            }
            else
            {
                return false;
            }
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (!ExitMessageBox())
                e.Cancel = true;
        }

        private void ButtonShop_OnClick(object sender, RoutedEventArgs e)
        {
            if (_shop == null)
                _shop = new Shop();

            FrameCheck(false);

            if (FrameFull.Content != _shop)
                FrameFull.Content = null; FrameFull.Content = _shop;
        }
    }
}