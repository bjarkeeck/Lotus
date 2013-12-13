using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

namespace LotusEngine
{
    [Serializable]
    public class Sprite
    {
        public Sprite(string path)
        {
            textures_field = new List<Texture>();

            if (!File.Exists(path))
            {
                throw new ArgumentException("Attempted to load sprite from path, but path does not exist: " + path);
            }

            XDocument doc = XDocument.Load(path);
            XElement root = doc.Element("sprite");
            name = root.Attribute("name").Value;
            fps = float.Parse(root.Attribute("fps").Value);

            foreach (var element in root.Elements("texture"))
            {
                string texture = element.Value;

                if (Textures.TextureExists(texture))
                    textures_field.Add(Textures.GetTexture(texture));
            }
        }

        public void SaveSprite()
        {
            XDocument doc = new XDocument();

            XElement xmlSprite = new XElement("sprite");
            doc.Add(xmlSprite);

            xmlSprite.Add(new XAttribute("name", name));
            xmlSprite.Add(new XAttribute("fps", fps.ToString()));

            foreach (var texture in textures_field)
                xmlSprite.Add(new XElement("texture", texture.Name));
        }

        public string name { get; set; }
        public string path { get; set; }
        public float fps { get; set; }
        public int currentIndex { get; set; }
        public float currentIndexTime { get; set; }

        private List<Texture> textures_field;
        public List<Texture> textures { get { return new List<Texture>(textures_field); } }
    }
}
