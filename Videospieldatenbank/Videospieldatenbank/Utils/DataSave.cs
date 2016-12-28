using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Videospieldatenbank.Utils
{
    static class DataSave
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
