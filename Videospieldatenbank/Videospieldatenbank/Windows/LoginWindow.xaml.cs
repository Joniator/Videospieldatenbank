using System.Windows;
using System.Windows.Input;
using Videospieldatenbank.Database;
using Videospieldatenbank.Utils;

namespace Videospieldatenbank.Windows
{
    /// <summary>
    ///     Interaktionslogik für LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public static UserDatabaseConnector UserDatabaseConnector = new UserDatabaseConnector();

        public LoginWindow()
        {
            InitializeComponent();
            CheckBoxSaveUsername.IsChecked = LoginSettings.CheckBoxUsername;
            TextBoxUsername.Text = LoginSettings.Username;
            CheckBoxSavePassword.IsChecked = LoginSettings.CheckBoxPassword;
            TextBoxPassword.Password = LoginSettings.Password;
        }

        private void CheckCheckBoxes()
        {
            if (CheckBoxSaveUsername.IsChecked == true)
            {
                LoginSettings.CheckBoxUsername = true;
                LoginSettings.Username = TextBoxUsername.Text;
            }
            if (CheckBoxSavePassword.IsChecked == true)
            {
                LoginSettings.CheckBoxPassword = true;
                LoginSettings.Password = TextBoxPassword.Password;
            }
        }

        private void Login(string username, string password)
        {
            if (!UserDatabaseConnector.Login(username, password))
            {
                MessageBox.Show("Login fehlgeschlagen, Username oder Passwort falsch.");
            }
            else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                CheckCheckBoxes();

                Close();
            }
        }

        private void Register(string username, string password)
        {
            if (!UserDatabaseConnector.Register(username, password))
                MessageBox.Show("Registrieren fehlgeschlagen, der Username ist bereits vergeben.");
            else
                Login(username, password);
        }

        private void ButtonLogin_OnClick(object sender, RoutedEventArgs e)
        {
            Login(TextBoxUsername.Text, TextBoxPassword.Password);
        }

        private void ButtonRegister_OnClick(object sender, RoutedEventArgs e)
        {
            if (TextBoxRegistUsername.Text.Length >= 3)
                if (TextBoxRegistPassword.Password.Length >= 3)
                    if (TextBoxRegistPassword.Password == TextBoxRegistRePassword.Password)
                        Register(TextBoxRegistUsername.Text, TextBoxRegistPassword.Password);
                    else
                        MessageBox.Show("Passwörter stimmen nicht überein!");
                else
                    MessageBox.Show("Das Passwort ist zu kurz. (Min. 3 Zeichen)");
            else
                MessageBox.Show("Der Username ist zu kurz. (Min. 3 Zeichen)");
        }

        private void TabItemLogin_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ButtonLogin_OnClick(null, null);
        }

        private void TabItemRegister_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ButtonRegister_OnClick(null, null);
        }
    }
}