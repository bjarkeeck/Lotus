using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusEngine
{
    public static class Settings
    {
        public static class Assets
        {
            public static string PrefabPath = "prefabs";
            public static string TexturePath = "textures";
            public static string SpritePath = "sprites";
            public static string SoundPath = "sounds";
            public static string ScenePath = "scenes";
        }

        public static class Screen
        {
            public static int Width { get; internal set; }
            public static int Height { get; internal set; }
            public static int ScreenPositionX { get; set; }
            public static int ScreenPositionY { get; set; }
        }

        public static class Editor
        {
            public static bool EditorIsRunning { get; set; }
        }
    }
}
