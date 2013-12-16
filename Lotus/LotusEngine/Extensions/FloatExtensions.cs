using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusEngine
{
    /// <summary>
    /// Extends the float type with methods related to LotusEngine.
    /// </summary>
    public static class FloatExtensions
    {
        /// <summary>
        /// Converts a float to a string formatted like this: "m:s".
        /// </summary>
        /// <param name="value">The string to format.</param>
        /// <returns>The formatted string.</returns>
        public static string MinutesSecondsString(this float value)
        {
            int min = (int)(value / 60),
                sec = (int)(value % 60);

            string strMin = min < 10 ? "0" + min : min.ToString(),
                   strSec = sec < 10 ? "0" + sec : sec.ToString();

            return strMin + ":" + strSec;
        }
    }
}
