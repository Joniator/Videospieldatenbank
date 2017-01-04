using System;
using System.Diagnostics;
using System.Windows;
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
                TimeSpan playTime = LoginWindow.UserDatabaseConnector.GetPlayTime(igdbUrl);
                ListBoxItemGametime.Content += $"{Math.Floor(playTime.TotalHours).ToString("00")}:{playTime.Minutes.ToString("00")}";
            }
            catch (Exception)
            {
                ListBoxItemGametime.Content += TimeSpan.Zero.ToString();
            }
            StartButton.Click += (sender, args) =>
            {
                Process process = Process.Start(LoginWindow.UserDatabaseConnector.GetExecPath(igdbUrl));
                process.EnableRaisingEvents = true;
                process.Exited += (o, eventArgs) =>
                {
                    TimeSpan playtime = DateTime.Now - process.StartTime;
                    LoginWindow.UserDatabaseConnector.AddPlayTime(igdbUrl, playtime);
                    MessageBox.Show(playtime.TotalSeconds.ToString());
                };
            };
        }
    }
}