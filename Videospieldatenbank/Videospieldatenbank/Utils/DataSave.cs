using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Videospieldatenbank.Utils
{
    internal static class DataSave
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
    }
}