using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpGL;
using System.Drawing.Imaging;

namespace Lotus
{
    /// <summary>
    /// The main form class.
    /// </summary>
    public partial class SharpGLForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharpGLForm"/> class.
        /// </summary>
        public SharpGLForm()
        {
            InitializeComponent();

            WindowState = FormWindowState.Normal;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
        }

        /// <summary>
        /// Handles the OpenGLDraw event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLDraw(object sender, PaintEventArgs e)
        {
            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Clear the color and depth buffer.
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            gl.MatrixMode(MatrixMode.Projection);

            //  Load the identity matrix.
            gl.LoadIdentity();

            gl.Ortho(0, Bounds.Width, Bounds.Height, 0, 0, 1);
            gl.Scale(Width / 1920f, Height / 1080f, 1);

            //gl.MatrixMode(MatrixMode.Modelview);

            gl.BindTexture(OpenGL.GL_TEXTURE_2D, textures[0]);

            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE);
            //gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            
            gl.Color(1, 1, 1, 1f);

            gl.Begin(OpenGL.GL_QUADS);

            gl.TexCoord(0.0f, 0.0f); gl.Vertex(0, 0);	// Bottom Left Of The Texture and Quad
            gl.TexCoord(1.0f, 0.0f); gl.Vertex(1000, 0);	// Bottom Right Of The Texture and Quad
            gl.TexCoord(1.0f, 1.0f); gl.Vertex(1000, 1000);	// Top Right Of The Texture and Quad
            gl.TexCoord(0.0f, 1.0f); gl.Vertex(0, 1000);	// Top Left Of The Texture and Quad

            gl.End();


            gl.Begin(OpenGL.GL_QUADS);
            
            gl.TexCoord(0.0f, 0.0f); gl.Vertex(100, 100);	// Bottom Left Of The Texture and Quad
            gl.TexCoord(1.0f, 0.0f); gl.Vertex(1100, 100);	// Bottom Right Of The Texture and Quad
            gl.TexCoord(1.0f, 1.0f); gl.Vertex(1100, 1100);	// Top Right Of The Texture and Quad
            gl.TexCoord(0.0f, 1.0f); gl.Vertex(100, 1100);	// Top Left Of The Texture and Quad

            gl.TexCoord(0.0f, 0.0f); gl.Vertex(100, 100);	// Bottom Left Of The Texture and Quad
            gl.TexCoord(1.0f, 0.0f); gl.Vertex(1100, 100);	// Bottom Right Of The Texture and Quad
            gl.TexCoord(1.0f, 1.0f); gl.Vertex(1100, 1100);	// Top Right Of The Texture and Quad
            gl.TexCoord(0.0f, 1.0f); gl.Vertex(100, 1100);	// Top Left Of The Texture and Quad

            gl.TexCoord(0.0f, 0.0f); gl.Vertex(100, 100);	// Bottom Left Of The Texture and Quad
            gl.TexCoord(1.0f, 0.0f); gl.Vertex(1100, 100);	// Bottom Right Of The Texture and Quad
            gl.TexCoord(1.0f, 1.0f); gl.Vertex(1100, 1100);	// Top Right Of The Texture and Quad
            gl.TexCoord(0.0f, 1.0f); gl.Vertex(100, 1100);	// Top Left Of The Texture and Quad

            gl.TexCoord(0.0f, 0.0f); gl.Vertex(100, 100);	// Bottom Left Of The Texture and Quad
            gl.TexCoord(1.0f, 0.0f); gl.Vertex(1100, 100);	// Bottom Right Of The Texture and Quad
            gl.TexCoord(1.0f, 1.0f); gl.Vertex(1100, 1100);	// Top Right Of The Texture and Quad
            gl.TexCoord(0.0f, 1.0f); gl.Vertex(100, 1100);	// Top Left Of The Texture and Quad

            gl.End();


            //gl.Disable(OpenGL.GL_BLEND);
            //gl.Disable(OpenGL.GL_TEXTURE_2D);

            ////gl.Begin(OpenGL.GL_QUADS);

            ////gl.Color(1.0f, 0.0f, 0.0f, 1.0f);
            ////gl.Vertex(100.0f, 100.0f);
            ////gl.Color(0.5f, 0.5f, 0.0f, 0.0f);
            ////gl.Vertex(800.0f, 100.0f);
            ////gl.Color(0.0f, 1.0f, 0.0f, 1.0f);
            ////gl.Vertex(800.0f, 800.0f);
            ////gl.Color(0.0f, 0.0f, 1.0f, 0.0f);
            ////gl.Vertex(100.0f, 800.0f);

            ////gl.End();
            //gl.Enable(OpenGL.GL_TEXTURE_2D);

        }



        /// <summary>
        /// Handles the OpenGLInitialized event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            //  TODO: Initialise OpenGL here.

            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Set the clear color.
            gl.ClearColor(0, 0, 0, 0);

            //gl.Disable(OpenGL.GL_DEPTH_TEST);

            bitmap = new Bitmap(@"c:\AFyUoX9.png");

            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.TexEnv(OpenGL.GL_TEXTURE_ENV, OpenGL.GL_TEXTURE_ENV_MODE, OpenGL.GL_MODULATE);

            gl.GenTextures(1, textures);
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, textures[0]);


            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, bitmap.PixelFormat);
            gl.TexImage2D(OpenGL.GL_TEXTURE_2D, 0, (int)OpenGL.GL_RGBA,
               bitmap.Width, bitmap.Height, 0, OpenGL.GL_BGRA, OpenGL.GL_UNSIGNED_BYTE,
               bitmapData.Scan0);


            //gl.TexImage2D(OpenGL.GL_TEXTURE_2D, 0, 3, bitmap.Width, bitmap.Height, 0, OpenGL.GL_BGR, OpenGL.GL_UNSIGNED_BYTE,
            //   bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
            //   ImageLockMode.ReadOnly, bitmap.PixelFormat).Scan0);

            //  Set linear filtering mode.
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_LINEAR);
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_LINEAR);
            

        }

        /// <summary>
        /// Handles the Resized event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void openGLControl_Resized(object sender, EventArgs e)
        {
            //  TODO: Set the projection matrix here.

            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Set the projection matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);

            //  Load the identity.
            gl.LoadIdentity();

            //  Create a perspective transformation.
            gl.Perspective(60.0f, (double)Width / (double)Height, 0.01, 100.0);

            //  Use the 'look at' helper function to position and aim the camera.
            gl.LookAt(-5, 5, -5, 0, 0, 0, 0, 1, 0);

            //  Set the modelview matrix.
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        private uint[] textures = new uint[1];

        Bitmap bitmap;
    }
}
