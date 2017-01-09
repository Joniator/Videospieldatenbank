using System.ComponentModel;
using System.Windows;
using Videospieldatenbank.Pages;
using Videospieldatenbank.Windows;

namespace Videospieldatenbank
{
    /// <summary>
    ///     Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow @this;
        public static Friends friends;
        public static GameInfo GameInfo;
        private GameList _gameList;
        private Profil _profil;
        private Shop _shop;

        public MainWindow()
        {
            InitializeComponent();
            @this = this;
            // Zeigt das Profil des Users an wenn er die App startet.
            ButtonContent_OnClick(null, null);
        }

        /// <summary>
        ///     Deaktiviert ungenutzte frames und aktiviert genutzte frames.
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
                FrameFull.Visibility = Visibility.Collapsed;
            if (!FrameFull.IsVisible && frameF)
                FrameFull.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Zeigt die Bibliothek.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonLibrary_OnClick(object sender, RoutedEventArgs e)
        {
            if ((_gameList == null) || (GameInfo == null))
                _gameList = new GameList();
            RefreshLibrary();

            FrameCheck(true);

            if (FrameLeft.Content != _gameList)
                FrameLeft.Content = _gameList;
        }

        public static void RefreshLibrary()
        {
            GameList.RefreshGameList();
        }

        /// <summary>
        ///     Zeigt das Spiel rechts in der GameInfo-Spalte an.
        /// </summary>
        /// <param name="igdbUrl"></param>
        public static void SetGameInfo(string igdbUrl)
        {
            GameInfo = new GameInfo(igdbUrl);
            @this.FrameRight.Content = GameInfo;
        }

        /// <summary>
        ///     Zeigt die Freundeliste.
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
        ///     Zeigt Profil/Settings.
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
                FrameFull.Content = null;
            FrameFull.Content = _profil;
        }

        /// <summary>
        /// Zeigt den Shop.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonShop_OnClick(object sender, RoutedEventArgs e)
        {
            if (_shop == null)
                _shop = new Shop();

            FrameCheck(false);

            if (FrameFull.Content != _shop)
                FrameFull.Content = null;
            FrameFull.Content = _shop;
        }

        /// <summary>
        /// Zeigt eine MessageBox, die fragt ob man wirklich das Programm verlassen will.
        /// Wenn ja, dann wird dieses beendet. Bei nein Schließt sich nur die Box.
        /// </summary>
        /// <returns></returns>
        private bool ExitMessageBox()
        {
            string text = "Willst du wirklich beenden?";
            string caption = "Beenden";
            MessageBoxResult result = MessageBox.Show(text, caption, MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                LoginWindow.UserDatabaseConnector.Logout();
                Application.Current.Shutdown();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Ruft beim beenden des Programms die Methode ExitMessageBox() auf.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (!ExitMessageBox())
                e.Cancel = true;
        }
    }
}