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
        public Vector2 position { get { return _position; } set { _position = value; } }
        [Serialize]
        private Vector2 _position;

        /// <summary>
        /// The scale of the Transform.
        /// </summary>
        public Vector2 scale { get { return _scale; } set { _scale = value; } }
        [Serialize]
        private Vector2 _scale;

        /// <summary>
        /// The rotation of the Transform, defined in 360 degrees.
        /// </summary>
        public float rotation { get { return _rotation; } set { _rotation = value; } }
        [Serialize]
        private float _rotation;
    }
}