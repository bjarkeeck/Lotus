using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Reflection;

namespace LotusEngine
{
    /// <summary>
    /// Defines data used to serialize and deserialize a GameObject.
    /// </summary>
    public sealed class Prefab
    {
        public const BindingFlags FindFields = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        #region Constructors
        /// <summary>
        /// Loads a Prefab from the given path.
        /// </summary>
        /// <param name="path">The path to load a Prefab from.</param>
        public Prefab(string path)
        {
            this.path = path;

            SoapFormatter formatter = new SoapFormatter();
            XDocument doc = XDocument.Load(path);
            XElement root = doc.Element("gameobject");

            name = root.Attribute("name").Value;
            zIndex = int.Parse(root.Attribute("zindex").Value);

            components_field = new List<PrefabComponent>();

            foreach (var component in root.Element("components").Elements("component"))
            {
                components_field.Add(new PrefabComponent(component));
            }
        }
        /// <summary>
        /// Creates a prefab from the given GameObject, with the given file path.
        /// </summary>
        /// <param name="gameObject">The GameObject to create the Prefab from.</param>
        /// <param name="name">The name with which this Prefab will be saved.</param>
        public Prefab(GameObject gameObject, string name)
        {
            this.name = name;
            path = Settings.Assets.PrefabPath + "\\" + name + ".prefab";
            zIndex = gameObject.zIndex;

            components_field = new List<PrefabComponent>();

            foreach (var component in gameObject.GetAllComponents())
            {
                components_field.Add(new PrefabComponent(component));
            }
        }
        /// <summary>
        /// Creates a prefab from the given GameObject, with the given file path.
        /// </summary>
        /// <param name="gameObject">The GameObject to create the Prefab from.</param>
        /// <param name="name">The name of the Prefab.</param>
        /// <param name="past">The path at which this Prefab will be saved.</param>
        public Prefab(GameObject gameObject, string name, string path)
        {
            this.name = name;
            this.path = path;
            zIndex = gameObject.zIndex;

            components_field = new List<PrefabComponent>();

            foreach (var component in gameObject.GetAllComponents())
            {
                components_field.Add(new PrefabComponent(component));
            }
        }
        #endregion

        #region Statics
        /// <summary>
        /// A list of all currently loaded Prefabs.
        /// </summary>
        private static List<Prefab> prefabs = new List<Prefab>();

        /// <summary>
        /// Clear the currently loaded Prefab cache.
        /// </summary>
        public static void ClearPrefabCache()
        {
            prefabs.Clear();
        }

        /// <summary>
        /// Delete the given Prefab from the Prefab cache.
        /// </summary>
        /// <param name="prefab"></param>
        public static void DeletePrefab(Prefab prefab)
        {
            prefabs.Remove(prefab);
        }
        /// <summary>
        /// Delete all Prefabs with the given name from the Prefab cache.
        /// </summary>
        /// <param name="name"></param>
        public static void DeletePrefab(string name)
        {
            prefabs.RemoveAll(n => n.name == name);
        }

        /// <summary>
        /// Load all Prefabs from the prefab path to the Prefab cache.
        /// </summary>
        public static void LoadAllPrefabs()
        {
            if (!Directory.Exists(Settings.Assets.PrefabPath))
                Directory.CreateDirectory(Settings.Assets.PrefabPath);

            DirectoryInfo dirInfo = new DirectoryInfo(Settings.Assets.PrefabPath);

            foreach (FileInfo file in dirInfo.GetFiles())
            {
                if (file.Extension == ".prefab")
                {
                    Prefab prefab = new Prefab(file.FullName);

                    if (prefabs.Any(n => n.name == prefab.name))
                        Debug.LogError("One or more prefabs with the name '{0}' have already been loaded. Skipping prefab at path '{1}'.", prefab.name, file.FullName);
                    else
                        prefabs.Add(prefab);
                }
            }
        }

        /// <summary>
        /// Get the Prefab of the given name from the Prefab cache.
        /// </summary>
        /// <param name="name">The name of the Prefab to return.</param>
        /// <returns>The Prefab of the given name, or null if a Prefab with that name does not exist.</returns>
        public static Prefab GetPrefab(string name)
        {
            return prefabs.FirstOrDefault(n => n.name == name);
        }

        /// <summary>
        /// Checks whether a Prefab with a given name exists.
        /// </summary>
        /// <param name="name">The name of the Prefab to check for.</param>
        /// <returns>True if the Prefab exists, otherwise false.</returns>
        public static bool Exists(string name)
        {
            return prefabs.Any(n => n.name == name);
        }

        /// <summary>
        /// Get all Prefabs in the Prefab cache.
        /// </summary>
        /// <returns>A List of all Prefabs in the Prefab cache.</returns>
        public static List<Prefab> GetAllPrefabs()
        {
            return new List<Prefab>(prefabs);
        }
        #endregion

        /// <summary>
        /// The file path of this Prefab.
        /// </summary>
        public string path { get; set; }
        /// <summary>
        /// The name of the GameObject this Prefab defines.
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// The z-index of the GameObject this Prefab defines.
        /// </summary>
        public int zIndex { get; set; }

        private List<PrefabComponent> components_field;
        /// <summary>
        /// A list of data on all of the components on the GameObject this Prefab defines.
        /// </summary>
        public List<PrefabComponent> components { get { return new List<PrefabComponent>(components_field); } }

        /// <summary>
        /// Returns the name of the GameObject this Prefab defines.
        /// </summary>
        public override string ToString()
        {
            return name;
        }

        /// <summary>
        /// Save this Prefab to the file defined by its path property.
        /// </summary>
        public void SavePrefab()
        {
            SoapFormatter formatter = new SoapFormatter();
            XDocument doc = new XDocument();

            XElement xmlRoot = new XElement("gameobject");
            xmlRoot.Add(new XAttribute("name", name));
            xmlRoot.Add(new XAttribute("zindex", zIndex));

            XElement xmlComponents = new XElement("components");

            foreach (var component in components_field)
            {
                var xmlComponent = new XElement("component");

                xmlComponent.Add(new XAttribute("type", component.type.AssemblyQualifiedName));

                foreach (var field in component.fieldValues)
                {
                    string value;

                    try
                    {
                        using (MemoryStream stream = new MemoryStream())
                        {
                            formatter.Serialize(stream, field.value);

                            byte[] bytes = new byte[stream.Length];

                            stream.Position = 0;
                            stream.Read(bytes, 0, bytes.Length);

                            value = Encoding.Unicode.GetString(bytes);
                        }

                        XElement xmlField = new XElement("field", value);
                        xmlField.Add(new XAttribute("name", field.name));
                        xmlField.Add(new XAttribute("type", field.type.AssemblyQualifiedName));
                        xmlField.Add(new XAttribute("hide", field.hide ? "true" : "false"));
                        xmlComponent.Add(xmlField);
                    }
                    catch { }
                }

                xmlComponents.Add(xmlComponent);
            }

            xmlRoot.Add(xmlComponents);
            doc.Add(xmlRoot);

            using (TextWriter writer = new StreamWriter(path, false, Encoding.Unicode))
            {
                writer.Write(doc.ToString());
            }
        }

        /// <summary>
        /// Add a PrefabComponent to this Prefab.
        /// </summary>
        /// <param name="component"></param>
        public void AddComponent(PrefabComponent component)
        {
            if (!components_field.Contains(component))
                components_field.Add(component);
            else
                Debug.LogError("Component {0} has already been added to GameObject {1}.", component.type, name);
        }
        /// <summary>
        /// Add a PrefabComponent corresponding to a Component of the Type type to this Prefab.
        /// </summary>
        /// <param name="type">The Type of the Component.</param>
        public void AddComponent(Type type)
        {
            if (!(typeof(Component)).IsAssignableFrom(type))
            {
                Debug.LogError("Type {0} is not a component!", type);
                return;
            }
            if (type.IsDefined(typeof(UniqueComponentAttribute), true) && components.Any(n => n.type == type))
            {
                Debug.LogError("Only one component of type {0} is allowed on a GameObject!", type);
                return;
            }

            Component component = (Component)Activator.CreateInstance(type);

            components_field.Add(new PrefabComponent(component));
        }

        /// <summary>
        /// Remove the given PrefabComponent from this Prefab.
        /// </summary>
        /// <param name="component"></param>
        public void RemoveComponent(PrefabComponent component)
        {
            if (components_field.Contains(component))
                components_field.Remove(component);
            else
                Debug.LogError("Component {0} cannot be removed from GameObject {1} because it is not on the GameObject.", component.type, name);
        }

        /// <summary>
        /// Defines data used to serialize and deserialize a Component.
        /// </summary>
        public sealed class PrefabComponent
        {
            /// <summary>
            /// Deserialize a PrefabComponent from the given XML data.
            /// </summary>
            /// <param name="data">The XML data to deserialize from.</param>
            public PrefabComponent(XElement data)
            {
                type = Type.GetType(data.Attribute("type").Value, true);

                if (!type.IsAssignableFrom(typeof(Component)))
                    Debug.LogError("Invalid component type on prefab: '{0}' does not inherit from Component.", type.ToString());

                fieldValues_field = new List<FieldValue>();

                foreach (var field in data.Elements("field"))
                {
                    fieldValues_field.Add(new FieldValue(field));
                }
            }
            /// <summary>
            /// Create PrefabComponent from the given Component.
            /// </summary>
            /// <param name="component"></param>
            public PrefabComponent(Component component)
            {
                type = component.GetType();

                fieldValues_field = new List<FieldValue>();

                foreach (var field in type.GetFields(FindFields))
                {
                    if (field.IsDefined(typeof(SerializeAttribute)) && field.GetType().IsSerializable)
                    {
                        fieldValues_field.Add(new FieldValue(field.Name, field.FieldType, field.GetValue(component)));
                    }
                }
            }

            private List<FieldValue> fieldValues_field;
            /// <summary>
            /// A list of data on all of the fields on the Component defined by this PrefabComponent.
            /// </summary>
            public List<FieldValue> fieldValues { get { return new List<FieldValue>(fieldValues_field); } }

            /// <summary>
            /// The Type of the Component defined by this PrefabComponent.
            /// </summary>
            public Type type { get; private set; }
        }

        /// <summary>
        /// Defines data used to serialize and deserialize a field on a Component.
        /// </summary>
        public sealed class FieldValue
        {
            /// <summary>
            /// Create a new FieldValue.
            /// </summary>
            /// <param name="name">The name of the defined field.</param>
            /// <param name="type">The Type of the defined field.</param>
            /// <param name="value">The value of the defined field.</param>
            /// <param name="hide">Whether to hide the defined field in the inspector.</param>
            public FieldValue(string name, Type type, object value, bool hide = false)
            {
                this.name = name;
                this.type = type;
                this.value = value;
                this.hide = hide;
            }
            /// <summary>
            /// Deserialize a FieldValue from the given XML data.
            /// </summary>
            /// <param name="data">The XML data to deserialize from.</param>
            public FieldValue(XElement data)
            {
                name = data.Attribute("name").Value;
                type = Type.GetType(data.Attribute("type").Value, true);
                if (data.Attribute("hide") != null)
                    hide = data.Attribute("hide").Value == "true";
                using (MemoryStream stream = new MemoryStream(data.Value.GetBytes()))
                    value = new SoapFormatter().Deserialize(stream);
            }

            /// <summary>
            /// The name of the defined field.
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// The Type of the defined field.
            /// </summary>
            public Type type { get; set; }
            /// <summary>
            /// The value of the defined field.
            /// </summary>
            public object value { get; set; }
            /// <summary>
            /// Whether to hide the defined field in the inspector.
            /// </summary>
            public bool hide { get; set; }
        }
    }
}