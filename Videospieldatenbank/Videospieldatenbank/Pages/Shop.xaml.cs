using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Videospieldatenbank.Database;

namespace Videospieldatenbank.Pages
{
    /// <summary>
    /// Interaktionslogik für Shop.xaml
    /// </summary>
    public partial class Shop : Page
    {
        public Shop()
        {
            GameDatabaseConnector gameDatabaseConnector = new GameDatabaseConnector();
            InitializeComponent();
            foreach (var Game in gameDatabaseConnector.GetGameInfosAll())
            {
                ListBoxGameName.Items.Add(new ListBoxItem { Content = Game.Name });
                ListBoxGenre.Items.Add(new ListBoxItem { Content = Game.Genres });
                ListBoxDeveloper.Items.Add(new ListBoxItem { Content = Game.Developer });
                ListBoxPlattform.Items.Add(new ListBoxItem { Content = Game.Plattforms });
            }
        }
    }
}
