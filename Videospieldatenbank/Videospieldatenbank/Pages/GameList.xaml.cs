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

        public GameList()
        {
            InitializeComponent();
            FillGameList(LoginWindow.UserDatabaseConnector.GetGames());
        }

        public void FillGameList(List<string> games)
        {
            try
            {
                foreach (string game in games)
                {
                    using (GameDatabaseConnector gameDatabaseConnector = new GameDatabaseConnector())
                    {
                        Database.Game gameInfo = gameDatabaseConnector.GetGameInfo(game);

                        ListBoxItem listBoxItem = new ListBoxItem()
                        {
                            Content = gameInfo.Name,
                            Foreground = Brushes.Azure,
                            Opacity = 100
                        };
                        listBoxItem.PreviewMouseDown += (sender, args) =>
                        {
                            MainWindow.SetGameInfo(game);
                        };

                        Listbox_TabItem_Games.Items.Add(listBoxItem);
                    }
                }
            }
            catch (Exception)
            {

            }

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
            throw new NotImplementedException();
        }

        public class Game
        {
            public string Path;
            public string Name;
            public string Launcher;

            public Game(string filename, string safeFileName)
            {
                Path = filename;
                Name = safeFileName.Remove(safeFileName.Length - 4, 4);
            }
        }
    }
}