using System.Windows.Controls;
using Videospieldatenbank.Pages.Settings;

namespace Videospieldatenbank
{
    /// <summary>
    ///     Interaktionslogik für OptionsDesign.xaml
    /// </summary>
    public partial class Profil : Page
    {
        private readonly ProfilSettings _profilSettings = new ProfilSettings();

        /// <summary>
        /// Läd die Profilseite.
        /// </summary>
        public Profil()
        {
            InitializeComponent();
            FrameProfileSettings.Content = _profilSettings;
        }

        /// <summary>
        /// Aktualisiert die Profilseite.
        /// </summary>
        public void ReloadProfil()
        {
            _profilSettings.UserInfos();
        }
    }
}