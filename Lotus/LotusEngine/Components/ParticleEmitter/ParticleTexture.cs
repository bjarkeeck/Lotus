using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusEngine.Components
{
    public class ParticleTexture
    {
        private Color startColor;
        private Color endColor;
        private float startScale;
        private float endScale;

        public bool Rotate { get; set; }
        public Texture Texture {get;set;}
        public bool DrawAdditive { get; set; }

        public ParticleTexture(Texture texture, Color startColor, Color endColor, float startScale = 1, float endScale = 1, bool rotate = true, bool drawAdditive = true)
        {
            this.Texture = texture;
            this.startColor = startColor;
            this.endColor = endColor;
            this.startScale = startScale;
            this.endScale = endScale;
            this.Rotate = rotate;
            this.DrawAdditive = drawAdditive;
        }

        public Color Color(int life, int currentLife)
        {
            if (currentLife < 0)
                return endColor;

            float scaleFactor = 1 - (float)currentLife / (float)life;
            return System.Drawing.Color.FromArgb(
                (byte)(startColor.A + (endColor.A - startColor.A) * scaleFactor),
                (byte)(startColor.R + (endColor.R - startColor.R) * scaleFactor),
                (byte)(startColor.G + (endColor.G - startColor.G) * scaleFactor),
                (byte)(startColor.B + (endColor.B - startColor.B) * scaleFactor)
            );

        }

        public float Scale(int life, int currentLife)
        {
            if (currentLife < 0)
                return endScale;

            float scaleFactor = 1 - (float)currentLife / (float)life;

            return startScale + (endScale - startScale) * scaleFactor;
        }

    }
}
