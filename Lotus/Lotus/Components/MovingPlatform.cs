using LotusEngine;

namespace Lotus {
    /// <summary>
    /// Component for movable platforms.
    /// This component only takes care of the movement between two points.
    /// </summary>
    [UniqueComponent]
    class MovingPlatform : Component {
        /// <summary>
        /// End position
        /// </summary>
        public Vector2 end { get; set; }
        /// <summary>
        /// Duration of movement
        /// </summary>
        public float duration { get; set; }
        /// <summary>
        /// Pause between end and start of movement
        /// </summary>
        public float pause { get; set; }
        /// <summary>
        /// Start position
        /// </summary>
        private Vector2 start;
        /// <summary>
        /// Moving towards end position
        /// </summary>
        private bool direction = true;
        /// <summary>
        /// Time since start of movement
        /// </summary>
        private float t = 0;

        public override void Start() {
            // Set the start position to current position
            start = transform.position;
        }

        public override void Update() {
            t += Time.DeltaTime;
            if (direction)
                transform.position = Vector2.Lerp(start,end,t / duration);
            else
                transform.position = Vector2.Lerp(end,start,t / duration);

            if (t / (duration + pause) >= 1) {
                direction = !direction;
                t = 0;
            }
        }
    }
}
