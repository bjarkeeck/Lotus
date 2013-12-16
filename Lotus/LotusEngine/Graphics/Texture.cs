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
        public Texture(uint OpenGLName, string Name, Bitmap Bitmap, string Path)
        {
            this.OpenGLName = OpenGLName;
            this.Name = Name;
            this.Bitmap = Bitmap;
            this.Path = Path;
        }

        public Vector2 Origin
        {
            get
            {
                return new Vector2(Bitmap.Height / 2, Bitmap.Height / 2f);
            }
        }

        public readonly uint OpenGLName;
        public readonly string Name;
        public readonly Bitmap Bitmap;
        public readonly string Path;

        public override string ToString()
        {
            return Name;
        }
    }
}
