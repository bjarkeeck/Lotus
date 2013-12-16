using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LotusEditor
{
    public partial class EditView : Form
    {
        public LotusEngine.View view;

        public EditView(LotusEngine.View view)
        {
            InitializeComponent();

            DialogResult = System.Windows.Forms.DialogResult.Cancel;

            this.view = view;

            textBox1.Text = view.worldX.ToString();
            textBox2.Text = view.worldY.ToString();
            textBox3.Text = view.worldWidth.ToString();
            textBox4.Text = view.worldHeight.ToString();
            textBox5.Text = view.screenX.ToString();
            textBox6.Text = view.screenY.ToString();
            textBox7.Text = view.width.ToString();
            textBox8.Text = view.height.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            float result;

            if (float.TryParse(textBox1.Text, out result))
                view.worldX = result;
            if (float.TryParse(textBox2.Text, out result))
                view.worldY = result;
            if (float.TryParse(textBox3.Text, out result))
                view.worldWidth = result;
            if (float.TryParse(textBox4.Text, out result))
                view.worldHeight = result;
            if (float.TryParse(textBox5.Text, out result))
                view.screenX = result;
            if (float.TryParse(textBox6.Text, out result))
                view.screenY = result;
            if (float.TryParse(textBox7.Text, out result))
                view.width = result;
            if (float.TryParse(textBox8.Text, out result))
                view.height = result;

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}
