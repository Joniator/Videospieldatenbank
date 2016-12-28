using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Videospieldatenbank.Utils
{
    [Serializable]
    static class Settings
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
