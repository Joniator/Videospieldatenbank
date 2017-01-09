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
            Settings.Default.Upgrade();
            CheckBoxSaveUsername.IsChecked = LoginSettings.CheckBoxUsername;
            if (LoginSettings.CheckBoxUsername) TextBoxUsername.Text = LoginSettings.Username;
            CheckBoxSavePassword.IsChecked = LoginSettings.CheckBoxPassword;
            if (LoginSettings.CheckBoxPassword) TextBoxPassword.Password = LoginSettings.Password;
        }

        /// <summary>
        ///     Prüft, ob die Checkboxes zum speichern checked/nicht checked sind.
        /// </summary>
        private void CheckCheckBoxes()
        {
            LoginSettings.CheckBoxUsername = CheckBoxSaveUsername.IsChecked.Value;
            LoginSettings.CheckBoxPassword = CheckBoxSavePassword.IsChecked.Value;

            if (CheckBoxSaveUsername.IsChecked == true)
                LoginSettings.Username = TextBoxUsername.Text;
            if (CheckBoxSavePassword.IsChecked == true)
                LoginSettings.Password = TextBoxPassword.Password;
        }

        /// <summary>
        ///     Wird zum einloggen benutzt.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
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

        /// <summary>
        ///     Wird zum registrieren benutzt.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        private void Register(string username, string password)
        {
            if (!UserDatabaseConnector.Register(username, password))
                MessageBox.Show("Registrieren fehlgeschlagen, der Username ist bereits vergeben.");
            else
                Login(username, password);
        }

        /// <summary>
        ///     Ruft die Methode Login auf.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonLogin_OnClick(object sender, RoutedEventArgs e)
        {
            Login(TextBoxUsername.Text, TextBoxPassword.Password);
        }

        /// <summary>
        ///     Prüft, ob der Username und das Passwort länger als 3 Buchstaben sind, ob das Passwort gleich ist und ruft die
        ///     Methode Register auf.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        ///     Sorgt dafür, dass man sich durch das drücken der Entertaste einloggen kann.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabItemLogin_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ButtonLogin_OnClick(null, null);
        }

        /// <summary>
        ///     Sorgt dafür, dass man sich durch das drücken der Entertaste registrieren kann.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabItemRegister_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ButtonRegister_OnClick(null, null);
        }
    }
}