using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace LotusEngine
{
    /// <summary>
    /// Functionality for keeping track of time.
    /// </summary>
    public sealed class Time
    {
        private static Stopwatch stopwatch;
        /// <summary>
        /// Contains the exact time since last frame.
        /// </summary>
        private static Stopwatch Stopwatch
        {
            get
            {
                if (stopwatch == null)
                {
                    stopwatch = new Stopwatch();
                    stopwatch.Start();

                    TimeSpeed = 1;
                }

                return stopwatch;
            }
        }

        /// <summary>
        /// Gets or sets the speed at which time runs.
        /// </summary>
        public static float TimeSpeed { get; set; }
        /// <summary>
        /// The time since last frame was executed, adjusted by Time.TimeSpeed.
        /// </summary>
        public static float DeltaTime { get; private set; }
        /// <summary>
        /// The real time since last frame was executed.
        /// </summary>
        public static float RealDeltaTime { get; private set; }
        /// <summary>
        /// The exact time since last frame was executed.
        /// </summary>
        public static double AbsoluteDeltaTime { get { return Stopwatch.Elapsed.TotalSeconds; } }


        /// <summary>
        /// The exact time in millissecconds since last frame was executed.
        /// </summary>
        public static float DeltaTimeMilliseconds { get; private set; }

        /// <summary>
        /// The current FPS.
        /// </summary>
        public static int CurrentFPS { get; private set; }

        /// <summary>
        /// Tell the Time class that a new frame has begun.
        /// </summary>
        internal static void NewFrameBegins()
        {
            RealDeltaTime = (float)Stopwatch.Elapsed.TotalSeconds;
            DeltaTime = RealDeltaTime * TimeSpeed;
            DeltaTimeMilliseconds = DeltaTime * 1000;
            CurrentFPS = Mathf.RoundToInt(1F / DeltaTime, 0, MidpointRounding.AwayFromZero);
            Stopwatch.Restart();
        }
    }
}
