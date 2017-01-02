using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Videospieldatenbank.Utils
{
    public static class PasswordUtils
    {
        /// <summary>
        ///     Erstellt einen MD5-Hash aus dem gegebenen String.
        /// </summary>
        /// <param name="source">Der String dessen Hash ermittelt werden soll.</param>
        /// <returns>Ein Bytearray dass den Hash enthält.</returns>
        [DebuggerStepThrough]
        public static string GetHash(string source)
        {
            using (HashAlgorithm hashAlgorithm = HashAlgorithm.Create("SHA256"))
            {
                StringBuilder hexBuilder = new StringBuilder();
                foreach (byte b in hashAlgorithm.ComputeHash(Encoding.Unicode.GetBytes(source)))
                {
                    hexBuilder.AppendFormat("{0:x2}", b);
                }
                return hexBuilder.ToString();
            }
        }
    }
}