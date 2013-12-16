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

namespace LotusEditor
{
    public partial class SelectView : Form
    {
        private Scene scene;

        public SelectView(Scene scene)
        {
            InitializeComponent();

            this.scene = scene;
            ReloadForm();
        }

        private void ReloadForm()
        {
            lbViews.Items.Clear();
            lbViews.Items.AddRange(scene.views.ToArray());
        }

        private void lbViews_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = lbViews.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                EditView editView = new EditView((LotusEngine.View)lbViews.SelectedItem);
                editView.ShowDialog(this);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lbViews.SelectedItem != null && MessageBox.Show(this, "Are you sure you want to delete this view?\nIt cannot be undone.", "CAUTION", MessageBoxButtons.YesNo) == DialogResult.OK)
            {
                scene.RemoveView((LotusEngine.View)lbViews.SelectedItem);
                ReloadForm();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            EditView view = new EditView(new LotusEngine.View(0, 0, 0, 0, 0, 0, 0, 0));

            view.ShowDialog(this);

            if (view.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                scene.AddView(view.view);
            }
        }
    }
}