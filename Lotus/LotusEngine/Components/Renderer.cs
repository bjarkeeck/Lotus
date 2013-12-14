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
    /// kaoishdiauhsdiauhdsiauhsdisasdasd as dasd
    /// </summary>
    public class Renderer : Component
    {
        public override void Start()
        {
            this.transform.rotation = 0;
        }


        public override void Update()
        {
            this.transform.rotation += 20f * Time.DeltaTime;
        }

        public override void Draw()
        {
            //Rendering.StartDrawing(this);
            //Rendering.DrawTexture(Textures.GetTexture("tempParticle"), Vector2.one, Color.White, 1f);
        }
    }
}
