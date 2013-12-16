using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusEngine
{
    /// <summary>
    /// Representation of 2D vectors and points.
    /// </summary>
    [Serializable]
    public struct Vector2
    {
        private float x_field, y_field;

        /// <summary>
        /// X component of the vector
        /// </summary>
        public float x
        {
            get { return x_field; }
            set { x_field = value; }
        }

        /// <summary>
        /// Y component of the vector
        /// </summary>
        public float y
        {
            get { return y_field; }
            set { y_field = value; }
        }

        /// <summary>
        /// Returns the length of this vector (Read Only)
        /// </summary>
        public float magnitude
        {
            get { return Mathf.Sqrt(sqrMagnitude); }
        }

        /// <summary>
        /// Returns the squared length of this vector (Read Only)
        /// </summary>
        public float sqrMagnitude
        {
            get { return x_field * x_field + y_field * y_field; }
        }

        /// <summary>
        /// Returns this vector with a magnitude of 1 (Read Only)
        /// </summary>
        public Vector2 normalized
        {
            get
            {
                if (magnitude == 0)
                    return this;
                return new Vector2(x_field / magnitude, y_field / magnitude);
            }
        }

        /// <summary>
        /// Shorthand for writing Vector2(1, 1);
        /// </summary>
        public static Vector2 one { get { return new Vector2(1, 1); } }

        /// <summary>
        /// Shorthand for writing Vector2(0, 0);
        /// </summary>
        public static Vector2 zero { get { return new Vector2(0, 0); } }

        /// <summary>
        /// Shorthand for writing Vector2(1, 0);
        /// </summary>
        public static Vector2 right { get { return new Vector2(1, 0); } }

        /// <summary>
        /// Shorthand for writing Vector2(0, 1);
        /// </summary>
        public static Vector2 down { get { return new Vector2(0, 1); } }

        /// <summary>
        /// Initializes a new instance of the Vector2 struct with the specified components
        /// </summary>
        /// <param name="x">x component of the vector</param>
        /// <param name="y">y component of the vector</param>
        public Vector2(float x, float y)
        {
            this.x_field = x;
            this.y_field = y;
        }

        // Operators
        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x_field + v2.x_field, v1.y_field + v2.y_field);
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x_field - v2.x_field, v1.y_field - v2.y_field);
        }

        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            return v1.x == v2.x && v1.y == v2.y;
        }
        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return v1.x != v2.x || v1.y != v2.y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj is Vector2 == false)
                return false;
            return x == ((Vector2)obj).x && y == ((Vector2)obj).y;
        }
        public override int GetHashCode()
        {
            return (x + "-" + y).GetHashCode();
        }

        public static Vector2 operator *(Vector2 v1, float a)
        {
            return new Vector2(v1.x_field * a, v1.y_field * a);
        }
        public static Vector2 operator *(float a, Vector2 v1)
        {
            return v1 * a;
        }

        public static Vector2 operator /(Vector2 v1, float a)
        {
            return new Vector2(v1.x_field / a, v1.y_field / a);
        }
        public static Vector2 operator /(float a, Vector2 v1)
        {
            return v1 / a;
        }

        /// <summary>
        /// Returns a (not so) nicely formatted string for this vector
        /// </summary>
        public override string ToString()
        {
            return String.Format("{0};{1}", x_field, y_field);
        }

        /// <summary>
        /// Returns the angle in degress between two vectors
        /// </summary>
        public static float Angle(Vector2 v1, Vector2 v2)
        {
            return FloatRad2Deg(Mathf.Atan2(v2.y - v1.y, v2.x - v1.x));
        }

        /// <summary>
        /// Returns a copy of vector with its magnitude clamped to maxLength
        /// </summary>
        public static Vector2 ClampMagnitude(Vector2 vector, float maxLength)
        {
            if (vector.magnitude > maxLength)
                return vector.normalized * maxLength;
            else
                return vector;
        }

        public static Vector2 DirectionVector(float degree)
        {
            return new Vector2(Mathf.Cos(FloatDeg2Rad(degree)), Mathf.Sin(FloatDeg2Rad(degree)));
        }

        /// <summary>
        /// Returns the distance between two vectors
        /// </summary>
        public static float Distance(Vector2 a, Vector2 b)
        {
            return (a - b).magnitude;
        }
        /// <summary>
        /// Returns the lowest distance between point p and a line defined by the endpoints v and w.
        /// Source: http://stackoverflow.com/questions/849211/shortest-distance-between-a-point-and-a-line-segment
        /// Adapted from C++
        /// </summary>
        public static float Distance(Vector2 p, Vector2 v, Vector2 w)
        {
            float l2 = (w - v).sqrMagnitude;  // i.e. |w-v|^2 -  avoid a sqrt
            if (l2 == 0.0) return Vector2.Distance(p, v);   // v == w case
            // Consider the line extending the segment, parameterized as v + t (w - v).
            // We find projection of point p onto the line. 
            // It falls where t = [(p-v) . (w-v)] / |w-v|^2
            float t = Vector2.Dot(p - v, w - v) / l2;
            if (t < 0.0) return Vector2.Distance(p, v);       // Beyond the 'v' end of the segment
            else if (t > 1.0) return Vector2.Distance(p, w);  // Beyond the 'w' end of the segment
            Vector2 projection = v + t * (w - v);  // Projection falls on the segment
            return Vector2.Distance(p, projection);
        }

        /// <summary>
        /// Returns the dot product of two vectors
        /// </summary>
        public static float Dot(Vector2 v1, Vector2 v2)
        {
            return (v1.x_field * v2.x_field + v1.y_field * v2.y_field);
        }

        /// <summary>
        /// Linearly interpolates between two vectors by amount of t
        /// </summary>
        public static Vector2 Lerp(Vector2 from, Vector2 to, float t)
        {
            if (t >= 1)
                return to;
            else if (t <= 0)
                return from;
            else
                return from + (to - from) * t;
        }

        /// <summary>
        /// Returns a vector that is made from the largest components of two vectors
        /// </summary>
        public static Vector2 Max(Vector2 v1, Vector2 v2)
        {
            return new Vector2(Math.Max(v1.x, v2.x), Math.Max(v1.y, v2.y));
        }

        /// <summary>
        /// Returns a vector that is made from the smallest components of two vectors
        /// </summary>
        public static Vector2 Min(Vector2 v1, Vector2 v2)
        {
            return new Vector2(Math.Min(v1.x, v2.x), Math.Min(v1.y, v2.y));
        }

        /// <summary>
        /// Multiplies two vectors component-wise
        /// </summary>
        public static Vector2 Scale(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x * v2.x, v1.y * v2.y);
        }

        /// <summary>
        /// Returns the float clamped between [0... 1]
        /// </summary>
        public static float FloatClamp(float value)
        {
            if (value > 1)
                return 1;
            else if (value < 0)
                return 0;
            else
                return value;
        }

        /// <summary>
        /// Returns float linearly interpolated between two floats by amount of t
        /// </summary>
        public static float FloatLerp(float from, float to, float t)
        {
            if (t >= 1)
                return to;
            else if (t <= 0)
                return from;
            else
                return from + (to - from) * t;
        }

        /// <summary>
        /// Returns the float clamped between [min... max]
        /// </summary>
        public static float FloatClamp(float value, float min, float max)
        {
            if (value > max)
                return max;
            else if (value < min)
                return min;
            else
                return value;
        }

        /// <summary>
        /// Returns given radians converted to degrees
        /// </summary>
        public static float FloatRad2Deg(float radian)
        {
            return (radian * 180 / Mathf.PI);
        }

        /// <summary>
        /// Returns given degrees converted to radians
        /// </summary>
        public static float FloatDeg2Rad(float degrees)
        {
            return (degrees * Mathf.PI / 180);
        }

        /// <summary>
        /// Tries to parse out a Vector2 from the given string.
        /// </summary>
        public static bool TryParse(string str, out Vector2 result)
        {
            result = Vector2.zero;
            string[] split = str.Split(';');

            if (split.Length != 2)
                return false;

            float x, y;

            if (!float.TryParse(split[0], out x))
                return false;
            if (!float.TryParse(split[1], out y))
                return false;

            result = new Vector2(x, y);

            return true;
        }
    }
}
