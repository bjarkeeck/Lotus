using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Reflection;
using SharpGL;
using System.Xml.Linq;

namespace LotusEngine
{
    /// <summary>
    /// Defines a GameObject.
    /// </summary>
    public sealed class GameObject : IComparable<GameObject>
    {
        /// <summary>
        /// Create a new GameObject instance.
        /// </summary>
        private GameObject()
        {
            AddComponent<Transform>();
        }

        #region Properties
        /// <summary>
        /// Whether this GameObject has been destroyed.
        /// </summary>
        public bool isDestroyed { get; private set; }
        /// <summary>
        /// The name of this GameObject.
        /// </summary>
        public string name { get; set; }

        private Renderer renderer_field;
        /// <summary>
        /// The Renderer of this GameObject. Null if there is none, the first returned by the GameObject.GetComponent() method if there are many.
        /// </summary>
        public Renderer renderer
        {
            get
            {
                if (renderer_field == null)
                    renderer_field = GetComponent<Renderer>();

                return renderer_field;
            }
        }

        private Transform transform_field;
        /// <summary>
        /// The Transform of this GameObject.
        /// </summary>
        public Transform transform
        {
            get
            {
                if (transform_field == null)
                    transform_field = GetComponent<Transform>();

                return transform_field;
            }
        }

        private Collider collider_field;
        /// <summary>
        /// The Collider of this GameObject. Null if there is none, the first returned by the GameObject.GetComponent() method if there are many.
        /// </summary>
        public Collider collider
        {
            get
            {
                if (collider_field == null)
                    collider_field = GetComponent<Collider>();

                return collider_field;
            }
        }

        private int zIndex_field;
        /// <summary>
        /// The z-index of this GameObject. The z-index defines the draw order of objects - objects with higher z-indexes will always be drawn later.
        /// </summary>
        public int zIndex
        {
            get
            {
                return zIndex_field;
            }
            set
            {
                zIndex_field = value;

                Scene.ActiveScene.SortSceneObjects();
            }
        }

        /// <summary>
        /// Whether this GameObject is a Prefab.
        /// </summary>
        public bool isPrefab { get; private set; }
        /// <summary>
        /// Whether this GameObject was created from a Prefab.
        /// </summary>
        public bool fromPrefab { get; private set; }
        /// <summary>
        /// The name of the Prefab this GameObject was created from.
        /// </summary>
        public string prefabName { get; private set; }
        /// <summary>
        /// The Scene which this GameObject belongs to.
        /// </summary>
        internal Scene homeScene { get; private set; }
        /// <summary>
        /// Whether this GameObject will be destroyed when a new Scene is loaded.
        /// </summary>
        internal bool dontDestroyOnLoad { get; set; }
        #endregion

        #region Fields
        /// <summary>
        /// All of the Components on this GameObject.
        /// </summary>
        private List<Component> components = new List<Component>();
        /// <summary>
        /// Whether Start() has been called yet on this GameObject.
        /// </summary>
        private bool startCalled = false;
        #endregion

        #region Static Functions
        /// <summary>
        /// Instantiates a new GameObject called "Gameobject" at position (0,0) with scale (1,1) and rotation 0.
        /// </summary>
        /// <returns>The instantiated GameObject.</returns>
        public static GameObject Instantiate()
        {
            return Instantiate("Gameobject", Vector2.zero, 0);
        }

        /// <summary>
        /// Instantiates a new GameObject with the given name at position (0,0) with scale (1,1) and rotation 0.
        /// </summary>
        /// <param name="name">The name of the GameObject to be instantiated.</param>
        /// <returns>The instantiated GameObject.</returns>
        public static GameObject Instantiate(string name)
        {
            return Instantiate(name, Vector2.zero, 0);
        }

        /// <summary>
        /// Instantiates a new GameObject with the given name at the given position with scale (1,1) and rotation 0.
        /// </summary>
        /// <param name="name">The name of the GameObject to be instantiated.</param>
        /// <param name="position">The position of the GameObject to be instantiated.</param>
        /// <returns>The instantiated GameObject.</returns>
        public static GameObject Instantiate(string name, Vector2 position)
        {
            return Instantiate(name, position, 0);
        }

        /// <summary>
        /// Instantiates a new GameObject with the given name at the given position with scale (1,1) and the given rotation.
        /// </summary>
        /// <param name="name">The name of the GameObject to be instantiated.</param>
        /// <param name="position">The position of the GameObject to be instantiated.</param>
        /// <param name="rotation">The rotation of the GameObject to be instantiated.</param>
        /// <returns>The instantiated GameObject.</returns>
        public static GameObject Instantiate(string name, Vector2 position, float rotation)
        {
            GameObject go = new GameObject();

            go.name = name;
            go.transform.position = position;
            go.transform.scale = Vector2.one;
            go.transform.rotation = rotation;
            go.homeScene = Scene.AddObject(go);
            go.Start();

            return go;
        }

        /// <summary>
        /// Instantiates a new GameObject from the given Prefab at position (0,0) with scale (1,1) and rotation 0.
        /// </summary>
        /// <param name="prefab">The Prefab which contains the data to instantiate the GameObject from.</param>
        /// <returns>The instantiated GameObject.</returns>
        public static GameObject Instantiate(Prefab prefab)
        {
            return Instantiate(prefab, Vector2.zero);
        }

        /// <summary>
        /// Instantiates a new GameObject from the given Prefab at the given position with scale (1,1) and rotation 0.
        /// </summary>
        /// <param name="prefab">The Prefab which contains the data to instantiate the GameObject from.</param>
        /// <param name="position">The position of the GameObject to be instantiated.</param>
        /// <returns>The instantiated GameObject.</returns>
        public static GameObject Instantiate(Prefab prefab, Vector2 position)
        {
            return Instantiate(prefab, position, 0);
        }

        public static GameObject Instantiate(Prefab prefab, Vector2 position, float rotation)
        {
            GameObject go = new GameObject();

            go.fromPrefab = true;
            go.prefabName = prefab.name;
            go.name = prefab.name;
            go.transform.position = position;
            go.transform.rotation = rotation;
            go.zIndex = prefab.zIndex;
            go.homeScene = Scene.AddObject(go);

            foreach (var prefabComponent in prefab.components)
            {
                //try
                //{
                if (prefabComponent.type == typeof(Transform))
                {
                    go.transform.scale = (Vector2)prefabComponent.fieldValues.Find(n => n.name == "_scale").value;
                }
                else
                {
                    Component component = go.AddComponent(prefabComponent.type);

                    foreach (var fieldValue in prefabComponent.fieldValues)
                    {
                        FieldInfo field = prefabComponent.type.GetField(fieldValue.name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        if (field != null)
                            field.SetValue(component, fieldValue.value);
                    }
                }
                //}
                //catch { }
            }

            go.Start();

            return go;
        }

        /// <summary>
        /// Instantiates a new GameObject from the given SceneGameObjectData at position (0,0) with scale (1,1) and rotation 0.
        /// </summary>
        /// <param name="objectData">The SceneGameObjectData which contains the data to instantiate the GameObject from.</param>
        /// <returns>The instantiated GameObject.</returns>
        internal static GameObject Instantiate(SceneData.SceneGameObjectData objectData)
        {
            GameObject go = new GameObject();
            var transform = objectData.components.Find(n => n.type == typeof(Transform));

            if (objectData.isPrefab)
            {
                //Prefab prefab = Prefab.GetPrefab(objectData.prefabName);

                //if (prefab == null)
                //{
                //    Debug.LogError("Tried to load prefab '{0}' but it did not exist!");
                //    return null;
                //}

                //go = Instantiate(prefab);
                go.fromPrefab = true;
                go.prefabName = objectData.prefabName;
            }
            //else
            //{
            //    go = new GameObject();
            //}

            go.name = objectData.name;
            go.zIndex = objectData.zIndex;
            go.homeScene = Scene.AddObject(go);

            foreach (var componentData in objectData.components)
            {
                //try
                //{
                Component component;

                if (componentData.type == typeof(Transform))
                    component = go.transform;
                else
                    component = go.AddComponent(componentData.type);

                foreach (var fieldData in componentData.fields)
                {
                    FieldInfo field = componentData.type.GetField(fieldData.Key, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (field != null)
                        field.SetValue(component, fieldData.Value);
                }
                //}
                //catch { }
            }

            go.Start();

            return go;
        }

        /// <summary>
        /// Finds a GameObject with the given name.
        /// </summary>
        /// <param name="name">The name of the GameObject to find.</param>
        /// <returns>The found GameObject, null if no GameObject was found.</returns>
        public static GameObject Find(string name)
        {
            return Scene.ActiveScene.sceneObjects.FirstOrDefault(n => n.name == name);
        }

        /// <summary>
        /// Finds all GameObjects with the given name.
        /// </summary>
        /// <param name="name">The name of the GameObjects to find.</param>
        /// <returns>A list of the found GameObjects, an empty list if no GameObject was found.</returns>
        public static List<GameObject> FindAll(string name)
        {
            return Scene.ActiveScene.sceneObjects.FindAll(n => n.name == name);
        }

        /// <summary>
        /// Find all Components in the scene of Type T or that inherits from Type T.
        /// </summary>
        /// <typeparam name="T">The Type of Components to find.</typeparam>
        /// <returns>A list of all the found Components.</returns>
        public static List<T> FindAllComponents<T>() where T : Component
        {
            List<T> result = new List<T>();

            foreach (var go in Scene.ActiveScene.sceneObjects)
                foreach (var component in go.GetAllComponents())
                    if (typeof(T).IsAssignableFrom(component.GetType()))
                        result.Add((T)component);

            return result;
        }

        /// <summary>
        /// Find all Components in the scene of Type T or that inherits from Type T.
        /// </summary>
        /// <typeparam name="T">The Type of Components to find.</typeparam>
        /// <returns>A list of all the found Components.</returns>
        public static IEnumerable<T> FindAllComponents<T>(Func<T, bool> predicate) where T : Component
        {
            foreach (GameObject go in Scene.ActiveScene.sceneObjects)
                foreach (Component component in go.GetAllComponents())
                    if (typeof(T).IsAssignableFrom(component.GetType()))
                        if (predicate.Invoke((T)component))
                            yield return (T)component;
        }

        #endregion

        /// <summary>
        /// Returns the name of the GameObject.
        /// </summary>
        public override string ToString()
        {
            return name;
        }

        /// <summary>
        /// Compares this GameObject to another GameObject by their z-index.
        /// </summary>
        /// <param name="other">The GameObject to compare to.</param>
        /// <returns>1 if other is null, otherwise the result of comparing the two z-indices with one another.</returns>
        public int CompareTo(GameObject other)
        {
            if (other == null)
                return 1;

            return zIndex.CompareTo(other.zIndex);
        }

        /// <summary>
        /// Resets the GameObject to its original prefab data.
        /// This is done by Destroying this GameObject and instantiating a new Prefab in the same position.
        /// </summary>
        /// <returns>The newly instantiated Prefab if the Prefab exists, otherwise the unchanged GameObject.</returns>
        public GameObject ResetToPrefab()
        {
            if (fromPrefab && Prefab.Exists(prefabName))
            {
                GameObject result = Instantiate(Prefab.GetPrefab(prefabName), transform.position, transform.rotation);
                result.transform.scale = transform.scale;
                result.name = name;

                Destroy();
                return result;
            }

            return this;
        }

        /// <summary>
        /// Tell the engine not to Destroy this GameObject when a new scene is loaded.
        /// </summary>
        public void DontDestroyOnLoad()
        {
            dontDestroyOnLoad = true;
        }

        /// <summary>
        /// Tell the engine to Destroy this GameObject when a new scene is loaded. This is the default.
        /// </summary>
        public void DestroyOnLoad()
        {
            dontDestroyOnLoad = false;
        }

        /// <summary>
        /// Add a Component to the GameObject.
        /// </summary>
        /// <param name="type">The Type of the Component to add.</param>
        /// <returns>The added Component.</returns>
        public Component AddComponent(Type type)
        {
            if (!(typeof(Component)).IsAssignableFrom(type))
            {
                Debug.LogError("Type {0} is not a component!", type);
                return null;
            }
            if (type.IsDefined(typeof(UniqueComponentAttribute), true) && HasComponent(type))
            {
                Debug.LogError("Only one component of type {0} is allowed on a GameObject!", type);
                return null;
            }

            try
            {
                Component component = (Component)Activator.CreateInstance(type);

                components.Add(component);
                component.gameObject = this;
                if (startCalled)
                    component.Start();

                return component;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Add a Component to the GameObject.
        /// </summary>
        /// <typeparam name="T">The Type of the Component to add.</typeparam>
        /// <returns>The added Component.</returns>
        public T AddComponent<T>() where T : Component, new()
        {
            if (typeof(T).IsDefined(typeof(UniqueComponentAttribute), true) && HasComponent<T>())
            {
                Debug.LogError("Only one component of type {0} is allowed on a GameObject!", typeof(T));
                return null;
            }

            T component = new T();

            components.Add(component);
            component.gameObject = this;
            if (startCalled)
                component.Start();

            return component;
        }

        /// <summary>
        /// Checks whether the GameObject has a Component.
        /// </summary>
        /// <param name="type">The Type of Component to check for.</param>
        /// <returns>True if the GameObject has a Component of the given Type, otherwise false.</returns>
        public bool HasComponent(Type type)
        {
            return components.Any(n => type.IsAssignableFrom(n.GetType()));
        }
        /// <summary>
        /// Checks whether the GameObject has a Component.
        /// </summary>
        /// <typeparam name="T">The Type of Component to check for.</typeparam>
        /// <returns>True if the GameObject has a Component of the given Type, otherwise false.</returns>
        public bool HasComponent<T>() where T : Component
        {
            return components.Any(n => typeof(T).IsAssignableFrom(n.GetType()));
        }

        /// <summary>
        /// Gets a Component on the GameObject.
        /// </summary>
        /// <param name="index">The index of the Component to get.</param>
        /// <returns>The Component at the given index, null if the index is invalid.</returns>
        public Component GetComponent(int index)
        {
            if (index < 0 || index >= components.Count)
            {
                Debug.LogError("Gameobject does not have a component with the index '{0}'", index);
                return null;
            }

            return components[index];
        }
        /// <summary>
        /// Gets a Component on the GameObject.
        /// </summary>
        /// <param name="type">The Type of the Component to get.</param>
        /// <returns>The first found Component. Null if there is no Component of the given Type.</returns>
        public Component GetComponent(Type type)
        {
            return components.FirstOrDefault(n => type.IsAssignableFrom(n.GetType()));
        }
        /// <summary>
        /// Gets a Component on the GameObject.
        /// </summary>
        /// <typeparam name="T">The Type of the Component to get.</typeparam>
        /// <returns>The first found Component. Null if there is no Component of the given Type.</returns>
        public T GetComponent<T>() where T : Component
        {
            return (T)components.FirstOrDefault(n => typeof(T).IsAssignableFrom(n.GetType()));
        }

        /// <summary>
        /// Gets all Components on the GameObject.
        /// </summary>
        /// <returns>A list of all Components on the GameObject.</returns>
        public List<Component> GetAllComponents()
        {
            return new List<Component>(components);
        }
        /// <summary>
        /// Gets all Components of a given Type on the GameObject.
        /// </summary>
        /// <param name="type">The Type of the Components to get.</param>
        /// <returns>A list of all the found Components.</returns>
        public List<Component> GetAllComponents(Type type)
        {
            return components.FindAll(n => type.IsAssignableFrom(n.GetType()));
        }
        /// <summary>
        /// Gets all Components of a given Type on the GameObject.
        /// </summary>
        /// <typeparam name="T">The Type of the Components to get.</typeparam>
        /// <returns>A list of all the found Components.</returns>
        public List<T> GetAllComponents<T>() where T : Component
        {
            return components.FindAll(n => typeof(T).IsAssignableFrom(n.GetType())).ConvertAll(n => (T)n);
        }

        /// <summary>
        /// Destroy this GameObject. This also destroys all Components on this GameObject.
        /// </summary>
        public void Destroy()
        {
            if (!isDestroyed)
            {
                while (components.Count > 0)
                {
                    if (components[0] is Transform)
                    {
                        components[0].gameObject = null;
                        components.RemoveAt(0);
                    }
                    else
                        components[0].Destroy();
                }

                Scene.RemoveObject(this);
                this.isDestroyed = true;
            }
            else
                Debug.LogError("This GameObject has already been destroyed!");
        }

        /// <summary>
        /// Calls the Start() method on all Components on this GameObject.
        /// </summary>
        internal void Start()
        {
            if (!isDestroyed && !startCalled && !Settings.Editor.EditorIsRunning)
            {
                startCalled = true;

                var temp = new List<Component>(components);

                temp.ForEach(n => { if (!n.isDestroyed) n.Start(); });
            }
        }

        /// <summary>
        /// Calls the Update() method on all Components on this GameObject.
        /// </summary>
        internal void Update()
        {
            if (!isDestroyed)
            {
                var temp = new List<Component>(components);

                temp.ForEach(n => { if (!n.isDestroyed) n.Update(); });
            }
        }

        /// <summary>
        /// Calls the Draw() method on all Components on this GameObject.
        /// </summary>
        internal void Draw()
        {
            if (!isDestroyed)
            {
                //View view = Scene.ActiveScene.renderingView;

                //float left = transform.position.x - view.worldX,
                //      top = transform.position.y - view.worldY,
                //      xScale = (view.width / Settings.Screen.Width) * (Settings.Screen.Width / view.worldWidth),
                //      yScale = (view.height / Settings.Screen.Height) * (Settings.Screen.Height / view.worldHeight);

                var temp = new List<Component>(components);

                foreach (var c in temp)
                {
                    if (c.isDestroyed)
                        continue;

                    View view = Scene.ActiveScene.renderingView;

                    float xScale = (view.width / Settings.Screen.Width) * (Settings.Screen.Width / view.worldWidth),
                          yScale = (view.height / Settings.Screen.Height) * (Settings.Screen.Height / view.worldHeight);

                    OpenGL gl = Rendering.gl;

                    gl.LoadIdentity();
                    gl.Ortho(0, Settings.Screen.Width, Settings.Screen.Height, 0, 0, 1);
                    gl.Translate(view.screenX, view.screenY, 0);
                    gl.Scale(xScale, yScale, 1);

                    //OpenGL gl = Rendering.gl;

                    //gl.LoadIdentity();
                    //gl.Translate(view.screenX, view.screenY, 0);
                    //gl.Scale(xScale, yScale, 1);
                    //gl.Translate(left, top, 0);
                    //gl.Rotate(0, 0, transform.rotation);

                    //Rendering.screen.ResetTransform();
                    //Rendering.TranslateTransform(view.screenX, view.screenY);
                    //Rendering.screen.ScaleTransform(xScale, yScale);
                    //Rendering.TranslateTransform(left, top);
                    //Rendering.RotateTransform(transform.rotation);

                    c.Draw();
                }
            }
        }

        /// <summary>
        /// Calls the DrawGUI() method on all Components on this GameObject.
        /// </summary>
        internal void DrawGUI(Graphics screen)
        {
            if (!isDestroyed)
            {
                var temp = new List<Component>(components);

                foreach (var c in temp)
                {
                    if (c.isDestroyed)
                        continue;

                    screen.ResetTransform();
                    c.DrawGUI();
                }
            }
        }

        /// <summary>
        /// Calls the OnCollisionEnter() method on all Components on this GameObject.
        /// </summary>
        internal void OnCollisionEnter(Collider other)
        {
            if (!isDestroyed)
            {
                var temp = new List<Component>(components);

                temp.ForEach(n => { if (!n.isDestroyed) n.OnCollisionEnter(other); });
            }
        }

        /// <summary>
        /// Calls the OnCollisionStay() method on all Components on this GameObject.
        /// </summary>
        internal void OnCollisionStay(Collider other)
        {
            if (!isDestroyed)
            {
                var temp = new List<Component>(components);

                temp.ForEach(n => { if (!n.isDestroyed) n.OnCollisionStay(other); });
            }
        }

        /// <summary>
        /// Calls the OnCollisionExit() method on all Components on this GameObject.
        /// </summary>
        internal void OnCollisionExit(Collider other)
        {
            if (!isDestroyed)
            {
                var temp = new List<Component>(components);

                temp.ForEach(n => { if (!n.isDestroyed) n.OnCollisionExit(other); });
            }
        }

        /// <summary>
        /// Calls the CheckForCollisions() method on all Colliders on this GameObject.
        /// </summary>
        public void CheckForCollisions()
        {
            if (!isDestroyed)
                foreach (var collider in GetAllComponents<Collider>())
                    collider.CollisionCheck();
        }

        /// <summary>
        /// Removes a Component from this GameObject.
        /// </summary>
        /// <param name="component">The Component to remove.</param>
        internal void RemoveComponent(Component component)
        {
            if (isDestroyed)
            {
                Debug.LogError("GameObject {0} has been destroyed and should not be accessed.", name);
                return;
            }
            if (component.GetType() == typeof(Transform))
            {
                Debug.LogError("You can't destroy a Transform.", name);
                return;
            }

            components.Remove(component);
        }
    }
}