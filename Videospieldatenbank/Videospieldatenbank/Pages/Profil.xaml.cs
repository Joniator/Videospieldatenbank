using System.Windows.Controls;
using Videospieldatenbank.Pages.Settings;

namespace Videospieldatenbank
{
    /// <summary>
    ///     Interaktionslogik für OptionsDesign.xaml
    /// </summary>
    public partial class Profil : Page
    {
        private readonly DesignSettings _designSettings= new DesignSettings();

        public Profil()
        {
            InitializeComponent();
            FrameLayout.Content = _designSettings;
        }
    }
}