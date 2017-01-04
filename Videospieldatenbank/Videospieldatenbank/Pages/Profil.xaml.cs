using System.Windows.Controls;
using Videospieldatenbank.Pages.Settings;
using Videospieldatenbank.Windows;

namespace Videospieldatenbank
{
    /// <summary>
    ///     Interaktionslogik für OptionsDesign.xaml
    /// </summary>
    public partial class Profil : Page
    {
        private readonly DesignSettings _designSettings = new DesignSettings();
        private readonly ProfilSettings _profilSettings = new ProfilSettings();

        public Profil()
        {
            InitializeComponent();
            FrameLayout.Content = _designSettings;
            FrameAccount.Content = _profilSettings;
        }

        public void ReloadProfil()
        {
            _profilSettings.UserInfos();
        }
    }
}