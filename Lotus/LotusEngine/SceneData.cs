using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
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
    /// Defines data used to serialize and deserialize a Scene.
    /// </summary>
    public class SceneData
    {
        /// <summary>
        /// Creates a SceneData from a Scene. 
        /// </summary>
        /// <param name="scene">The Scene to create a SceneData from.</param>
        public SceneData(Scene scene)
        {
            path = scene.path;
            name = scene.name;
            bgColor = scene.bgColor;
            views_field = new List<View>(scene.views);
            sceneObjects_field = new List<SceneGameObjectData>(scene.sceneObjects.ConvertAll<SceneGameObjectData>(n => new SceneGameObjectData(n)));
        }
        /// <summary>
        /// Loads a SceneData from a file.
        /// </summary>
        /// <param name="name">The name of the Scene.</param>
        /// <param name="fullpath">If true, name is taken as being the full path to the scene file.</param>
        public SceneData(string name, bool fullpath = false)
        {
            path = fullpath ? name : Settings.Assets.ScenePath + "\\" + name + ".scene";

            if (!File.Exists(path))
                throw new ArgumentException("The path to the scene " + name + " does not exist: " + path);

            SoapFormatter formatter = new SoapFormatter();
            XDocument doc = XDocument.Load(path);
            XElement root = doc.Element("scene");

            this.name = root.Attribute("name").Value;

            using (MemoryStream stream = new MemoryStream(root.Element("bgcolor").Value.GetBytes()))
                bgColor = (Color)formatter.Deserialize(stream);

            views_field = new List<View>();

            foreach (var element in root.Element("views").Elements("view"))
            {
                using (MemoryStream stream = new MemoryStream(element.Value.GetBytes()))
                {
                    try
                    {
                        views_field.Add((View)formatter.Deserialize(stream));
                    }
                    catch
                    {
                        views_field.Add(new View());
                    }
                }
            }

            sceneObjects_field = new List<SceneGameObjectData>();

            foreach (var element in root.Element("gameobjects").Elements("gameobject"))
                sceneObjects_field.Add(new SceneGameObjectData(element));
        }

        /// <summary>
        /// The Scene's file path.
        /// </summary>
        public string path { get; private set; }
        /// <summary>
        /// The name of the Scene.
        /// </summary>
        public string name { get; private set; }
        /// <summary>
        /// The background color of the Scene.
        /// </summary>
        public Color bgColor { get; private set; }

        public List<View> views_field;
        /// <summary>
        /// All of the views in this Scene.
        /// </summary>
        public List<View> views { get { return new List<View>(views_field); } }

        private List<SceneGameObjectData> sceneObjects_field;
        /// <summary>
        /// All of the GameObjects in this Scene.
        /// </summary>
        public List<SceneGameObjectData> sceneObjects { get { return new List<SceneGameObjectData>(sceneObjects_field); } }

        /// <summary>
        /// Saves the Scene to its path.
        /// </summary>
        public void SaveSceneData()
        {
            SoapFormatter formatter = new SoapFormatter();
            XDocument doc = new XDocument();

            XElement xmlRoot = new XElement("scene"),
                     xmlBgColor = new XElement("bgcolor"),
                     xmlGameobjects = new XElement("gameobjects"),
                     xmlViews = new XElement("views");

            xmlRoot.Add(new XAttribute("name", name));
            xmlRoot.Add(xmlBgColor);
            xmlRoot.Add(xmlViews);
            xmlRoot.Add(xmlGameobjects);
            doc.Add(xmlRoot);

            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, bgColor);

                byte[] bytes = new byte[stream.Length];

                stream.Position = 0;
                stream.Read(bytes, 0, bytes.Length);

                xmlBgColor.Value = Encoding.Unicode.GetString(bytes);
            }

            foreach (var view in views)
            {
                var xmlView = new XElement("view");
                xmlViews.Add(xmlView);

                using (MemoryStream stream = new MemoryStream())
                {
                    formatter.Serialize(stream, view);

                    byte[] bytes = new byte[stream.Length];

                    stream.Position = 0;
                    stream.Read(bytes, 0, bytes.Length);

                    xmlView.Value = Encoding.Unicode.GetString(bytes);
                }
            }

            foreach (var gameobject in sceneObjects_field)
            {
                XElement xmlGameobject = new XElement("gameobject"),
                         xmlComponents = new XElement("components");

                xmlGameobject.Add(new XAttribute("name", gameobject.name));
                xmlGameobject.Add(new XAttribute("zindex", gameobject.zIndex));

                if (gameobject.isPrefab)
                    xmlGameobject.Add(new XAttribute("prefab", gameobject.prefabName));

                xmlGameobjects.Add(xmlGameobject);
                xmlGameobject.Add(xmlComponents);

                foreach (var component in gameobject.components)
                {
                    var xmlComponent = new XElement("component");
                    xmlComponents.Add(xmlComponent);
                    xmlComponent.Add(new XAttribute("type", component.type.AssemblyQualifiedName));

                    foreach (var field in component.fields)
                    {
                        try
                        {
                            var xmlField = new XElement("field");

                            xmlField.Add(new XAttribute("name", field.Key));

                            using (MemoryStream stream = new MemoryStream())
                            {
                                formatter.Serialize(stream, field.Value);

                                byte[] bytes = new byte[stream.Length];

                                stream.Position = 0;
                                stream.Read(bytes, 0, bytes.Length);

                                xmlField.Value = Encoding.Unicode.GetString(bytes);
                            }

                            xmlComponent.Add(xmlField);
                        }
                        catch { }
                    }
                }
            }

            using (TextWriter writer = new StreamWriter(path, false, Encoding.Unicode))
            {
                writer.Write(doc.ToString());
            }
        }

        /// <summary>
        /// Defines data to serialize and deserialize a GameObject in a scene.
        /// </summary>
        public class SceneGameObjectData
        {
            /// <summary>
            /// Deserializes a SceneGameObjectData from XML data.
            /// </summary>
            /// <param name="data">The XML data to use.h</param>
            public SceneGameObjectData(XElement data)
            {
                if (data.Attribute("prefab") != null)
                {
                    isPrefab = true;
                    prefabName = data.Attribute("prefab").Value;
                }
                else
                {
                    isPrefab = false;
                    prefabName = null;
                }

                name = data.Attribute("name").Value;
                zIndex = int.Parse(data.Attribute("zindex").Value);
                components_field = new List<SceneComponentData>();

                foreach (var element in data.Element("components").Elements("component"))
                    components_field.Add(new SceneComponentData(element));
            }
            /// <summary>
            /// Creates a SceneGameObjectData from a GameObject.
            /// </summary>
            /// <param name="gameObject">The GameObject to use.</param>
            public SceneGameObjectData(GameObject gameObject)
            {
                if (gameObject.fromPrefab)
                {
                    isPrefab = true;
                    prefabName = gameObject.prefabName;
                }
                else
                {
                    isPrefab = false;
                    prefabName = null;
                }

                name = gameObject.name;
                zIndex = gameObject.zIndex;
                components_field = new List<SceneComponentData>();

                // THIS OUTCOMMENTED CODE IS WHERE, NORMALLY, IT WOULD ONLY SAVE THE VARIABLES CHANGED FROM THE ORIGINAL PREFAB.
                // INSTEAD, IT JUST SAVES ALL OF THEM BECAUSE WE'RE LAZY TODAY AND IT'S... COMPLICATED.

                //if (isPrefab)
                //{
                //    Prefab prefab = Prefab.GetPrefab("name");
                //    SoapFormatter formatter = new SoapFormatter();

                //    foreach (var component in gameObject.GetAllComponents())
                //    {
                //        if (prefab.components.Any
                //    }
                //}
                //else
                //{
                    foreach (var component in gameObject.GetAllComponents())
                        components_field.Add(new SceneComponentData(component));
                //}
            }
            /// <summary>
            /// Create a SceneGameObjectData from a Prefab.
            /// </summary>
            /// <param name="prefab">The Prefab to use.</param>
            public SceneGameObjectData(Prefab prefab)
            {
                isPrefab = true;
                prefabName = prefab.name;

                name = prefab.name;
                zIndex = prefab.zIndex;
                components_field = new List<SceneComponentData>();

                foreach (var component in prefab.components)
                    components_field.Add(new SceneComponentData(component));
            }

            /// <summary>
            /// Whether the defined GameObject has been created from a Prefab.
            /// </summary>
            public bool isPrefab { get; private set; }
            /// <summary>
            /// The name of the Prefab the defined GameObject has been created from.
            /// </summary>
            public string prefabName { get; private set; }
            /// <summary>
            /// The name of the defined GameObject.
            /// </summary>
            public string name { get; private set; }
            /// <summary>
            /// The z-index of the defined GameObject.
            /// </summary>
            public int zIndex { get; private set; }

            private List<SceneComponentData> components_field;
            /// <summary>
            /// A list containing data on all Components on the defined GameObject.
            /// </summary>
            public List<SceneComponentData> components { get { return new List<SceneComponentData>(components_field); } }

            /// <summary>
            /// Defines data to serialize and deserialize a Component on a GameObject.
            /// </summary>
            public class SceneComponentData
            {
                /// <summary>
                /// Deserializes a SceneComponentData from XML data.
                /// </summary>
                /// <param name="data">The XML data to use.</param>
                public SceneComponentData(XElement data)
                {
                    SoapFormatter formatter = new SoapFormatter();
                    fields_field = new Dictionary<string, object>();
                    type = Type.GetType(data.Attribute("type").Value, true);

                    foreach (var element in data.Elements("field"))
                        using (MemoryStream stream = new MemoryStream(element.Value.GetBytes()))
                            fields_field.Add(element.Attribute("name").Value, formatter.Deserialize(stream));
                }
                /// <summary>
                /// Creates a SceneComponentData from a Component.
                /// </summary>
                /// <param name="component">The Component to use.</param>
                public SceneComponentData(Component component)
                {
                    type = component.GetType();

                    fields_field = new Dictionary<string, object>();

                    foreach (var fieldInfo in type.GetFields(Prefab.FindFields))
                        if (fieldInfo.GetValue(component) != null && fieldInfo.IsDefined(typeof(SerializeAttribute)))
                            fields_field.Add(fieldInfo.Name, fieldInfo.GetValue(component));
                }
                //public SceneComponentData(Component component, List<FieldInfo> fields)
                //{
                //    type = component.GetType();

                //    fields_field = new Dictionary<string, object>();

                //    foreach (var fieldInfo in fields)//type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                //        if (fieldInfo.GetValue(component) != null && !fieldInfo.IsDefined(typeof(DontSerializeAttribute)))
                //            fields_field.Add(fieldInfo.Name, fieldInfo.GetValue(component));
                //}
                /// <summary>
                /// Creates a SceneComponentData from a PrefabComponent.
                /// </summary>
                /// <param name="component">The PrefabComponent to use.</param>
                public SceneComponentData(Prefab.PrefabComponent component)
                {
                    type = component.type;

                    fields_field = new Dictionary<string, object>();

                    foreach (var field in component.fieldValues)
                        if (field.value != null)
                            fields_field.Add(field.name, field.value);
                }

                public Type type { get; private set; }

                private Dictionary<string, object> fields_field;
                public Dictionary<string, object> fields { get { return new Dictionary<string, object>(fields_field); } }
            }
        }
    }
}