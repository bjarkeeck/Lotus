using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotusEngine
{
    /// <summary>
    /// Defines a viewport on the screen.
    /// </summary>
    [Serializable]
    public sealed class View
    {
        /// <summary>
        /// Create a new View instance.
        /// </summary>
        public View()
        { }
        /// <summary>
        /// Create a new View instance.
        /// </summary>
        /// <param name="worldX">The world x position.</param>
        /// <param name="worldY">The world y position</param>
        /// <param name="worldWidth">The world width.</param>
        /// <param name="worldHeight">The world height.</param>
        /// <param name="screenX">The screen x position.</param>
        /// <param name="screenY">The screen y position.</param>
        /// <param name="width">The screen width.</param>
        /// <param name="height">The screen height.</param>
        public View(float worldX, float worldY, float worldWidth, float worldHeight, float screenX, float screenY, float width, float height)
        {
            this.worldX = worldX;
            this.worldY = worldY;
            this.worldWidth = worldWidth;
            this.worldHeight = worldHeight;
            this.screenX = screenX;
            this.screenY = screenY;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// The world x position.
        /// </summary>
        public float worldX { get; set; }
        /// <summary>
        /// The world y position.
        /// </summary>
        public float worldY { get; set; }
        /// <summary>
        /// The world width.
        /// </summary>
        public float worldWidth { get; set; }
        /// <summary>
        /// The world height.
        /// </summary>
        public float worldHeight { get; set; }
        /// <summary>
        /// The screen x position.
        /// </summary>
        public float screenX { get; set; }
        /// <summary>
        /// The screen y position.
        /// </summary>
        public float screenY { get; set; }

        private float width_field;
        /// <summary>
        /// The screen width.
        /// </summary>
        public float width
        { 
            get
            {
                if (Scene.ActiveScene.views[0] == this)
                    return Settings.Screen.Width;
                return width_field;
            }
            set
            {
                width_field = value;
            }
        }

        private float height_field;
        /// <summary>
        /// The screen height.
        /// </summary>
        public float height
        {
            get
            {
                if (Scene.ActiveScene.views[0] == this)
                    return Settings.Screen.Height;
                return height_field;
            }
            set
            {
                height_field = value;
            }
        }

        /// <summary>
        /// Transforms screen coordinates to a world position.
        /// </summary>
        /// <param name="position">The screen coordinates.</param>
        /// <returns>The world position.</returns>
        public Vector2 ScreenToWorldPosition(Vector2 position)
        {
            float left, top, xScale, yScale;

            xScale = worldWidth / width;
            yScale = worldHeight / height;

            left = worldX + position.x * xScale;
            top = worldY + position.y * yScale;

            return new Vector2(left, top);
        }

        /// <summary>
        /// Transforms a world position to screen coordinates.
        /// </summary>
        /// <param name="position">The world position.</param>
        /// <returns>The screen coordinates.</returns>
        public Vector2 WorldToScreenPosition(Vector2 position)
        {
            float left, top, xScale, yScale;

            xScale = worldWidth / width;
            yScale = worldHeight / height;

            left = -((worldX - position.x) / xScale);
            top = -((worldY - position.y) / yScale);

            return new Vector2(left, top);
        }
    }
}
