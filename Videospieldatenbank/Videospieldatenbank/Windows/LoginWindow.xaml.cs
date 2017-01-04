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
using Videospieldatenbank.Database;
using Videospieldatenbank.Utils;

namespace Videospieldatenbank.Windows
{
    /// <summary>
    /// Interaktionslogik für LoginWindow.xaml
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

        void CheckCheckBoxes()
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

        void Login(string username, string password)
        {
            if (!UserDatabaseConnector.Login(username, password))
            {
                MessageBox.Show("USERNAME / PASSWORD is wrong or user doesn't exist!");
            }
            else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                CheckCheckBoxes();

                this.Close();
            }
        }

        void Register(string username, string password)
        {
            if (!UserDatabaseConnector.Register(username, password))
            {
                MessageBox.Show("USERNAME is in use! Please take an other USERNAME");
            }
            else
            {
                Login(username, password);
            }
        }

        private void ButtonLogin_OnClick(object sender, RoutedEventArgs e)
        {
            Login(TextBoxUsername.Text,TextBoxPassword.Password);
        }

        private void ButtonRegister_OnClick(object sender, RoutedEventArgs e)
        {
            if (TextBoxRegistUsername.Text.Length >= 3)
            {
                if (TextBoxRegistPassword.Password.Length >= 3)
                {
                    if (TextBoxRegistPassword.Password == TextBoxRegistRePassword.Password)
                        Register(TextBoxRegistUsername.Text, TextBoxRegistPassword.Password);
                    else
                    {
                        MessageBox.Show("Password is not equal!");
                    }
                }
                else
                {
                    MessageBox.Show("Password is too short! It must have more than three signs.");
                }
            }
            else
            {
                MessageBox.Show("Username is too short! It must have more than three signs.");
            }
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
