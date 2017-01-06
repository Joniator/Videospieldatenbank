using System.Windows;
using Videospieldatenbank.Pages.Settings;

namespace Videospieldatenbank.Windows
{
    /// <summary>
    ///     Interaktionslogik für FriendsProfilWindow.xaml
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