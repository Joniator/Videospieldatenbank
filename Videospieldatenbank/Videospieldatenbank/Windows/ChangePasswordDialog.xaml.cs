using System.Windows;

namespace Videospieldatenbank.Windows
{
    /// <summary>
    ///     Interaktionslogik für ChangePasswordDialog.xaml
    /// </summary>
    public partial class ChangePasswordDialog : Window
    {
        public ChangePasswordDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Setzt das neue Passwort.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoginWindow.UserDatabaseConnector.SetPassword(NewPassword.Password);
                DialogResult = true;
            }
            catch
            {
                DialogResult = false;
            }
        }
    }
}