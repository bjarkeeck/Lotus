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
        public override void Draw()
        {
            Rendering.StartDrawing(this,false);

            Rendering.DrawTexture(0, 0, 800, 800, Textures.GetTexture("fancy"), 1);
            Rendering.DrawTexture(0, 0, 400, 400, Textures.GetTexture("fancy"), 1);
        }
    }
}
