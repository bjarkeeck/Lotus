using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusEngine
{
    /// <summary>
    /// Extends the string type with methods related to LotusEngine.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Gets the raw bytes of a given string.
        /// </summary>
        /// <param name="str">The string to convert to bytes.</param>
        /// <returns>The bytes of the string as an array.</returns>
        public static byte[] GetBytes(this string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
