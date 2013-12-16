using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusEngine
{
    public abstract class Collider : Component
    {
        /// <summary>
        /// Performs collision on all Colliders.
        /// </summary>
        public static void CollisionCheckAll()
        {
            allColliders = GameObject.FindAllComponents<Collider>();

            foreach (var go in Scene.ActiveScene.sceneObjects)
                go.CheckForCollisions();
        }
        
        /// <summary>
        /// A list of all colliders in the active scene.
        /// </summary>
        internal static List<Collider> allColliders;

        /// <summary>
        /// Whether to draw this collider in the Draw phase.
        /// </summary>
        public bool drawCollider;
        /// <summary>
        /// A list of all the colliders this collider collided with last frame.
        /// </summary>
        internal List<Collider> collisionsLastFrame = new List<Collider>();
        /// <summary>
        /// A list of all the colliders this collider collided with this frame.
        /// </summary>
        protected List<Collider> collisionsThisFrame = new List<Collider>();
        
        /// <summary>
        /// Checks for collision with all other colliders.
        /// </summary>
        public abstract void CollisionCheck();

        /// <summary>
        /// Checks whether a given line collides with any colliders.
        /// </summary>
        /// <param name="a">Starting point of the line.</param>
        /// <param name="b">Ending point of the line.</param>
        /// <returns>True if a line hits any collider, otherwise false</returns>
        public static bool LineCollisionCheck(Vector2 a, Vector2 b)
        {
            foreach (var collider in allColliders)
            {
                if (collider is CircleCollider && LineIntersectsCircle(collider as CircleCollider, a, b))
                    return true;
                else if (collider is PolygonCollider && LineIntersectsPolygon(collider as PolygonCollider, a, b))
                    return true;
            }

            return false;
        }
        /// <summary>
        /// Checks whether a given line collides with colliders on any of the included GameObjects.
        /// </summary>
        /// <param name="a">Starting point of the line.</param>
        /// <param name="b">Ending point of the line.</param>
        /// <param name="includeObjects">The list of GameObjects to include.</param>
        /// <returns>True if a line hits any collider on an included GameObject, otherwise.</returns>
        public static bool LineCollisionCheck(Vector2 a, Vector2 b, params GameObject[] includeObjects)
        {
            foreach (var collider in allColliders)
            {
                if (!includeObjects.Contains(collider.gameObject))
                    continue;

                if (collider is CircleCollider && LineIntersectsCircle(collider as CircleCollider, a, b))
                    return true;
                else if (collider is PolygonCollider && LineIntersectsPolygon(collider as PolygonCollider, a, b))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets all intersection points of a given line with any colliders.
        /// </summary>
        /// <param name="a">Starting point of the line.</param>
        /// <param name="b">Ending point of the line.</param>
        /// <returns>A list of the points where the line intersected any colliders.</returns>
        public static List<Vector2> LineIntersects(Vector2 a, Vector2 b)
        {
            List<Vector2> result = new List<Vector2>();

            List<Vector2> intersects;

            foreach (var collider in allColliders)
            {
                if (collider is PolygonCollider && LineIntersectsPolygon(collider as PolygonCollider, a, b, out intersects))
                    result.AddRange(intersects);
                if (collider is CircleCollider && LineIntersectsCircle(collider as CircleCollider, a, b, out intersects))
                    result.AddRange(intersects);
            }

            return result;
        }
        /// <summary>
        /// Gets all intersection points of a given line with colliders on any of the included GameObjects.
        /// </summary>
        /// <param name="a">Starting point of the line.</param>
        /// <param name="b">Ending point of the line.</param>
        /// <param name="includeObjects">The list of GameObjects to include.</param>
        /// <returns>A list of the points where the line intersected colliders on any of the included GameObjects.</returns>
        public static List<Vector2> LineIntersects(Vector2 a, Vector2 b, params GameObject[] includeObjects)
        {
            List<Vector2> result = new List<Vector2>();

            List<Vector2> intersects;

            foreach (var collider in allColliders)
            {
                if (!includeObjects.Contains(collider.gameObject))
                    continue;

                if (collider is PolygonCollider && LineIntersectsPolygon(collider as PolygonCollider, a, b, out intersects))
                    result.AddRange(intersects);
                if (collider is CircleCollider && LineIntersectsCircle(collider as CircleCollider, a, b, out intersects))
                    result.AddRange(intersects);
            }

            return result;
        }

        /// <summary>
        /// Checks whether two given PolygonColliders intersect one another.
        /// </summary>
        /// <param name="a">One PolygonCollider.</param>
        /// <param name="b">The other PolygonCollider.</param>
        /// <returns>True if the two PolygonColliders intersect, otherwise false.</returns>
        public static bool PolygonIntersectsPolygon(PolygonCollider a, PolygonCollider b)
        {
            Vector2[] aPoints = a.worldPoints,
                      bPoints = b.worldPoints;

            Vector2 result;

            for (int i1 = 0; i1 < aPoints.Length; i1++)
                for (int i2 = 0; i2 < bPoints.Length; i2++)
                    if (LineIntersectsLine(aPoints[i1],
                                           aPoints[(i1 + 1) % aPoints.Length],
                                           bPoints[i2],
                                           bPoints[(i2 + 1) % bPoints.Length],
                                           out result))
                        return true;

            return false;
        }

        /// <summary>
        /// Checks whether a given CircleCollider intersects with a given PolygonCollider.
        /// </summary>
        /// <param name="circle">The given CircleCollider.</param>
        /// <param name="box">The given PolygonCollider.</param>
        /// <returns>True if the two Colliders intersect, otherwise false.</returns>
        public static bool CircleIntersectsPolygon(CircleCollider circle, PolygonCollider polygon)
        {
            var points = polygon.worldPoints;
            if (PointInPolygon(circle.center, polygon))
                return true;

            for (int i = 0; i < points.Length; i++)
                if (LineIntersectsCircle(circle, points[i], points[(i + 1) % points.Length]))
                    return true;

            return false;
        }

        /// <summary>
        /// Checks whether two given CircleColliders intersect one another.
        /// </summary>
        /// <param name="circle1">One CircleCollider.</param>
        /// <param name="circle2">The other CircleCollider.</param>
        /// <returns>True if the two CircleColliders intersect, otherwise false.</returns>
        public static bool CircleIntersectsCircle(CircleCollider circle1, CircleCollider circle2)
        {
            float distance = Vector2.Distance(circle1.center, circle2.center);
            return distance <= circle1.radius + circle2.radius;
        }

        /// <summary>
        /// Checks whether a given point is within the polygon defining a PolygonCollider.
        /// </summary>
        /// <param name="point">The point to check.</param>
        /// <param name="polygon">The PolygonCollider to check within.</param>
        /// <returns>True if the point is within the PolygonCollider, otherwise false.</returns>
        public static bool PointInPolygon(Vector2 point, PolygonCollider polygon)
        {
            List<Vector2> result;

            if (LineIntersectsPolygon(polygon, point, point - new Vector2(-1000000, 0), out result))
            {
                if (result.Count % 2 == 0)
                    return false;
                else
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks whether a given point is within a given .
        /// </summary>
        /// <param name="point">The given point.</param>
        /// <param name="circle">The given CircleCollider.</param>
        /// <returns>True if the point is within the Circle, otherwise false.</returns>
        public static bool PointInCircle(Vector2 point, CircleCollider circle)
        {
            return Vector2.Distance(point, circle.center) <= circle.radius;
        }

        /// <summary>
        /// Checks whether a given line intersects with a given CircleCollider.
        /// </summary>
        /// <param name="circle">The given CircleCollider.</param>
        /// <param name="a">Starting point of the line.</param>
        /// <param name="b">Ending point of the line.</param>
        /// <returns>True if the line intersects the CircleCollider, otherwise false.</returns>
        public static bool LineIntersectsCircle(CircleCollider circle, Vector2 a, Vector2 b)
        {
            float dist = Vector2.Distance(circle.center, a, b);

            return dist <= circle.radius;
        }
        /// <summary>
        /// Checks whether a given line intersects with a given CircleCollider and returns the intersections points in an out parameter.
        /// </summary>
        /// <param name="circle">The given CircleCollider.</param>
        /// <param name="a">Starting point of the line.</param>
        /// <param name="b">Ending point of the line.</param>
        /// <param name="intersects">The out parameter which contains the intersections points with the CircleCollider if there were any.</param>
        /// <returns>True if the line intersects the CircleCollider, otherwise false.</returns>
        public static bool LineIntersectsCircle(CircleCollider circle, Vector2 a, Vector2 b, out List<Vector2> intersects)
        {
            intersects = new List<Vector2>();

            if (!LineIntersectsCircle(circle, a, b))
                return false;

            Vector2 center = circle.center;
            float dx, dy, A, B, C, det, t;

            dx = b.x - a.x;
            dy = b.y - a.y;

            A = dx * dx + dy * dy;
            B = 2 * (dx * (a.x - center.x) + dy * (a.y - center.y));
            C = (a.x - center.x) * (a.x - center.x) + (a.y - center.y) * (a.y - center.y) - circle.radius * circle.radius;

            det = B * B - 4 * A * C;

            if (A <= 0.0000001 || det < 0)
                // No solutions
                return false;
            else if (det == 0)
            {
                // One solution
                t = -B / (2 * A);

                if (0 <= t && t <= 1)
                    intersects.Add(new Vector2(a.x + t * dx, a.y + t * dy));
            }
            else
            {
                // Two solutions
                t = (-B + Mathf.Sqrt(det)) / (2 * A);
                if (0 <= t && t <= 1)
                    intersects.Add(new Vector2(a.x + t * dx, a.y + t * dy));
                t = (-B - Mathf.Sqrt(det)) / (2 * A);
                if (0 <= t && t <= 1)
                    intersects.Add(new Vector2(a.x + t * dx, a.y + t * dy));
            }

            return true;
        }

        /// <summary>
        /// Checks whether a given line intersects with a given PolygonCollider.
        /// </summary>
        /// <param name="circle">The given PolygonCollider.</param>
        /// <param name="a">Starting point of the line.</param>
        /// <param name="b">Ending point of the line.</param>
        /// <param name="intersects">The out parameter which contains the intersections points with the PolygonCollider if there were any.</param>
        /// <returns>True if the line intersects the PolygonCollider, otherwise false.</returns>
        public static bool LineIntersectsPolygon(PolygonCollider polygon, Vector2 a, Vector2 b, out List<Vector2> intersections)
        {
            var points = polygon.worldPoints;

            intersections = new List<Vector2>();

            Vector2 result;

            for (int i = 0; i < points.Length; i++)
                if (LineIntersectsLine(a, b, points[i], points[(i + 1) % points.Length], out result))
                    intersections.Add(result);

            return intersections.Count > 0;
        }
        /// <summary>
        /// Checks whether a given line intersects with a given PolygonCollider and returns the intersections points in an out parameter.
        /// </summary>
        /// <param name="circle">The given PolygonCollider.</param>
        /// <param name="a">Starting point of the line.</param>
        /// <param name="b">Ending point of the line.</param>
        /// <returns>True if the line intersects the PolygonCollider, otherwise false.</returns>
        public static bool LineIntersectsPolygon(PolygonCollider polygon, Vector2 a, Vector2 b)
        {
            var points = polygon.worldPoints;

            Vector2 result;

            for (int i = 0; i < points.Length; i++)
                if (LineIntersectsLine(a, b, points[i], points[(i + 1) % points.Length], out result))
                    return true;

            return false;
        }

        /// <summary>
        /// Checks if two given lines intersect with one another and returns the intersection point (if any) in an out parameter.
        /// Source: http://stackoverflow.com/questions/3746274/line-intersection-with-aabb-rectangle.
        /// Edited to implement Cohen-Sutherland type pruning for efficiency.
        /// </summary>
        /// <param name="a1">Starting point of line a.</param>
        /// <param name="a2">Ending point of line a.</param>
        /// <param name="b1">Starting point of line b.</param>
        /// <param name="b2">Ending point of line b.</param>
        /// <param name="intersection">The out parameter which contains the intersection point if there was any.</param>
        /// <returns>True if the two lines intersect, otherwise false.</returns>
        public static bool LineIntersectsLine(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, out Vector2 intersection)
        {
            intersection = Vector2.zero;

            Vector2 bLowerLeft = new Vector2(b1.x < b2.x ? b1.x : b2.x, b1.y > b2.y ? b1.y : b2.y);
            Vector2 bUpperRight = new Vector2(b1.x > b2.x ? b1.x : b2.x, b1.y < b2.y ? b1.y : b2.y);

            if ((a1.x < bLowerLeft.x && a2.x < bLowerLeft.x)
               || (a1.y > bLowerLeft.y && a2.y > bLowerLeft.y)
               || (a1.x > bUpperRight.x && a2.x > bUpperRight.x)
               || (a1.y < bUpperRight.y && a2.y < bUpperRight.y))
                return false;

            Vector2 b = a2 - a1;
            Vector2 d = b2 - b1;

            float bDotDPerp = b.x * d.y - b.y * d.x;

            // If b dot d == 0, it means the lines are parallel and have infinite intersection points
            if (bDotDPerp == 0)
                return false;

            Vector2 c = b1 - a1;
            float t = (c.x * d.y - c.y * d.x) / bDotDPerp;
            if (t < 0 || t > 1)
                return false;

            float u = (c.x * b.y - c.y * b.x) / bDotDPerp;
            if (u < 0 || u > 1)
                return false;

            intersection = a1 + t * b;

            return true;
        }
    }
}