using System.Diagnostics;
using System.Text;

namespace Videospieldatenbank.Utils
{
    public static class ImageUtils
    {
        /// <summary>
        /// Konvertiert einen String in ein Bytearray.
        /// </summary>
        /// <param name="source">Der zu konvertierende String.</param>
        /// <returns>Bytearray</returns>
        [DebuggerStepThrough]
        public static byte[] ToByteArray(this string source)
        {
            byte[] byteArray = new byte[source.Length];
            for (int i = 0; i < source.Length; i++)
                byteArray[i] = (byte) source[i];
            return byteArray;
        }

        /// <summary>
        /// Konvertiert einen Bytearray in einen String.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string ByteArrayToString(this byte[] array)
        {
            StringBuilder stringBuilder = new StringBuilder(65536);
            foreach (byte b in array)
                stringBuilder.Append((char) b);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Konvertiert einen Stringarray in einen String.
        /// </summary>
        /// <param name="array">Das zu konvertierende Array.</param>
        /// <param name="delimiter">Das Zeichen mit dem die Einträge getrennt werden.</param>
        /// <returns>String</returns>
        [DebuggerStepThrough]
        public static string StringArrayToString(this string[] array, string delimiter)
        {
            string result = "";
            foreach (string s in array)
                result += s + delimiter;
            return result;
        }
    }
}