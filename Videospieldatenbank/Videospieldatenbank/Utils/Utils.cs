using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Videospieldatenbank.Utils
{
    public static class ImageUtils
    {
        public static byte[] StringToByteArray(string source)
        {
            byte[] byteArray = new byte[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                byteArray[i] = (byte)source[i];
            }
            return byteArray;
        }

        public static string ByteArrayToString(byte[] source)
        {
            StringBuilder stringBuilder = new StringBuilder(65536);
            foreach (byte b in source)
            {
                stringBuilder.Append((char)b);
            }
            return stringBuilder.ToString();
        }
    }
}
