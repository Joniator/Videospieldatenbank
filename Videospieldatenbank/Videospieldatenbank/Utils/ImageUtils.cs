using System.Diagnostics;
using System.Windows.Media;

namespace Videospieldatenbank.Utils
{
    public static class ImageUtils
    {
        public static byte[] ImageToBytes(ImageSource imageSource)
        {
            return (byte[]) new ImageSourceConverter().ConvertTo(imageSource, typeof(byte[]));
        }

        /// <summary>
        ///     Konvertiert ein Bytearray in eine ImageSource.
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <returns></returns>
        public static ImageSource BytesToImageSource(byte[] imageBytes)
        {
            return (ImageSource) new ImageSourceConverter().ConvertFrom(imageBytes);
        }

        /// <summary>
        ///     Konvertiert einen Stringarray in einen String.
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