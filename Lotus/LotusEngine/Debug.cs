using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusEngine
{
    /// <summary>
    /// Contains functionality for Debugging.
    /// </summary>
    public static class Debug
    {
        /// <summary>
        /// Writes a message to the standard output stream.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="args">Substitution parameters for the message - acts like String.Format().</param>
        public static void Log(object message, params object[] args)
        {
            Console.WriteLine(message.ToString(), args);
        }

        /// <summary>
        /// Writes a warning to the standard output stream.
        /// </summary>
        /// <param name="message">The warning to write.</param>
        /// <param name="args">Substitution parameters for the warning - acts like String.Format().</param>
        public static void LogWarning(object message, params object[] args)
        {
            Console.WriteLine("--WARNING-- " + message.ToString(), args);
        }

        /// <summary>
        /// Writes an error to the standard output stream.
        /// </summary>
        /// <param name="message">The error to write.</param>
        /// <param name="args">Substitution parameters for the error - acts like String.Format().</param>
        public static void LogError(object message, params object[] args)
        {
            Console.WriteLine("--ERROR-- " + message.ToString(), args);
        }
    }
}