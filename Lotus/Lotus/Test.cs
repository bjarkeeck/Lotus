using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LotusEngine;
using System.Drawing;

namespace Lotus {
	class Test : Component {
		public override void Update () {
			transform.rotation += 20 * Time.DeltaTime;
		}
		//
		public override void Draw () {
			Rendering.DrawLine (Vector2.zero, new Vector2 (Settings.Screen.Width / 2, Settings.Screen.Height / 2), 2, Color.Red);

			var intersects = Collider.LineIntersects (Vector2.zero, new Vector2 (Settings.Screen.Width / 2, Settings.Screen.Height / 2));

			foreach (var intersect in intersects)
				Rendering.DrawCircle (intersect, 5, 2, Color.Red);
		}
	}
}