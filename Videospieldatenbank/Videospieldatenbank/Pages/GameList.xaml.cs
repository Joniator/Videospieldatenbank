using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

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
        }

        private void MenuItemAdd_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.ShowDialog();

            if (listGames == null)
                listGames = new List<Game>();

            //in arbeit
            {
                Game game = null;

                for (int i = 1; i <= openFileDialog.SafeFileNames.Length;)
                {
                    game = new Game(fileName, safeFileName);
                }
                listGames.Add(game);

                ListBoxItem listBoxItem = new ListBoxItem();

                listBoxItem.Content = game.Name;

                Listbox_TabItem_Games.Items.Add(listBoxItem);
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