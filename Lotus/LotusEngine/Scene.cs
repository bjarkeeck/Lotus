using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace LotusEngine
{
    /// <summary>
    /// Defines a Scene.
    /// </summary>
    public sealed class Scene
    {
        /// <summary>
        /// Creates an empty Scene.
        /// </summary>
        /// <param name="name">The name of the Scene to create.</param>
        public Scene(string name)
        {
            activeScene = this;
            this.name = name;
            path = Settings.Assets.ScenePath + "\\" + name;

            if (!path.ToLower().EndsWith(".scene"))
                path += ".scene";

            views_field = new List<View>();
            views_field.Add(new View(0, 0, Settings.Screen.Width, Settings.Screen.Height, 0, 0, Settings.Screen.Width, Settings.Screen.Height));

            bgColor = Color.Black;
        }
        /// <summary>
        /// Creates a Scene using the given SceneData.
        /// </summary>
        /// <param name="data">The SceneData to create the Scene from.</param>
        public Scene(SceneData data)
        {
            activeScene = this;
            path = data.path;
            name = data.name;            

            //views_field = new List<View>(data.views);
            //bgColor = data.bgColor;

            //foreach (var obj in data.sceneObjects)
            //{
            //    GameObject.Instantiate(obj);
            //}
        }

        private List<GameObject> sceneObjects_field = new List<GameObject>();
        /// <summary>
        /// All of the GameObjects in the Scene.
        /// </summary>
        public List<GameObject> sceneObjects { get { return new List<GameObject>(sceneObjects_field); } }

        private List<View> views_field;
        /// <summary>
        /// All of the Views that the Scene contains.
        /// </summary>
        public List<View> views { get { return new List<View>(views_field); } }

        /// <summary>
        /// The file path of the Scene.
        /// </summary>
        public string path { get; set; }
        /// <summary>
        /// The name of the Scene.
        /// </summary>
        public string name { get; private set; }
        /// <summary>
        /// The background color of the Scene.
        /// </summary>
        public Color bgColor { get; set; }
        /// <summary>
        /// The currently rendering View.
        /// </summary>
        public View renderingView { get; private set; }
        /// <summary>
        /// The index of the currently rendering View.
        /// </summary>
        public int renderingViewIndex { get { if (views_field.Contains(renderingView)) return views_field.IndexOf(renderingView); else return -1; } }
        
        private static Scene activeScene;
        /// <summary>
        /// The Scene which is currently loaded and active.
        /// </summary>
        public static Scene ActiveScene
        {
            get
            {
                return activeScene;
            }
        }

        /// <summary>
        /// The scene to load at the end of the frame.
        /// </summary>
        private static SceneData sceneToLoad { get; set; }

        private static List<SceneData> scenes;
        /// <summary>
        /// A list of data on all Scenes.
        /// </summary>
        private static List<SceneData> Scenes { get { return new List<SceneData>(scenes); } }

        /// <summary>
        /// Sort scene objects according to z-index.
        /// </summary>
        internal void SortSceneObjects()
        {
            ActiveScene.sceneObjects_field.Sort();
        }

        /// <summary>
        /// Load all Scenes in a folder to the Scene cache.
        /// </summary>
        /// <param name="path">The path of the folder.</param>
        public static void LoadAllScenes()
        {
            scenes = new List<SceneData>();

            if (!Directory.Exists(Settings.Assets.ScenePath))
                Directory.CreateDirectory(Settings.Assets.ScenePath);

            DirectoryInfo dirInfo = new DirectoryInfo(Settings.Assets.ScenePath);

            foreach (FileInfo file in dirInfo.GetFiles())
            {
                if (file.Extension == ".scene")
                {
                    SceneData sceneData = new SceneData(file);

                    if (scenes.Any(n => n.name == sceneData.name))
                        Debug.LogError("One or more scenes with the name '{0}' have already been loaded. Skipping scene at path '{1}'.", sceneData.name, file.FullName);
                    else
                        scenes.Add(sceneData);
                }
            }

            // TODO Remove this hack!
            if (scenes.Count == 0)
                new Scene("test");
        }

        /// <summary>
        /// Checks if a Scene exists.
        /// </summary>
        /// <param name="name">The name of the Scene to check for.</param>
        /// <returns>True if the Scene exists, otherwise false.</returns>
        public static bool Exists(string name)
        {
            return scenes.Any(n => n.name == name);
        }

        /// <summary>
        /// Tells the engine to load a Scene.
        /// </summary>
        /// <param name="index">The index of the Scene.</param>
        public static void LoadScene(int index)
        {
            if (index < 0 || index >= scenes.Count)
            {
                Debug.LogError("Scene could not be loaded because a scene with the index '{0}' does not exist!", index);
                return;
            }

            sceneToLoad = scenes[index];
        }
        /// <summary>
        /// Tells the engine to load a Scene.
        /// </summary>
        /// <param name="index">The name of the Scene.</param>
        public static void LoadScene(string name)
        {
            if (!scenes.Any(n => n.name == name))
            {
                Debug.LogError("Scene could not be loaded because the scene '{0}' does not exist!", name);
                return;
            }

            sceneToLoad = scenes.Find(n => n.name == name);
        }
        /// <summary>
        /// Tells the engine to load a Scene.
        /// </summary>
        /// <param name="index">The SceneData defining the Scene.</param>
        public static void LoadScene(SceneData scene)
        {
            sceneToLoad = scene;
        }

        /// <summary>
        /// Unload the current scene and Destroy all GameObjects therein.
        /// </summary>
        private static void UnloadCurrentScene()
        {
            if (activeScene != null)
            {
                foreach (var go in activeScene.sceneObjects)
                    if (!go.dontDestroyOnLoad)
                        go.Destroy();
            }
        }

        /// <summary>
        /// Add a GameObject to the Scene.
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        internal static Scene AddObject(GameObject go)
        {
            ActiveScene.sceneObjects_field.Add(go);
            ActiveScene.sceneObjects_field.Sort();
            return activeScene;
        }

        /// <summary>
        /// Remove a GameObject from the Scene.
        /// </summary>
        /// <param name="go"></param>
        internal static void RemoveObject(GameObject go)
        {
            go.homeScene.sceneObjects_field.Remove(go);
        }

        /// <summary>
        /// Add a View to the Scene.
        /// </summary>
        /// <param name="view"></param>
        public void AddView(View view)
        {
            if (view != null)
                views_field.Add(view);
        }

        /// <summary>
        /// Remove a View from the Scene.
        /// </summary>
        /// <param name="view"></param>
        public void RemoveView(View view)
        {
            if (views_field.Contains(view))
                views_field.Remove(view);
        }

        /// <summary>
        /// Set the Scene's currently rendering View.
        /// </summary>
        /// <param name="view">The View to change to.</param>
        internal void SetCurrentView(View view)
        {
            renderingView = view;
        }
        /// <summary>
        /// Set the Scene's currently rendering View.
        /// </summary>
        /// <param name="index">The index of the View to change to.</param>
        internal void SetCurrentView(int index)
        {
            if (index >= 0 && index < views_field.Count)
                renderingView = views_field[index];
        }

        /// <summary>
        /// Tell the Scene class that a new frame has begun.
        /// </summary>
        internal static void NewFrameBegins()
        {
            if (sceneToLoad != null)
            {
                InternalLoadScene(sceneToLoad);
                sceneToLoad = null;
            }
            else if (activeScene == null)
            {
                if (scenes != null && scenes.Count > 0)
                    InternalLoadScene(scenes[0]);
                else
                    activeScene = new Scene("Empty Scene");
            }
        }
        
        /// <summary>
        /// Instantly load a Scene.
        /// </summary>
        /// <param name="sceneData">The Scene to load.</param>
        internal static void InternalLoadScene(SceneData sceneData)
        {
            UnloadCurrentScene();
            activeScene = new Scene(sceneData);
        }
    }
}
