using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using Videospieldatenbank.Database;
using Videospieldatenbank.Windows;

namespace Videospieldatenbank
{
    /// <summary>
    ///     Interaktionslogik für GameList.xaml
    /// </summary>
    public partial class GameList : Page
    {
        private static List<Game> listGames;
        private static GameList @this;
        private string _selectedGame;

        public GameList()
        {
            InitializeComponent();
            @this = this;
            FillGameList(LoginWindow.UserDatabaseConnector.GetGames());
        }

        public void FillGameList(List<string> games)
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

        public static void RefreshGameList()
        {
            @this.Listbox_TabItem_Games.Items.Clear();
            @this.FillGameList(LoginWindow.UserDatabaseConnector.GetGames());
        }

        private void MenuItemAdd_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.ShowDialog();
        }

        private void MenuItemEdit_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

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

        public class Game
        {
            public string Launcher;
            public string Name;
            public string Path;

            public Game(string filename, string safeFileName)
            {
                Path = filename;
                Name = safeFileName.Remove(safeFileName.Length - 4, 4);
            }
        }
    }
}