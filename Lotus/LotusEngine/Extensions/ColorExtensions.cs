using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace LotusEngine
{
    /// <summary>
    /// Extends the Color type with methods related to LotusEngine.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Converts a RGBA Color to an array of 0-1 float values mapping to their 0-255 byte values.
        /// </summary>
        /// <param name="value">The string to format.</param>
        /// <returns>The formatted string.</returns>
        public static float[] GetRGBAFloats(this Color color)
        {
            return new float[] {
                color.R / 255f,
                color.G / 255f,
                color.B / 255f,
                color.A / 255f
            };
        }
    }
}
