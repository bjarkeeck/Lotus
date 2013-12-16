using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LotusEngine;
using System.IO;

namespace LotusEditor
{
    public partial class EditSprite : Form
    {
        private Sprite sprite;

        public EditSprite(Sprite sprite)
        {
            InitializeComponent();

            this.sprite = sprite;

            foreach (var texture in sprite.textures)
            {
                lbImages.Items.Add(texture);
            }

            txtSpriteName.Text = sprite.name;
            nudSpriteFps.Value = (decimal)sprite.fps;
        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            SelectTextures selectTextures = new SelectTextures();

            selectTextures.ShowDialog(this);

            if (selectTextures.DialogResult == DialogResult.OK)
            {
                selectTextures.selectedTextures.ForEach(n => lbImages.Items.Add(n));
            }
        }

        private void lbImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbImages.SelectedItem != null)
            {
                var image = ((Texture)lbImages.SelectedItem).Bitmap;

                var graphics = pbImage.CreateGraphics();
                graphics.Clear(Color.Black);
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                graphics.DrawImage(image, 0, 0, pbImage.Width, pbImage.Height);
                graphics.Dispose();
            }
            else
            {
                pbImage.Image = null;
            }
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            int index = lbImages.SelectedIndex;

            if (lbImages.SelectedItem != null && index > 0)
            {
                var item = lbImages.SelectedItem;

                lbImages.Items.RemoveAt(index);
                lbImages.Items.Insert(index - 1, item);
                lbImages.SelectedIndex = index - 1;
            }
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            int index = lbImages.SelectedIndex;

            if (lbImages.SelectedItem != null && index < lbImages.Items.Count - 1)
            {
                var item = lbImages.SelectedItem;

                lbImages.Items.RemoveAt(index);
                lbImages.Items.Insert(index + 1, item);
                lbImages.SelectedIndex = index + 1;
            }
        }

        private void btnSaveSprite_Click(object sender, EventArgs e)
        {
            sprite.name = txtSpriteName.Text;
            sprite.fps = (float)nudSpriteFps.Value;
            sprite.RemoveAllTextures();

            foreach (Texture texture in lbImages.Items)
                sprite.AddTexture(texture);

            sprite.path = Settings.Assets.SpritePath + "\\" + sprite.name + ".sprite";

            if (!Directory.Exists(Settings.Assets.SpritePath))
                Directory.CreateDirectory(Settings.Assets.SpritePath);
            
            sprite.SaveSprite();
            Close();
        }

        private void lbImages_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete && lbImages.SelectedItem != null)
            {
                lbImages.Items.Remove(lbImages.SelectedItem);
            }
        }
    }
}