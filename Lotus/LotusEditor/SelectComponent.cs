using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using Lotus;

namespace LotusEditor
{
    public partial class SelectComponent : Form
    {
        public Type SelectedComponent { get; private set; }

        public SelectComponent()
        {
            InitializeComponent();

            DialogResult = DialogResult.Abort;

            Assembly.Load("Lotus");

            lbComponents.Items.AddRange((from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                         from type in assembly.GetTypes()
                                         where typeof(LotusEngine.Component).IsAssignableFrom(type)
                                               && type != typeof(LotusEngine.Component)
                                               && !type.IsAbstract
                                         select type).ToArray());
        }

        private void lbComponents_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = lbComponents.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                DialogResult = DialogResult.OK;
                SelectedComponent = (Type)lbComponents.SelectedItem;
                Close();
            }
        }
    }
}
