using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace LotusEngine
{
    /// <summary>
    /// A circle-shaped collider.
    /// </summary>
    public class CircleCollider : Collider
    {
        /// <summary>
        /// Creates a new CircleCollider instance.
        /// </summary>
        public CircleCollider()
        {
            if (Collider.allColliders != null)
                Collider.allColliders.Add(this);
        }

        /// <summary>
        /// The radius of the CircleCollider.
        /// </summary>
        public float radius;
        /// <summary>
        /// How much to offset the collider from the origin. Affected by rotation and scale.
        /// </summary>
        public Vector2 offset;

        /// <summary>
        /// Get the center of the circle collider.
        /// </summary>
        public Vector2 center
        {
            get
            {
                float sin = (float)Math.Sin((double)Vector2.FloatDeg2Rad(-transform.rotation - 90));
                float cos = (float)Math.Cos((double)Vector2.FloatDeg2Rad(-transform.rotation - 90));

                float dx = offset.x,
                      dy = offset.y,
                      newX = cos * dx - sin * dy,
                      newY = sin * -dx - cos * dy;

                return transform.position + new Vector2(newX, newY);
            }
        }

        /// <summary>
        /// Checks for collision with all other colliders.
        /// </summary>
        public override void CollisionCheck()
        {
            collisionsLastFrame = new List<Collider>(collisionsThisFrame);
            collisionsThisFrame = new List<Collider>();

            Collider.allColliders.RemoveAll(n => n.isDestroyed);

            foreach (var collider in Collider.allColliders)
            {
                if (Object.ReferenceEquals(this, collider) || collider.isDestroyed)
                    continue;

                if (collider is CircleCollider && Collider.CircleIntersectsCircle(this, collider as CircleCollider))
                    collisionsThisFrame.Add(collider);
                else if (collider is PolygonCollider && Collider.CircleIntersectsPolygon(this, collider as PolygonCollider))
                    collisionsThisFrame.Add(collider);
            }

            foreach (var collider in collisionsThisFrame)
            {
                if (collisionsLastFrame.Contains(collider))
                    gameObject.OnCollisionStay(collider);
                else
                    gameObject.OnCollisionEnter(collider);
            }

            foreach (var collider in collisionsLastFrame)
            {
                if (!collisionsThisFrame.Contains(collider))
                    gameObject.OnCollisionExit(collider);
            }
        }

        /// <summary>
        /// The Draw method.
        /// </summary>
        public override void Draw()
        {
            if (drawCollider)
            {
                Rendering.StartDrawing(this);

                Rendering.DrawCircle(offset, radius, 2, collisionsLastFrame.Count == 0 ? Color.Yellow : Color.Red);
            }
        }
    }
}