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
using System.Windows.Shapes;
using Videospieldatenbank.Pages.Settings;

namespace Videospieldatenbank.Windows
{
    /// <summary>
    /// Interaktionslogik für FriendsProfilWindow.xaml
    /// </summary>
    public partial class FriendsProfilWindow : Window
    {
        private readonly ProfilSettings _profilSettings = new ProfilSettings();

        public FriendsProfilWindow(int FriendId)
        {
            InitializeComponent();
            _profilSettings.UserInfos(FriendId);
            FrameFull.Content = _profilSettings;
        }
    }
}
