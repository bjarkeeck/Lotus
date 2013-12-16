using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpGL;
using System.Drawing;

namespace LotusEngine
{
    public static class Rendering
    {
        public static OpenGL gl { get; private set; }

        internal static void InitializeGraphics(OpenGL gl)
        {
            Rendering.gl = gl;

            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_MODULATE);

            // Load and bind all textures
            Textures.LoadAllTextures();

            gl.Disable(OpenGL.GL_TEXTURE_2D);
        }

        /// <summary>
        /// Transforms the draw matrix to the current view.
        /// </summary>
        public static void StartDrawing()
        {
            View view = Scene.ActiveScene.renderingView;

            float xScale = (view.width / Settings.Screen.Width) * (Settings.Screen.Width / view.worldWidth),
                  yScale = (view.height / Settings.Screen.Height) * (Settings.Screen.Height / view.worldHeight);

            OpenGL gl = Rendering.gl;

            gl.LoadIdentity();
            gl.Ortho(0, Settings.Screen.Width, Settings.Screen.Height, 0, 0, 1);
            gl.Translate(view.screenX, view.screenY, 0);
            gl.Scale(xScale, yScale, 1);
        }

        /// <summary>
        /// Transforms the draw matrix to the current view and the gameobject of the given component.
        /// </summary>
        /// <param name="component">The component to transform to.</param>
        /// <param name="rotate">Whether to rotate to the component's rotation.</param>
        public static void StartDrawing(Component component, bool rotate = true)
        {
            View view = Scene.ActiveScene.renderingView;

            float left = component.transform.position.x - view.worldX,
                  top = component.transform.position.y - view.worldY,
                  xScale = (view.width / Settings.Screen.Width) * (Settings.Screen.Width / view.worldWidth),
                  yScale = (view.height / Settings.Screen.Height) * (Settings.Screen.Height / view.worldHeight);

            OpenGL gl = Rendering.gl;

            gl.LoadIdentity();
            gl.Ortho(0, Settings.Screen.Width, Settings.Screen.Height, 0, 0, 1);
            gl.Translate(view.screenX, view.screenY, 0);
            gl.Scale(xScale, yScale, 1);
            gl.Translate(left, top, 0);
            if (rotate)
                gl.Rotate(0, 0, component.transform.rotation);
            gl.Scale(component.transform.scale.x, component.transform.scale.y, 1);

        }

        /// <summary>
        /// Draw's a texture, ( Remember to call Rendering.StartRendering(this) )
        /// </summary>
        /// <param name="texture">The texture, ( use Textures.GetTexture("texture name") )</param>
        /// <param name="position">The texture position relative to GameObject.Transform.Position</param>
        /// <param name="size">Specify the width & height of the texture, set x or y to zero, to maintain aspect ratio.</param>
        /// <param name="sourceRect">A rectangle that specifies wich part of the texture should be renderd. Use null, to draw the entire texture</param>
        /// <param name="color">The tint color of wich the wich the texture will be rendered with.</param>
        /// <param name="scale">Scale factor. 1 = 100%</param>
        /// <param name="rotation">Specifies the angle (in degrees) relatives to GameObject.Transform.Rotation </param>
        /// <param name="rotationOrigin">Specifies the position relative to the texture, of wich the it will be rotated around.</param>
        /// <param name="textureFlip">Flip the image, Vertical or horisontal</param>
        /// <param name="additiveBlending">Enable additive blend-mode</param>
        public static void DrawTexture(Texture texture, Vector2 position, Vector2 size, Rectangle? sourceRect, Color color, float scale, float rotation, Vector2 rotationOrigin, TextureFlip textureFlip = TextureFlip.None, bool additiveBlending = false)
        {
            // Configure to texture drawing
            gl.Enable(OpenGL.GL_BLEND);
            gl.Enable(OpenGL.GL_TEXTURE_2D);

            // Set blending
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, additiveBlending ? OpenGL.GL_ONE : OpenGL.GL_ONE_MINUS_SRC_ALPHA);

            // Bind the texture
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, texture.OpenGLName);

            // Set drawing color
            gl.Color((float)color.R / 255f, (float)color.G / 255f, (float)color.B / 255f, (float)color.A / 255f);

            // Set size
            if (size.x == 0 && size.y > 0)
                size.x = texture.Bitmap.Width / (float)texture.Bitmap.Height * size.y;
            else if (size.y == 0 && size.x > 0)
                size.y = texture.Bitmap.Height / (float)texture.Bitmap.Width * size.x;
            else if (size.y == 0 && size.x == 0)
                size = new Vector2(texture.Bitmap.Width, texture.Bitmap.Height);

            // set rotation
            if (rotation != 0)
            {
                //Set origin first, then rotate
                gl.Translate(position.x + rotationOrigin.x, position.y + rotationOrigin.y, 0);
                gl.Rotate(0, 0, rotation);
                //Reset back to original position.
                gl.Translate(position.x - rotationOrigin.x, position.y - rotationOrigin.y, 0);
            }

            // sourceRect
            sourceRect = sourceRect ?? new Rectangle(0, 0, (int)size.x, (int)size.y);

            // Scale
            gl.Scale(1 * scale, 1 * scale, 1);

            //Sørger for at positionere forbliver bliver ens, elvom elementerne bliver scaleret...
            position /= scale;

            // Draw
            gl.Begin(OpenGL.GL_QUADS);

            // Left Top
            gl.TexCoord(sourceRect.Value.X / size.x, sourceRect.Value.Y / size.y);
            gl.Vertex(position.x + (textureFlip == TextureFlip.FlipHorizontally ? size.x : 0), position.y + (textureFlip == TextureFlip.FlipVertical ? size.y : 0));

            // Right Top
            gl.TexCoord(sourceRect.Value.Width / size.x, sourceRect.Value.Y / size.y);
            gl.Vertex(position.x + (textureFlip == TextureFlip.FlipHorizontally ? 0 : size.x), position.y + (textureFlip == TextureFlip.FlipVertical ? size.y : 0));

            // Botom Left
            gl.TexCoord(sourceRect.Value.Width / size.x, sourceRect.Value.Height / size.y);
            gl.Vertex(position.x + (textureFlip == TextureFlip.FlipHorizontally ? 0 : size.x), position.y + (textureFlip == TextureFlip.FlipVertical ? 0 : size.y));

            // Bottom Right
            gl.TexCoord(sourceRect.Value.X / size.x, sourceRect.Value.Height / size.y);
            gl.Vertex(position.x + (textureFlip == TextureFlip.FlipHorizontally ? size.x : 0), position.y + (textureFlip == TextureFlip.FlipVertical ? 0 : size.y));

            gl.End();

            // Reset drawing alpha
            gl.Color(1f, 1f, 1f, 1f);

            // Reset scale
            gl.Scale(1 / scale, 1 / scale, 1f);

            // Reset rotation
            if (rotation != 0)
            {
                gl.Translate(position.x + rotationOrigin.x, position.y + rotationOrigin.y, 0);
                gl.Rotate(0, 0, -rotation);
                gl.Translate(position.x - rotationOrigin.x, position.y - rotationOrigin.y, 0);
            }

            // Disable texture drawing
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.Disable(OpenGL.GL_BLEND);

        }
        /// <summary>
        /// Draw's a texture, ( Remember to call Rendering.StartRendering(this) )
        /// </summary>
        /// <param name="texture">The texture, ( use Textures.GetTexture("texture name") )</param>
        /// <param name="position">The texture position relative to GameObject.Transform.Position</param>
        /// <param name="color">The tint color of wich the wich the texture will be rendered with.</param>
        /// <param name="size">Specify the width & height of the texture, set x or y to zero, to maintain aspect ratio.</param>
        /// <param name="rotation">Specifies the angle (in degrees) relatives to GameObject.Transform.Rotation </param>
        /// <param name="rotationOrigin">Specifies the position relative to the texture, of wich the it will be rotated around.</param>
        /// <param name="textureFlip">Flip the image, Vertical or horisontal</param>
        /// <param name="additiveBlending">Enable additive blend-mode</param>
        public static void DrawTexture(Texture texture, Vector2 position, Color color, Vector2 size, float rotation, Vector2 rotationOrigin, TextureFlip textureFlip = TextureFlip.None, bool additiveBlending = false)
        {
            DrawTexture(texture, position, size, null, color, 1, rotation, rotationOrigin, textureFlip, additiveBlending);
        }
        /// <summary>
        /// Draw's a texture, ( Remember to call Rendering.StartRendering(this) )
        /// </summary>
        /// <param name="texture">The texture, ( use Textures.GetTexture("texture name") )</param>
        /// <param name="position">The texture position relative to GameObject.Transform.Position</param>
        /// <param name="color">The tint color of wich the wich the texture will be rendered with.</param>
        /// <param name="scale">Scale factor. 1 = 100%</param>
        /// <param name="rotation">Specifies the angle (in degrees) relatives to GameObject.Transform.Rotation </param>
        /// <param name="rotationOrigin">Specifies the position relative to the texture, of wich the it will be rotated around.</param>
        /// <param name="textureFlip">Flip the image, Vertical or horisontal</param>
        /// <param name="additiveBlending">Enable additive blend-mode</param>
        public static void DrawTexture(Texture texture, Vector2 position, Color color, float scale, float rotation, Vector2 rotationOrigin, TextureFlip textureFlip = TextureFlip.None, bool additiveBlending = false)
        {
            DrawTexture(texture, position, Vector2.zero, null, color, scale, rotation, rotationOrigin, textureFlip, additiveBlending);
        }
        /// <summary>
        /// Draw's a texture, ( Remember to call Rendering.StartRendering(this) )
        /// </summary>
        /// <param name="texture">The texture, ( use Textures.GetTexture("texture name") )</param>
        /// <param name="position">The texture position relative to GameObject.Transform.Position</param>
        /// <param name="color">The tint color of wich the wich the texture will be rendered with.</param>
        /// <param name="rotation">Specifies the angle (in degrees) relatives to GameObject.Transform.Rotation </param>
        /// <param name="rotationOrigin">Specifies the position relative to the texture, of wich the it will be rotated around.</param>
        /// <param name="textureFlip">Flip the image, Vertical or horisontal</param>
        /// <param name="additiveBlending">Enable additive blend-mode</param>
        public static void DrawTexture(Texture texture, Vector2 position, Color color, float rotation, Vector2 rotationOrigin, TextureFlip textureFlip = TextureFlip.None, bool additiveBlending = false)
        {
            DrawTexture(texture, position, Vector2.zero, null, color, 1, rotation, rotationOrigin, textureFlip, additiveBlending);
        }
        /// <summary>
        /// Draw's a texture, ( Remember to call Rendering.StartRendering(this) )
        /// </summary>
        /// <param name="texture">The texture, ( use Textures.GetTexture("texture name") )</param>
        /// <param name="position">The texture position relative to GameObject.Transform.Position</param>
        /// <param name="color">The tint color of wich the wich the texture will be rendered with.</param>
        /// <param name="size">Specify the width & height of the texture, set x or y to zero, to maintain aspect ratio.</param>
        /// <param name="scale">Scale factor. 1 = 100%</param>
        /// <param name="textureFlip">Flip the image, Vertical or horisontal</param>
        /// <param name="additiveBlending">Enable additive blend-mode</param>
        public static void DrawTexture(Texture texture, Vector2 position, Color color, Vector2 size, float scale = 1, TextureFlip textureFlip = TextureFlip.None, bool additiveBlending = false)
        {
            DrawTexture(texture, position, size, null, color, scale, 0, Vector2.zero, textureFlip, additiveBlending);
        }
        /// <summary>
        /// Draw's a texture, ( Remember to call Rendering.StartDrawing(this) )
        /// </summary>
        /// <param name="texture">The texture, ( use Textures.GetTexture("texture name") )</param>
        /// <param name="position">The texture position relative to the GameObject.Transform.Position of the </param>
        /// <param name="color">The tint color of wich the wich the texture will be rendered with.</param>
        /// <param name="scale">Scale factor. 1 = 100%</param>
        /// <param name="textureFlip">Flip the image, Vertical or horisontal</param>
        /// <param name="additiveBlending">Enable additive blend-mode</param>
        public static void DrawTexture(Texture texture, Vector2 position, Color color, float scale = 1, TextureFlip textureFlip = TextureFlip.None, bool additiveBlending = false)
        {
            DrawTexture(texture, position, Vector2.zero, null, color, scale, 0, Vector2.zero, textureFlip, additiveBlending);
        }


        public static void FillRectangle(float x, float y, float width, float height, Color color, bool additiveBlending = false)
        {
            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, additiveBlending ? OpenGL.GL_ONE : OpenGL.GL_ONE_MINUS_SRC_ALPHA);

            // Set drawing color
            gl.Color((float)color.R / 255f, (float)color.G / 255f, (float)color.B / 255f, (float)color.A / 255f);

            // Draw
            gl.Begin(OpenGL.GL_QUADS);

            gl.Vertex(x, y);
            gl.Vertex(x + width, y);
            gl.Vertex(x + width, y + height);
            gl.Vertex(x, y + height);

            gl.End();

            // Reset drawing color
            gl.Color(1f, 1f, 1f, 1f);

            gl.Disable(OpenGL.GL_BLEND);
        }

        public static void DrawLine(Vector2 a, Vector2 b, float width, Color color, LineStipling stipling = LineStipling.None, byte stiplingFactor = 1, bool additiveBlending = false)
        {
            // Enable line smoothing
            gl.Enable(OpenGL.GL_LINE_SMOOTH);
            gl.Hint(OpenGL.GL_LINE_SMOOTH_HINT, OpenGL.GL_NICEST);

            // Enable blending
            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, additiveBlending ? OpenGL.GL_ONE : OpenGL.GL_ONE_MINUS_SRC_ALPHA);

            // Set drawing color
            gl.Color((float)color.R / 255f, (float)color.G / 255f, (float)color.B / 255f, (float)color.A / 255f);

            // Enable stipling
            if (stipling != LineStipling.None)
            {
                gl.LineStipple(stiplingFactor, (ushort)stipling);
                gl.Enable(OpenGL.GL_LINE_STIPPLE);
            }
            else
                gl.Disable(OpenGL.GL_LINE_STIPPLE);

            // Set line width
            gl.LineWidth(width);

            // Draw
            gl.Begin(OpenGL.GL_LINES);

            gl.Vertex(a.x, a.y);
            gl.Vertex(b.x, b.y);

            gl.End();

            // Reset drawing color
            gl.Color(1f, 1f, 1f, 1f);

            // Disable stipling
            if (stipling != LineStipling.None)
            {
                gl.Disable(OpenGL.GL_LINE_STIPPLE);
            }

            // Disable blending and line smoothing
            gl.Disable(OpenGL.GL_BLEND);
            gl.Disable(OpenGL.GL_LINE_SMOOTH);
        }

        public static void DrawCircle(Vector2 center, float radius, float lineWidth, Color color, LineStipling stipling = LineStipling.None, byte stiplingFactor = 1, bool additiveBlending = false)
        {
            // Enable line smoothing
            gl.Enable(OpenGL.GL_LINE_SMOOTH);
            gl.Hint(OpenGL.GL_LINE_SMOOTH_HINT, OpenGL.GL_NICEST);

            // Enable blending
            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, additiveBlending ? OpenGL.GL_ONE : OpenGL.GL_ONE_MINUS_SRC_ALPHA);

            // Set drawing color
            gl.Color((float)color.R / 255f, (float)color.G / 255f, (float)color.B / 255f, (float)color.A / 255f);

            // Enable stipling
            if (stipling != LineStipling.None)
            {
                gl.LineStipple(stiplingFactor, (ushort)stipling);
                gl.Enable(OpenGL.GL_LINE_STIPPLE);
            }
            else
                gl.Disable(OpenGL.GL_LINE_STIPPLE);


            // Set line width
            gl.LineWidth(lineWidth);

            // Draw
            gl.Begin(OpenGL.GL_LINES);

            for (int i = 0; i <= 360; i++)
            {
                float degInRad1 = Vector2.FloatDeg2Rad(i),
                      degInRad2 = Vector2.FloatDeg2Rad(i + 1);
                gl.Vertex(center.x + Math.Cos(degInRad1) * radius, center.y + Math.Sin(degInRad1) * radius);
                gl.Vertex(center.x + Math.Cos(degInRad2) * radius, center.y + Math.Sin(degInRad2) * radius);
            }

            gl.End();

            // Reset drawing color
            gl.Color(1f, 1f, 1f, 1f);

            // Disable stipling
            if (stipling != LineStipling.None)
            {
                gl.Disable(OpenGL.GL_LINE_STIPPLE);
            }

            // Disable blending and line smoothing
            gl.Disable(OpenGL.GL_BLEND);
            gl.Disable(OpenGL.GL_LINE_SMOOTH);
        }
    }
}
