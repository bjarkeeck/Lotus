using System;

namespace LotusEngine {
	public static class RandomLotus {
		private static Random rand = new Random ();

		public static float Range (float min, float max) {
			return min + (rand.NextDouble () * (max - min));
		}

		public static int Range (int min, int max) {
			return rand.Next (min, max);
		}

		public static float Range (float max) {
			return (rand.NextDouble () * (max));
		}

		public static int Range (int max) {
			return rand.Next (0, max);
		}

		public static float Value () {
			return (float)rand.NextDouble ();
		}
	}
}