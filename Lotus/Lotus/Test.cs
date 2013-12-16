using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LotusEngine;
using System.Drawing;

namespace Lotus
{
    class Test : Component
    {
        public override void Update()
        {
            transform.rotation += 20 * Time.DeltaTime;
            transform.position += Vector2.right * Time.DeltaTime * 20;
        }

        public override void Draw()
        {
            Rendering.DrawLine(Vector2.zero, Input.MousePosition, 2, Color.Red);//new Vector2(Settings.Screen.Width / 2, Settings.Screen.Height / 2), 2, Color.Red);

            var intersects = Collider.LineIntersects(Vector2.zero, Input.MousePosition);//new Vector2(Settings.Screen.Width / 2, Settings.Screen.Height / 2));

            foreach (var intersect in intersects)
                Rendering.DrawCircle(intersect, 5, 2, Color.Red);
        }
    }
}