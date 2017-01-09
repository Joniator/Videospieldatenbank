using System.Windows;

namespace Videospieldatenbank.Windows
{
    /// <summary>
    ///     Interaktionslogik für ChangeUsernameDialog.xaml
    /// </summary>
    public partial class ChangeUsernameDialog : Window
    {
        public ChangeUsernameDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Setzt den neuen Usernamen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoginWindow.UserDatabaseConnector.SetUsername(NewUsername.Text);
                DialogResult = true;
            }
            catch
            {
                DialogResult = false;
            }
        }
    }
}