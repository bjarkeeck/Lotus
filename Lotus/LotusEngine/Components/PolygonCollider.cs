using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace LotusEngine
{
    /// <summary>
    /// An arbitrarily shaped polygon collider.
    /// </summary>
    public abstract class PolygonCollider : Collider
    {
        /// <summary>
        /// Create a new PolygonCollider instance.
        /// </summary>
        public PolygonCollider()
        {
            if (Collider.allColliders != null)
                Collider.allColliders.Add(this);
        }

        /// <summary>
        /// The rotation of the collider.
        /// </summary>
        [Serialize]
        public float rotation;
        /// <summary>
        /// Whether to use the collider's rotation or the GameObject's Transform's rotation.
        /// </summary>
        [Serialize]
        public bool useColliderRotation;
        /// <summary>
        /// How much to offset the collider from the center. Affected by rotation and scale.
        /// </summary>
        [Serialize]
        public Vector2 offset;

        /// <summary>
        /// The center of the collider (the average position of all points in the polygon).
        /// </summary>
        public Vector2 center { get; private set; }

        private Vector2[] localPoints_field;
        /// <summary>
        /// Gets an array of each point in the polygon defining the collider in local coordinates relative to the center point.
        /// </summary>
        public Vector2[] localPoints
        {
            get
            {
                return localPoints_field;
            }
        }

        private Vector2[] worldPoints_field;
        /// <summary>
        /// Gets an array of each point in the polygon defining the collider in world coordinates.
        /// </summary>
        public Vector2[] worldPoints
        {
            get
            {
                return worldPoints_field;
            }
        }

        /// <summary>
        /// The actual, unrotated point data of the polygon.
        /// </summary>
        public Vector2[] points { get; set; }

        /// <summary>
        /// The Update method.
        /// </summary>
        public override void Update()
        {
            RefreshVariables();
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
                if (Object.ReferenceEquals(collider, this) || collider.isDestroyed)
                    continue;

                if (collider is CircleCollider && Collider.CircleIntersectsPolygon(collider as CircleCollider, this))
                    collisionsThisFrame.Add(collider);
                else if (collider is PolygonCollider && Collider.PolygonIntersectsPolygon(this, collider as PolygonCollider))
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
                if (Settings.Editor.EditorIsRunning)
                    RefreshVariables();

                Rendering.StartDrawing(this, !useColliderRotation);

                if (useColliderRotation)
                    Rendering.gl.Rotate(0, 0, rotation);

                for (int i = 0; i < points.Length; i++)
                    Rendering.DrawLine(points[i], points[(i + 1) % points.Length], 2, collisionsLastFrame.Count == 0 ? Color.Yellow : Color.Red);

                //Rendering.StartDrawing(this);

                //for (int i = 0; i < localPoints_field.Length; i++)
                //    Rendering.DrawLine(localPoints_field[i], localPoints_field[(i + 1) % localPoints_field.Length], 2, collisionsLastFrame.Count == 0 ? Color.Yellow : Color.Red);
            }
        }

        /// <summary>
        /// Refreshes all relevant variables.
        /// </summary>
        private void RefreshVariables()
        {
            float rot = useColliderRotation ? -rotation : -transform.rotation;

            Vector2 size = GetSize();

            float width = transform.scale.x * size.x,
                  height = transform.scale.y * size.y,
                  x = transform.position.x + offset.x * transform.scale.x - width * 0.5f,
                  y = transform.position.y + offset.y * transform.scale.y - height * 0.5f;

            center = new Vector2(x, y);

            worldPoints_field = new Vector2[points.Length];
            localPoints_field = new Vector2[points.Length];
            
            float sin = Mathf.Sin(Vector2.FloatDeg2Rad(rot));
            float cos = Mathf.Cos(Vector2.FloatDeg2Rad(rot));

            for (int i = 0; i < points.Length; i++)
            {
                float dx = points[i].x,
                      dy = points[i].y,
                      newX = cos * dx - sin * dy + width * 0.5f,
                      newY = sin * -dx - cos * dy + height * 0.5f;

                localPoints_field[i] = new Vector2(newX, newY);
                worldPoints_field[i] = new Vector2(newX + x, newY + y);
            }

            //var c = new Vector2(transform.position.x, transform.position.y);

            //for (int i = 0; i < points.Length; i++)
            //{
            //    float dx = localPoints_field[i].x - c.x,
            //          dy = localPoints_field[i].y - c.y,
            //          newX = cos * dx - sin * dy,
            //          newY = sin * -dx - cos * dy;

            //    localPoints_field[i] = new Vector2(newX, newY);
            //    worldPoints_field[i] = new Vector2(newX + c.x, newY + c.y);
            //}
        }

        /// <summary>
        /// Gets the size of the unrotated point data.
        /// </summary>
        public Vector2 GetSize()
        {
            return new Vector2(FindRightMostX() - FindLeftMostX(), FindBottomMostY() - FindTopMostY());
        }

        public float FindLeftMostX()
        {
            return points.Min(n => n.x);
        }

        public float FindRightMostX()
        {
            return points.Max(n => n.x);
        }

        public float FindTopMostY()
        {
            return points.Min(n => n.y);
        }

        public float FindBottomMostY()
        {
            return points.Max(n => n.y);
        }
    }
}