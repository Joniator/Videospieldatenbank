using System;

namespace Videospieldatenbank.Utils
{
    [Serializable]
    internal static class Settings
    {
        [Serializable]
        public static class Login
        {
            public static bool CheckBoxUsername;
            public static bool CheckBoxPassword;
            public static string Username;
            public static string Password;
        }
    }
}