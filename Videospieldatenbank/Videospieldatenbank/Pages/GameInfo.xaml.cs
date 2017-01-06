using System;
using System.Windows.Controls;
using Videospieldatenbank.Database;
using Videospieldatenbank.Utils;
using Videospieldatenbank.Windows;

namespace Videospieldatenbank
{
    /// <summary>
    ///     Interaktionslogik für GameInfo.xaml
    /// </summary>
    public partial class GameInfo : Page
    {
        public GameInfo(string igdbUrl)
        {
            InitializeComponent();
            GameDatabaseConnector gdc = new GameDatabaseConnector();
            Game gameInfo = gdc.GetGameInfo(igdbUrl);
            ListBoxItemName.Content += gameInfo.Name;
            foreach (string genre in gameInfo.Genres)
                ListBoxItemGenre.Content += genre + "; ";
            ListBoxItemPuplisher.Content += gameInfo.Developer;
            Cover.Source = ImageUtils.BytesToImageSource(gameInfo.Cover);
            try
            {
                ListBoxItemGametime.Content += LoginWindow.UserDatabaseConnector.GetPlayTime(igdbUrl).ToString();
            }
            catch (Exception)
            {
                ListBoxItemGametime.Content += TimeSpan.Zero.ToString();
            }
        }
    }
}