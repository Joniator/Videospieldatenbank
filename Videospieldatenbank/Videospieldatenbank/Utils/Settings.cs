using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Videospieldatenbank.Utils
{
    static class LoginSettings
    {
        public static bool CheckBoxUsername
        {
            get
            {
                Videospieldatenbank.Settings.Default.Reload();
                return Videospieldatenbank.Settings.Default.CheckBoxUsername;
            }
            set
            {
                Videospieldatenbank.Settings.Default.CheckBoxUsername = value;
                Videospieldatenbank.Settings.Default.Save();
            }
        }

        public static bool CheckBoxPassword
        {
            get
            {
                Videospieldatenbank.Settings.Default.Reload();
                return Videospieldatenbank.Settings.Default.CheckBoxPassword;
            }
            set
            {
                Videospieldatenbank.Settings.Default.CheckBoxPassword = value;
                Videospieldatenbank.Settings.Default.Save();
            }
        }
        public static string Username
        {
            get
            {
                Videospieldatenbank.Settings.Default.Reload();
                return Videospieldatenbank.Settings.Default.Username;
            }
            set
            {
                Videospieldatenbank.Settings.Default.Username = value;
                Videospieldatenbank.Settings.Default.Save();
            }
        }

        public static string Password
        {
            get
            {
                Videospieldatenbank.Settings.Default.Reload();
                return Videospieldatenbank.Settings.Default.Password;
            }
            set
            {
                Videospieldatenbank.Settings.Default.Password = value;
                Videospieldatenbank.Settings.Default.Save();
            }
        }
    }

}