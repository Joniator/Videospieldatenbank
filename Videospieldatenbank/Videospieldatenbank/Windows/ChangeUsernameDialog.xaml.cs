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