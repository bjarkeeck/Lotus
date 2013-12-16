using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGL;
using System.Drawing;

namespace LotusEngine
{
    /// <summary>
    /// Renders a GameObject in the current Scene.
    /// </summary>
    public class Renderer : Component
    {
        [Serialize]
        public Sprite sprite;
        [Serialize]
        public Vector2 drawOffset;
        [Serialize]
        public bool center;
        [Serialize]
        public bool additiveBlending;
        [Serialize]
        public bool animate = true;
        
        public override void Update()
        {
            if (animate)
                sprite.Animate();
        }

        public override void Draw()
        {
            if (sprite != null)
            {

                Rendering.StartDrawing(this);
                Rendering.DrawTexture(sprite.currentTexture, center ? drawOffset - new Vector2(sprite.currentTexture.Bitmap.Width * 0.5f, sprite.currentTexture.Bitmap.Height * 0.5f) : drawOffset, Color.White, 1, TextureFlip.None, additiveBlending);
            }
        }
    }
}
