using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Runtime.Serialization;

namespace LotusEngine
{
    [Serializable]
    public class Sprite : ISerializable
    {
        /// <summary>
        /// Creates a new Sprite instance.
        /// </summary>
        /// <param name="name">The name of the Sprite.</param>
        /// <param name="fps">The FPS of the Sprite.</param>
        public Sprite(string name, float fps)
        {
            this.name = name;
            this.fps = fps;
            path = Settings.Assets.SpritePath + "\\" + name + ".sprite";
            textures_field = new List<Texture>();
        }

        public Sprite(FileInfo file)
        {
            path = file.FullName;

            textures_field = new List<Texture>();
            
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

        public Sprite(SerializationInfo info, StreamingContext ct)
        {
            name = (string)info.GetValue("name", typeof(string));

            if (Exists(name))
            {
                Sprite sprite = allSprites[name];

                this.fps = sprite.fps;
                this.path = sprite.path;
                this.textures_field = sprite.textures;
            }
        }

        public Sprite(Sprite sprite)
        {
            this.name = sprite.name;
            this.fps = sprite.fps;
            this.path = sprite.path;
            this.textures_field = sprite.textures;
        }

        private static Dictionary<string, Sprite> allSprites = new Dictionary<string, Sprite>();

        public static List<Sprite> GetAllSprites()
        {
            return new List<Sprite>(allSprites.Values);
        }

        public static Sprite GetSprite(string name)
        {
            if (Exists(name))
                return new Sprite(allSprites[name]);
            return null;
        }

        public static bool Exists(string name)
        {
            return allSprites.ContainsKey(name);
        }

        public static void LoadAllSprites()
        {
            if (!Directory.Exists(Settings.Assets.SpritePath))
                Directory.CreateDirectory(Settings.Assets.SpritePath);

            DirectoryInfo dirInfo = new DirectoryInfo(Settings.Assets.SpritePath);

            allSprites = new Dictionary<string, Sprite>();

            foreach (var file in dirInfo.GetFiles())
            {
                if (file.Extension == ".sprite")
                {
                    allSprites.Add(Path.GetFileNameWithoutExtension(file.Name), new Sprite(file));
                }
            }
        }
        public string name { get; set; }
        public string path { get; set; }
        public float fps { get; set; }
        public int currentIndex { get; set; }
        public float currentIndexTime { get; set; }
        
        /// <summary>
        /// The current texture of the Sprite.
        /// </summary>
        public Texture currentTexture
        {
            get
            {
                if (currentIndex >= 0 && currentIndex < textures_field.Count)
                    return textures_field[currentIndex];
                return null;
            }
        }

        private List<Texture> textures_field;
        public List<Texture> textures { get { return new List<Texture>(textures_field); } }

        public void GetObjectData(SerializationInfo info, StreamingContext ct)
        {
            info.AddValue("name", name);
        }

        public override string ToString()
        {
            return name;
        }

        public void RemoveAllTextures()
        {
            textures_field.Clear();
        }

        public void RemoveTexture(Texture texture)
        {
            if (textures_field.Contains(texture))
                textures_field.Remove(texture);
        }

        public void AddTexture(Texture texture)
        {
            if (!textures_field.Contains(texture))
                textures_field.Add(texture);
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

            using (TextWriter writer = new StreamWriter(path, false, Encoding.Unicode))
                writer.Write(doc.ToString());
        }

        /// <summary>
        /// Animate the index of the Sprite according to Time.DeltaTime.
        /// </summary>
        public void Animate()
        {
            currentIndexTime += Time.DeltaTime;

            int index = (int)(currentIndexTime * fps);

            if (index < 0 || index >= textures_field.Count)
            {
                currentIndexTime = 0;
                index = 0;
            }

            currentIndex = index;
        }
    }
}
