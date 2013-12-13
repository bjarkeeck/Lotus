using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using SharpGL;

namespace LotusEngine
{
    public class Texture
    {
        public Texture(uint OpenGLName, string Name, Bitmap Bitmap)
        {
            this.OpenGLName = OpenGLName;
            this.Name = Name;
            this.Bitmap = Bitmap;
        }

        public readonly uint OpenGLName;
        public readonly string Name;
        public readonly Bitmap Bitmap;
    }
}
