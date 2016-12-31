using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Videospieldatenbank.Utils
{
    [Serializable]
    static class Settings
    {
        public static void Serializer(string filename, object sObject)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(filename + ".sav", FileMode.OpenOrCreate);

            try
            {
                binaryFormatter.Serialize(fileStream, sObject);
            }
            catch (Exception)
            {
                throw;
            }

            fileStream.Close();
        }

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
