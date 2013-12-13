using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusEngine
{
    /// <summary>
    /// Defines spatial position, scale and rotation in a 2D space.
    /// </summary>
    [UniqueComponent]
    public sealed class Transform : Component
    {
        /// <summary>
        /// Create a new Transform instance.
        /// </summary>
        public Transform()
        {
            scale = new Vector2(1, 1);
        }
        
        /// <summary>
        /// The position of the Transform..
        /// </summary>
        public Vector2 position { get; set; }
        /// <summary>
        /// The scale of the Transform.
        /// </summary>
        public Vector2 scale { get; set; }
        /// <summary>
        /// The rotation of the Transform, defined in 360 degrees.
        /// </summary>
        public float rotation { get; set; }
    }
}