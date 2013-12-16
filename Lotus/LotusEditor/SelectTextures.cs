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
    public partial class SelectTextures : Form
    {
        public List<Texture> selectedTextures;

        public SelectTextures()
        {
            InitializeComponent();

            DialogResult = DialogResult.Abort;

            ReloadTextures();
        }

        private void btnAddTextures_Click(object sender, EventArgs e)
        {
            openTextures.ShowDialog(this);

            foreach (var path in openTextures.FileNames)
            {
                File.Copy(path, Settings.Assets.TexturePath + "\\" + Path.GetFileName(path));
            }

            Textures.LoadAllTextures();
            ReloadTextures();
        }

        private void ReloadTextures()
        {
            lbTextures.Items.Clear();

            foreach (var texture in Textures.GetAllTextures())
                lbTextures.Items.Add(texture);
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            if (lbTextures.SelectedItems.Count > 0)
            {
                selectedTextures = new List<Texture>();

                foreach (var item in lbTextures.SelectedItems)
                    selectedTextures.Add((Texture)item);

                DialogResult = DialogResult.OK;
            }

            Close();
        }

        private void lbTextures_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete && MessageBox.Show("Are you sure you want to delete this/these textures?\nThis action cannot be undone!", "WARNING!!11!1!!!lL!L1", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (Texture texture in lbTextures.SelectedItems)
                {
                    texture.Bitmap.Dispose();
                    File.Delete(texture.Path);
                }

                Textures.LoadAllTextures();
                ReloadTextures();
            }
        }
    }
}
