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
    public partial class SelectSprite : Form
    {
        public SelectSprite()
        {
            InitializeComponent();

            ReloadForm();
        }

        private void ReloadForm()
        {
            Sprite.LoadAllSprites();

            lbSprites.Items.Clear();

            foreach (var sprite in Sprite.GetAllSprites())
                lbSprites.Items.Add(sprite);
        }

        private void btnAddSprite_Click(object sender, EventArgs e)
        {
            Sprite sprite = new Sprite("new sprite", 10);
            EditSprite editSprite = new EditSprite(sprite);
            editSprite.ShowDialog(this);
            ReloadForm();
        }

        private void btnDeleteSprite_Click(object sender, EventArgs e)
        {
            if (lbSprites.SelectedItem != null && MessageBox.Show(this, "Are you sure you want to delete this sprite?\nIt cannot be undone.", "CAUTION", MessageBoxButtons.YesNo) == DialogResult.OK)
            {
                File.Delete((lbSprites.SelectedItem as Sprite).path);
                ReloadForm();
            }
        }

        private void lbSprites_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = lbSprites.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                EditSprite editSprite = new EditSprite((Sprite)lbSprites.SelectedItem);
                editSprite.ShowDialog(this);
            }
        }
    }
}
