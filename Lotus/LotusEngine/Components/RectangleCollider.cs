using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusEngine.Components
{
    class RectangleCollider : PolygonCollider
    {
        [Serialize]
        private float width;
        public float Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;

                UpdatePoints();
            }
        }
        [Serialize]
        private float height;
        public float Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;

                UpdatePoints();
            }
        }

        public float Left
        {
            get
            {
                return worldPoints.Min(n => n.x);
            }
        }

        public float Right
        {
            get
            {
                return worldPoints.Max(n => n.x);
            }
        }

        public float Top
        {
            get
            {
                return worldPoints.Min(n => n.y);
            }
        }

        public float Bottom
        {
            get
            {
                return worldPoints.Max(n => n.y);
            }
        }

        public override void Start()
        {
            UpdatePoints();
        }

        public override void Update()
        {
            if (Settings.Editor.EditorIsRunning)
                UpdatePoints();
        }

        private void UpdatePoints()
        {
            points = new Vector2[]{
                new Vector2(Width * -0.5f + offset.x, Height * -0.5f + offset.y),
                new Vector2(Width * 0.5f + offset.x, Height * -0.5f + offset.y),
                new Vector2(Width * 0.5f + offset.x, Height * 0.5f + offset.y),
                new Vector2(Width * -0.5f + offset.x, Height * 0.5f + offset.y)
            };
        }
    }
}
