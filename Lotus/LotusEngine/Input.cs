using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LotusEngine
{
    /// <summary>
    /// Handles user input.
    /// </summary>
    public static class Input
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern short GetKeyState(int keyCode);

        private static List<Keys> keyEnumerator;
        private static Dictionary<Keys, bool> inputLastFrame;
        private static Dictionary<Keys, bool> inputThisFrame;

        /// <summary>
        /// The position of the mouse on the screen.
        /// </summary>
        public static Vector2 MousePosition
        {
            get
            {
                return new Vector2(Cursor.Position.X - Settings.Screen.ScreenPositionX, Cursor.Position.Y - Settings.Screen.ScreenPositionY);
            }
        }

        /// <summary>
        /// Initializes the engine's Input functionality.
        /// </summary>
        private static void InitializeInput()
        {
            keyEnumerator = new List<Keys>();
            inputLastFrame = new Dictionary<Keys, bool>();
            inputThisFrame = new Dictionary<Keys, bool>();

            foreach (Keys value in typeof(Keys).GetEnumValues())
            {
                if (!keyEnumerator.Contains(value))
                {
                    keyEnumerator.Add(value);
                    inputLastFrame.Add(value, false);
                    inputThisFrame.Add(value, false);
                }
            }
        }

        /// <summary>
        /// Tell the Input class that a new frame has begun.
        /// </summary>
        internal static void NewFrameBegins()
        {
            if (keyEnumerator == null || inputLastFrame == null || inputThisFrame == null)
                InitializeInput();

            foreach (Keys key in keyEnumerator)
            {
                inputLastFrame[key] = inputThisFrame[key];
                inputThisFrame[key] = (GetKeyState((int)key) & 0x8000) == 0x8000;
            }
        }

        /// <summary>
        /// Checks whether a key is being pressed down this frame.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the key is being pressed down this frame, otherwise false.</returns>
        public static bool GetKey(Keys key)
        {
            return inputThisFrame[key];
        }

        /// <summary>
        /// Checks whether a key has been pressed down this frame.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the key has been pressed down this frame, otherwise false.</returns>
        public static bool GetKeyDown(Keys key)
        {
            return inputThisFrame[key] && !inputLastFrame[key];
        }

        /// <summary>
        /// Checks whether a key has been released this frame.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the key has been released this frame, otherwise false.</returns>
        public static bool GetKeyUp(Keys key)
        {
            return !inputThisFrame[key] && inputLastFrame[key];
        }
    }
}