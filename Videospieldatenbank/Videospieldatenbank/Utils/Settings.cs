namespace Videospieldatenbank.Utils
{
    /// <summary>
    ///     Greift auf die Settingsdatei zu.
    /// </summary>
    internal static class LoginSettings
    {
        public static bool CheckBoxUsername
        {
            get
            {
                Settings.Default.Reload();
                return Settings.Default.CheckBoxUsername;
            }
            set
            {
                Settings.Default.CheckBoxUsername = value;
                Settings.Default.Save();
            }
        }

        public static bool CheckBoxPassword
        {
            get
            {
                Settings.Default.Reload();
                return Settings.Default.CheckBoxPassword;
            }
            set
            {
                Settings.Default.CheckBoxPassword = value;
                Settings.Default.Save();
            }
        }

        public static string Username
        {
            get
            {
                Settings.Default.Reload();
                return Settings.Default.Username;
            }
            set
            {
                Settings.Default.Username = value;
                Settings.Default.Save();
            }
        }

        public static string Password
        {
            get
            {
                Settings.Default.Reload();
                return Settings.Default.Password;
            }
            set
            {
                Settings.Default.Password = value;
                Settings.Default.Save();
            }
        }
    }
}