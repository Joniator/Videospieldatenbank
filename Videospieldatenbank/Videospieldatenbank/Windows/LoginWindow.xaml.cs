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

namespace Videospieldatenbank.Windows
{
    /// <summary>
    /// Interaktionslogik für LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private UserDatabaseConnector _userDatabaseConnector = new UserDatabaseConnector();
        public LoginWindow()
        {
            InitializeComponent();

        }

        void Login(string username, string password)
        {
            if (!_userDatabaseConnector.Login(username, password))
            {
                MessageBox.Show("USERNAME / PASSWORD is wrong or user doesn't exist!");
            }
            else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();

                this.Close();
            }
        }

        void Register(string username, string password)
        {
            if (!_userDatabaseConnector.Register(username, password))
            {
                MessageBox.Show("USERNAME is in use! Please take an other USERNAME");
            }
        }

        private void ButtonLogin_OnClick(object sender, RoutedEventArgs e)
        {
            Login(TextBoxUsername.Text,TextBoxPassword.Password);
        }

        private void ButtonRegister_OnClick(object sender, RoutedEventArgs e)
        {
            if(TextBoxRegistPassword.Password == TextBoxRegistRePassword.Password)
                Register(TextBoxRegistUsername.Text,TextBoxRegistPassword.Password);
            else
            {
                MessageBox.Show("Password is not equal!");
            }
        }
    }
}
