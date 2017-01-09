using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Videospieldatenbank.Database;
using Videospieldatenbank.Windows;

namespace Videospieldatenbank
{
    /// <summary>
    ///     Interaktionslogik für GameList.xaml
    /// </summary>
    public partial class GameList : Page
    {
        private static GameList @this;
        private string _selectedGame;

        public GameList()
        {
            InitializeComponent();
            @this = this;
            FillGameList(LoginWindow.UserDatabaseConnector.GetGames());
        }

        /// <summary>
        /// Füllt die Listbox mit den Games des Users von der Datenbank.
        /// </summary>
        /// <param name="games">Die Liste mit den Games von der Datenbank.</param>
        private void FillGameList(List<string> games)
        {
            try
            {
                foreach (string game in games)
                    using (GameDatabaseConnector gameDatabaseConnector = new GameDatabaseConnector())
                    {
                        Database.Game gameInfo = gameDatabaseConnector.GetGameInfo(game);

                        ListBoxItem listBoxItem = new ListBoxItem
                        {
                            Content = gameInfo.Name,
                            Foreground = Brushes.Black,
                            Opacity = 100
                        };
                        listBoxItem.PreviewMouseDown += (sender, args) =>
                        {
                            MainWindow.SetGameInfo(game);
                            _selectedGame = game;
                        };

                        Listbox_TabItem_Games.Items.Add(listBoxItem);
                    }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Aktualisiert die GameList.
        /// </summary>
        public static void RefreshGameList()
        {
            @this.Listbox_TabItem_Games.Items.Clear();
            @this.FillGameList(LoginWindow.UserDatabaseConnector.GetGames());
        }

        /// <summary>
        /// Tritt beim löschen eines Spiels auf.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemDelete_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(
                                $"Dies kann nicht rückgängig gemacht werden und löscht sämtliche Spielstatistiken für {_selectedGame}.",
                                "Sicher?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                LoginWindow.UserDatabaseConnector.RemoveGame(_selectedGame);
                MainWindow.RefreshLibrary();
            }
            else
            {
                MessageBox.Show("Abgebrochen.");
            }
        }
    }
}