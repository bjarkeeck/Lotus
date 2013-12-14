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
using LotusEngine;
using LotusEngine.Components;

namespace Lotus
{
    /// <summary>
    /// The main form class.
    /// </summary>
    /// 

    public partial class SharpGLForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharpGLForm"/> class.
        /// </summary>
        /// 
        public SharpGLForm()
        {
            InitializeComponent();

            WindowState = FormWindowState.Normal;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            //this.Location = new Point(-1 * this.Width + 99999, -1 * this.Height);

        }

        /// <summary>
        /// Handles the OpenGLDraw event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLDraw(object sender, PaintEventArgs e)
        {
            if (ActiveForm == this)
            {
                Core.Update();
            }

            Core.Draw();
        }


        /// <summary>
        /// Handles the OpenGLInitialized event of the openGLControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            Core.InitializeEngine(openGLControl.OpenGL, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            GameObject test = GameObject.Instantiate("test", new Vector2(Settings.Screen.Width / 2, Settings.Screen.Height / 2), 45);

            test.AddComponent<Renderer>();
            test.AddComponent<Test>();
            test.AddComponent<ParticleEmitter>();

            var poly = test.AddComponent<PolygonCollider>();

            poly.points = new Vector2[] {
                new Vector2(-100, 100),
                new Vector2(100, 100),
                new Vector2(0, -100),
            };

            poly.drawCollider = true;
            poly.useColliderRotation = true;

            test = GameObject.Instantiate("test", new Vector2(Settings.Screen.Width / 2 - 300, Settings.Screen.Height / 2 - 300), 45);

            var circle = test.AddComponent<CircleCollider>();

            circle.radius = 200;
            circle.drawCollider = true;

            timerGameLoop.Start();
        }

        private void timerGameLoop_Tick(object sender, EventArgs e)
        {
            openGLControl.Invalidate();
            openGLControl.Update();

            timerGameLoop.Start();
        }
    }
}
