using System;

namespace LotusEngine {
    class Mathf {
        public const float Deg2Rad = 0.0174533f;
        public const float Epsilon = 1.4013e-045f;
        public const float ExponentialE = 2.71828f;
        public const float GoldenRatio = 1.61803f;
        public const float Infinity = 1.0f / 0.0f;
        public const float NegativeInfinity = -1.0f / 0.0f;
        public const float PI = 3.14159f;
        public const float Rad2Deg = 57.2958f;
        public const float TAU = PI * 2;

        public static int Abs(int a) { return Math.Abs(a); }
        public static float Abs(float a) { return (float)Math.Abs(a); }
        public static float Acos(float a) { return (float)Math.Acos(a); }
        public static float Asin(float a) { return (float)Math.Asin(a); }
        public static float Atan(float a) { return (float)Math.Atan(a); }
        public static float Atan2(float y,float x) { return (float)Math.Atan2(y,x); }
        public static float Ceil(float a) { return (float)Math.Ceiling(a); }
        public static int CeilToInt(float a) { return (int)Ceil(a); }
        public static int Clamp(int value,int min,int max) {
            return (int)Clamp(value,min,max);
        }
        public static float Clamp(float value,float min,float max) {
            if (max <= min)
                return min;
            return value < min ? min : value > max ? max : value;
        }
        public static float Clamp01(float value) { return value < 0 ? 0 : value > 1 ? 1 : value; }
        public static float Cos(float a) { return (float)Math.Cos(a); }
        public static float Exp(float power) { return (float)Math.Exp(power); }
        public static float Floor(float a) { return (float)Math.Floor(a); }
        public static int FloorToInt(float a) { return (int)Floor(a); }
        public static float Lerp(float from,float to,float t) { return t >= 1 ? to : t < 0 ? from : from + (to - from) * t; }
        public static float Log(float value) { return (float)Math.Log(value); }
        public static float Log10(float value) { return (float)Math.Log10(value); }
        public static int Max(int a,int b) { return Math.Max(a,b); }
        public static float Max(float a,float b) { return Math.Max(a,b); }
        public static int Min(int a,int b) { return Mathf.Min(a,b); }
        public static float Min(float a,float b) { return Math.Min(a,b); }
        public static float Pow(float f,float p) { return (float)Math.Pow(f,p); }
        public static bool RoughlyEqual(float a,float b,float threshold) { return Math.Abs(a - b) <= threshold; }
        public static float Round(float f) { return (float)Math.Round(f); }
        public static float Round(float f,int decimals) { return (float)Math.Round(f,decimals); }
        public static float Round(float f,int decimals,MidpointRounding mode) { return (float)Math.Round(f,decimals,mode); }
        public static int RoundToInt(float f) { return (int)Round(f); }
        public static int RoundToInt(float f,int decimals) { return (int)Round(f,decimals); }
        public static int RoundToInt(float f,int decimals,MidpointRounding mode) { return (int)Round(f,decimals,mode); }
        public static float Sign(float f) { return (float)Math.Sign(f); }
        public static float Sin(float f) { return (float)Math.Sin(f); }
        public static float Sqrt(float f) { return (float)Math.Sqrt(f); }
        public static float Tan(float f) { return (float)Math.Tan(f); }
    }
}
