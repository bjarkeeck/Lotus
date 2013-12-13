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

            //  Set the clear color.
            gl.ClearColor(0, 0, 0, 0);

            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_MODULATE);

            // Load and bind all textures
            Textures.LoadAllTextures(gl);

            gl.Disable(OpenGL.GL_TEXTURE_2D);
        }

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
        }


        public static void DrawTexture(float x, float y, float width, float height, Texture texture, float alpha, bool additiveBlending = false)
        {
            // Configure to texture drawing
            gl.Enable(OpenGL.GL_BLEND);
            gl.Enable(OpenGL.GL_TEXTURE_2D);

            // Set blending
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, additiveBlending ? OpenGL.GL_ONE : OpenGL.GL_ONE_MINUS_SRC_ALPHA);

            // Bind the texture
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, texture.OpenGLName);

            // Set drawing alpha
            if (alpha < 0) alpha = 0;
            else if (alpha > 1) alpha = 1;

            gl.Color(alpha, alpha, alpha, alpha);

            // Draw
            gl.Begin(OpenGL.GL_QUADS);

            gl.TexCoord(0f, 0f); gl.Vertex(x, y);
            gl.TexCoord(1f, 0f); gl.Vertex(x + width, y);
            gl.TexCoord(1f, 1f); gl.Vertex(x + width, y + height);
            gl.TexCoord(0f, 1f); gl.Vertex(x, y + height);

            gl.End();

            // Reset drawing alpha
            gl.Color(1f, 1f, 1f, 1f);

            // Disable texture drawing
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.Disable(OpenGL.GL_BLEND);
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
