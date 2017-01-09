using System;
using System.Diagnostics;
using System.IO;
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
        private string _igdbUrl;

        /// <summary>
        ///     Füllt die Informationen der Spiele aus.
        /// </summary>
        /// <param name="igdbUrl">Der Spielelink von igdb.com</param>
        public GameInfo(string igdbUrl)
        {
            InitializeComponent();
            _igdbUrl = igdbUrl;
            GameDatabaseConnector gdc = new GameDatabaseConnector();
            Game gameInfo = gdc.GetGameInfo(igdbUrl);
            ListBoxItemName.Content += gameInfo.Name;
            foreach (string genre in gameInfo.Genres)
                ListBoxItemGenre.Content += genre + "; ";
            ListBoxItemDeveloper.Content += gameInfo.Developer;
            Cover.Source = ImageUtils.BytesToImageSource(gameInfo.Cover);
            try
            {
                TimeSpan playTime = LoginWindow.UserDatabaseConnector.GetPlayTime(igdbUrl);
                ListBoxItemGametime.Content +=
                        $"{Math.Floor(playTime.TotalHours).ToString("00")}:{playTime.Minutes.ToString("00")}";
            }
            catch (Exception)
            {
                ListBoxItemGametime.Content += TimeSpan.Zero.ToString();
            }
            StartButton.Click += (sender, args) =>
            {
                string execPath = LoginWindow.UserDatabaseConnector.GetExecPath(igdbUrl);
                ProcessStartInfo psi = new ProcessStartInfo(execPath);
                psi.WorkingDirectory = Path.GetDirectoryName(execPath);
                Process process = Process.Start(psi);
                process.EnableRaisingEvents = true;
                process.Exited += (o, eventArgs) =>

                        // Zeigt nach dem beenden des Spiels die Laufzeit an.
                        {
                            TimeSpan playtime = process.ExitTime - process.StartTime;
                            MessageBox.Show(playtime.TotalSeconds.ToString(), "Programm wurde geschlossen.");
                            LoginWindow.UserDatabaseConnector.AddPlayTime(igdbUrl, playtime);
                        };
            };
        }
    }
}