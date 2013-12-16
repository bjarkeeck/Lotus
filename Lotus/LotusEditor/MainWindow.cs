using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using LotusEngine;
using SharpGL;

namespace LotusEditor
{
    // TODO Sound support
    // TODO Copy Paste
    // TODO Click/drag/multiple selection
    public partial class MainWindow : Form
    {
        public readonly List<Type> AllowedTypes = new List<Type>(new Type[] {
            typeof(string),
            typeof(int),
            typeof(float),
            typeof(bool),
            typeof(Vector2),
            typeof(Color),
            typeof(Sprite)
        });

        private LotusEngine.View view;
        private bool draggingView;
        private Vector2 dragStartPointScreen,
                        dragStartPointWorld;

        private Prefab selectedPrefab;
        private GameObject selectedGameObject;

        public MainWindow()
        {
            InitializeComponent();

            this.openGLControl.MouseWheel += new MouseEventHandler(pbScreen_MouseWheel);
            this.lbPrefabs.MouseDoubleClick += new MouseEventHandler(lbPrefabs_DoubleClick);
            this.lbPrefabs.KeyDown += new KeyEventHandler(lbPrefabs_KeyDown);
            this.lbSceneObjects.MouseDoubleClick += new MouseEventHandler(lbSceneObjects_DoubleClick);
            this.lbSceneObjects.KeyDown += new KeyEventHandler(lbSceneObjects_KeyDown);

            //foreach (var file in new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles())
            //{
            //    if (file.Extension == ".exe")
            //    {
            //        Assembly.LoadFrom(file.FullName);
            //    }
            //}
        }

        public void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            view = new LotusEngine.View(0, 0, openGLControl.Width, openGLControl.Height, 0, 0, openGLControl.Width, openGLControl.Height);

            Settings.Editor.EditorIsRunning = true;
            Core.InitializeEngine(openGLControl.OpenGL, openGLControl.Width, openGLControl.Height);

            var args = Environment.GetCommandLineArgs();

            if (args.Any(n => Scene.Exists(n)))
            {
                string scene = args.First(n => Scene.Exists(n));
                Scene.LoadScene(scene);
            }

            timerGameLoop.Start();
        }

        private void openGLControl_OpenGLDraw(object sender, PaintEventArgs e)
        {
            if (ActiveForm == this)
            {
                Core.Update();

                Settings.Screen.ScreenPositionX = openGLControl.PointToScreen(Point.Empty).X;
                Settings.Screen.ScreenPositionY = openGLControl.PointToScreen(Point.Empty).Y;

                if (Input.GetKeyDown(Keys.MButton) && MouseInEditorWindow())
                {
                    draggingView = true;
                    dragStartPointScreen = Input.MousePosition;
                    dragStartPointWorld = new Vector2(view.worldX, view.worldY);
                }

                if (Input.GetKeyUp(Keys.MButton))
                {
                    draggingView = false;
                }

                if (draggingView)
                {
                    var pos = dragStartPointWorld + dragStartPointScreen - Input.MousePosition;

                    view.worldX = pos.x;
                    view.worldY = pos.y;

                    lblViewPosition.Text = view.worldX + ";" + view.worldY;
                }
            }

            Core.Draw(view, cbDrawGUI.Checked);
        }

        private void CompleteReloadEditor()
        {
            Prefab.ClearPrefabCache();
            Prefab.LoadAllPrefabs();

            Scene.LoadAllScenes();

            ReloadEditor();
        }

        private void ReloadEditor()
        {
            lbPrefabs.Items.Clear();
            foreach (Prefab prefab in Prefab.GetAllPrefabs())
            {
                lbPrefabs.Items.Add(prefab);
            }

            lbSceneObjects.Items.Clear();

            var sortedObjects = Scene.ActiveScene.sceneObjects;
            sortedObjects.Sort((g1, g2) => g1.name.CompareTo(g2.name));
            foreach (GameObject go in sortedObjects)
            {
                lbSceneObjects.Items.Add(go);
            }

            if (selectedGameObject != null)
                SelectGameobject(selectedGameObject);
            else if (selectedPrefab != null)
                SelectPrefab(selectedPrefab);
            else
            {
                panelEdit.Controls.Clear();
                lblEditObject.Text = "Edit Object";
            }

            btnSceneColor.BackColor = Scene.ActiveScene.bgColor;
            Text = "Lotus Editor v1.0 - " + Scene.ActiveScene.name + ".scene";
        }

        private void SaveProject()
        {
            foreach (Prefab prefab in lbPrefabs.Items)
            {
                prefab.SavePrefab();
            }

            SceneData sceneData = new SceneData(Scene.ActiveScene);
            sceneData.SaveSceneData();
        }

        private void timerGameLoop_Tick(object sender, EventArgs e)
        {
            openGLControl.Invalidate();
            openGLControl.Update();

            timerGameLoop.Start();
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {
            ReloadEditor();
        }

        private void MainWindow_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Prefab)))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void MainWindow_DragDrop(object sender, DragEventArgs e)
        {
            if (MouseInEditorWindow())
            {
                if (e.Data.GetDataPresent(typeof(Prefab)))
                {
                    GameObject.Instantiate((Prefab)e.Data.GetData(typeof(Prefab)), view.ScreenToWorldPosition(Input.MousePosition));

                    ReloadEditor();
                }
            }
        }

        private bool MouseInEditorWindow()
        {
            return ContainsFocus
                && Input.MousePosition.x >= 0
                && Input.MousePosition.x <= Settings.Screen.Width
                && Input.MousePosition.y >= 0
                && Input.MousePosition.y <= Settings.Screen.Height;
        }

        #region pbScreen
        private void pbScreen_MouseWheel(object sender, MouseEventArgs e)
        {
            if (MouseInEditorWindow() && e.Delta != 0)
            {
                float zoom = 1 - (e.Delta * 0.1f / 120),
                      w = view.worldWidth,
                      h = view.worldHeight;

                view.worldWidth *= zoom;
                view.worldHeight *= zoom;

                view.worldX -= (view.worldWidth - w) * 0.5f;
                view.worldY -= (view.worldHeight - h) * 0.5f;
            }
        }

        private void openGLControl_MouseEnter(object sender, EventArgs e)
        {
            //openGLControl.Focus();
        }

        private void openGLControl_MouseLeave(object sender, EventArgs e)
        {
            this.Focus();
        }
        #endregion

        #region lbPrefabs
        private void lbPrefabs_DoubleClick(object sender, MouseEventArgs e)
        {
            int index = lbPrefabs.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                SelectPrefab((Prefab)lbPrefabs.SelectedItem);
            }
        }

        private void lbPrefabs_KeyDown(object sender, KeyEventArgs e)
        {
            Prefab prefab = lbPrefabs.SelectedItem as Prefab;

            if (e.KeyData == Keys.Delete && MessageBox.Show("Are you sure you want to delete this prefab?\nIt cannot be undone.", "CAUTION", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Prefab.DeletePrefab(prefab);
                if (selectedPrefab == prefab)
                    selectedPrefab = null;
                File.Delete(prefab.path);
                ReloadEditor();
            }
        }

        private void lbPrefabs_MouseDown(object sender, MouseEventArgs e)
        {
            if (Input.GetKey(Keys.ControlKey))
            {
                int index = lbPrefabs.IndexFromPoint(e.Location);
                if (index != ListBox.NoMatches)
                {
                    lbPrefabs.DoDragDrop(lbPrefabs.Items[index], DragDropEffects.Copy);
                }
            }
        }

        private void lbPrefabs_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(GameObject)))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void lbPrefabs_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(GameObject)))
            {
                GameObject go = (GameObject)e.Data.GetData(typeof(GameObject));

                if (Prefab.GetPrefab(go.name) == null)
                {
                    Prefab prefab = new Prefab(go, go.name);

                    prefab.SavePrefab();
                    Prefab.ClearPrefabCache();
                    Prefab.LoadAllPrefabs();
                    ReloadEditor();
                }
                else
                    MessageBox.Show("Prefab with name " + go.name + " already exists.", "ERROR", MessageBoxButtons.OK);
            }
        }
        #endregion

        #region lbSceneObjects
        private void lbSceneObjects_DoubleClick(object sender, MouseEventArgs e)
        {
            int index = lbSceneObjects.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                SelectGameobject((GameObject)lbSceneObjects.SelectedItem);
            }
        }

        private void lbSceneObjects_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete)
            {
                GameObject go = lbSceneObjects.SelectedItem as GameObject;
                if (selectedGameObject == go)
                    selectedGameObject = null;
                go.Destroy();
                ReloadEditor();
            }
        }

        private void lbSceneObjects_MouseDown(object sender, MouseEventArgs e)
        {
            if (Input.GetKey(Keys.ControlKey))
            {
                int index = lbSceneObjects.IndexFromPoint(e.Location);
                if (index != ListBox.NoMatches)
                {
                    lbSceneObjects.DoDragDrop(lbSceneObjects.Items[index], DragDropEffects.Copy);
                }
            }
        }
        #endregion

        #region menuStrip
        private void reloadAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CompleteReloadEditor();
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReloadEditor();
        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveProject();
            MessageBox.Show("Project has been saved!");
        }

        private void newSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sceneSaver.InitialDirectory = Directory.GetCurrentDirectory() + "/" + Settings.Assets.ScenePath;

            if (sceneSaver.ShowDialog(this) == DialogResult.OK)
            {
                string name = new FileInfo(sceneSaver.FileName).Name;

                if (name.ToLower().EndsWith(".scene"))
                    name = name.Remove(name.Length - 6);

                SceneData data = new SceneData(new Scene(name));
                data.SaveSceneData();

                Scene.LoadAllScenes();
                Scene.LoadScene(name);
                CompleteReloadEditor();
            }
        }

        private void openSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sceneOpener.InitialDirectory = Directory.GetCurrentDirectory() + "\\" + Settings.Assets.ScenePath;

            if (sceneOpener.ShowDialog(this) == DialogResult.OK && sceneOpener.FileName.ToLower().EndsWith(".scene"))
            {
                SceneData sceneData = new SceneData(sceneOpener.FileName, true);
                Scene.LoadScene(sceneData);
                CompleteReloadEditor();
            }
        }

        private void emptyGameObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameObject go = GameObject.Instantiate("GameObject");
            SelectGameobject(go);
            ReloadEditor();
        }

        private void editBitmapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectSprite selectSprite = new SelectSprite();
            selectSprite.ShowDialog(this);
        }

        private void editViewsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SelectView(Scene.ActiveScene).ShowDialog(this);
        }
        #endregion

        #region Object Editing
        private void SelectPrefab(Prefab prefab)
        {
            lblEditObject.Text = "Edit Prefab";
            panelEdit.Controls.Clear();
            selectedGameObject = null;
            selectedPrefab = prefab;

            Label label;
            TextBox textbox;

            label = new Label();
            label.Text = "Name:";
            label.Width = (int)(panelEdit.Width / 2.2);
            panelEdit.Controls.Add(label);

            textbox = new TextBox();
            textbox.Name = "edit-name";
            textbox.Text = prefab.name;
            textbox.Width = (int)(panelEdit.Width / 2.2);
            textbox.TextChanged += PrefabEditControlChanged;
            panelEdit.Controls.Add(textbox);

            label = new Label();
            label.Text = "Z-Index:";
            label.Width = (int)(panelEdit.Width / 2.2);
            panelEdit.Controls.Add(label);

            textbox = new TextBox();
            textbox.Name = "edit-zindex";
            textbox.Text = prefab.zIndex.ToString();
            textbox.Width = (int)(panelEdit.Width / 2.2);
            textbox.TextChanged += PrefabEditControlChanged;
            panelEdit.Controls.Add(textbox);

            foreach (var component in prefab.components)
            {
                if (component.type == typeof(Transform))
                {
                    label = new Label();
                    label.Text = component.type.Name;
                    label.Width = (int)(panelEdit.Width / 1.1);
                    panelEdit.Controls.Add(label);
                }
                else
                {
                    label = new Label();
                    label.Text = component.type.Name;
                    label.Width = (int)(panelEdit.Width / 2.2);
                    panelEdit.Controls.Add(label);

                    Button delete = new Button();
                    delete.Name = prefab.components.IndexOf(component).ToString();
                    delete.Text = "Delete Component";
                    delete.Width = (int)(panelEdit.Width / 2.2);
                    delete.Click += PrefabEditComponentDelete;
                    delete.BackColor = Color.Red;
                    panelEdit.Controls.Add(delete);
                }

                foreach (var field in component.fieldValues)
                {
                    if (!AllowedTypes.Contains(field.type) || field.hide)
                        continue;

                    label = new Label();
                    label.Text = field.name + " - " + field.type.Name;
                    label.Width = (int)(panelEdit.Width / 2.2);
                    panelEdit.Controls.Add(label);

                    switch (field.type.Name)
                    {
                        case "String":
                        case "Int32":
                        case "Single":
                        case "Vector2":
                        case "Sprite":
                            textbox = new TextBox();
                            textbox.Name = prefab.components.IndexOf(component) + "-" + field.name;
                            textbox.Text = (field.value ?? "").ToString();
                            textbox.Width = (int)(panelEdit.Width / 2.2);
                            textbox.TextChanged += PrefabEditControlChanged;
                            panelEdit.Controls.Add(textbox);
                            if (field.type.Name == "Sprite")
                                textbox.BackColor = Sprite.Exists(textbox.Text) ? Color.Green : Color.Red;
                            break;
                        case "Color":
                            Button button = new Button();
                            button.Name = prefab.components.IndexOf(component) + "-" + field.name;
                            button.Text = "";
                            button.Width = (int)(panelEdit.Width / 2.2);
                            button.Click += PrefabEditControlChanged;
                            button.BackColor = (Color)field.value;
                            panelEdit.Controls.Add(button);
                            break;
                        case "Boolean":
                            CheckBox cb = new CheckBox();
                            cb.Name = prefab.components.IndexOf(component) + "-" + field.name;
                            cb.Width = (int)(panelEdit.Width / 2.2);
                            cb.Checked = (bool)field.value;
                            cb.CheckedChanged += PrefabEditControlChanged;
                            panelEdit.Controls.Add(cb);
                            break;
                    }
                }
            }

            Button addComponent = new Button();
            addComponent.Text = "Add Component";
            addComponent.Width = (int)(panelEdit.Width / 1.1);
            addComponent.BackColor = Color.Green;
            addComponent.Click += PrefabEditComponentAdd;
            panelEdit.Controls.Add(addComponent);
        }

        private void PrefabEditComponentDelete(object sender, EventArgs e)
        {
            Button button = sender as Button;

            int index = int.Parse(button.Name);

            selectedPrefab.RemoveComponent(selectedPrefab.components[index]);

            SelectPrefab(selectedPrefab);
        }

        private void PrefabEditComponentAdd(object sender, EventArgs e)
        {
            SelectComponent selectComponent = new SelectComponent();

            selectComponent.ShowDialog(this);

            if (selectComponent.DialogResult == DialogResult.OK)
            {
                selectedPrefab.AddComponent(selectComponent.SelectedComponent);
            }

            SelectPrefab(selectedPrefab);
        }

        private void PrefabEditControlChanged(object sender, EventArgs e)
        {
            Control control = sender as Control;

            if (control.Name == "edit-name")
            {
                selectedPrefab.name = (control as TextBox).Text;
            }
            else if (control.Name == "edit-zindex")
            {
                TextBox tb = control as TextBox;

                int value;
                if (int.TryParse(tb.Text, out value))
                    selectedPrefab.zIndex = value;
                else
                    tb.Text = selectedPrefab.zIndex.ToString();
            }
            else
            {
                string[] split = control.Name.Split('-');
                int index = int.Parse(split[0]);
                var field = selectedPrefab.components[index].fieldValues.Find(n => n.name == split[1]);

                switch (field.type.Name)
                {
                    case "String":
                        field.value = (control as TextBox).Text;
                        break;
                    case "Int32":
                        TextBox tb_int = control as TextBox;

                        int value_int;
                        if (int.TryParse(tb_int.Text, out value_int))
                            field.value = value_int;
                        //else
                        //    tb_int.Text = selectedPrefab.zIndex.ToString();
                        break;
                    case "Single":
                        TextBox tb_float = control as TextBox;

                        float value_float;
                        if (float.TryParse(tb_float.Text, out value_float))
                            field.value = value_float;
                        //else
                        //    tb_float.Text = selectedPrefab.zIndex.ToString();
                        break;
                    case "Vector2":
                        TextBox tb_vector2 = control as TextBox;

                        Vector2 value_vector2;
                        if (Vector2.TryParse(tb_vector2.Text, out value_vector2))
                            field.value = value_vector2;
                        //else
                        //    tb_vector2.Text = selectedPrefab.zIndex.ToString();
                        break;
                    case "Color":
                        ColorDialog colorDialog = new ColorDialog();
                        colorDialog.Color = (Color)field.value;
                        colorDialog.ShowDialog(this);
                        field.value = colorDialog.Color;
                        control.BackColor = colorDialog.Color;
                        break;
                    case "Boolean":
                        field.value = (control as CheckBox).Checked;
                        break;
                    case "Sprite":
                        TextBox tb_sprite = control as TextBox;
                        if (Sprite.Exists(tb_sprite.Text))
                        {
                            field.value = Sprite.GetSprite(tb_sprite.Text);
                            control.BackColor = Color.Green;
                        }
                        else
                            control.BackColor = Color.Red;
                        break;
                }
            }
        }

        private void SelectGameobject(GameObject gameobject)
        {
            lblEditObject.Text = "Edit GameObject";
            panelEdit.Controls.Clear();
            selectedGameObject = gameobject;
            selectedPrefab = null;

            if (selectedGameObject.fromPrefab && Prefab.Exists(selectedGameObject.prefabName))
            {
                Button button = new Button();
                button.Text = "Reset to Prefab";
                button.Width = (int)(panelEdit.Width / 1.1);
                button.Click += GameobjectEditResetToPrefab;
                panelEdit.Controls.Add(button);

                button = new Button();
                button.Text = "Update Prefab to this";
                button.Width = (int)(panelEdit.Width / 1.1);
                button.Click += GameobjectEditUpdatePrefabToGameObject;
                panelEdit.Controls.Add(button);
            }

            Label label;
            TextBox textbox;

            label = new Label();
            label.Text = "Name:";
            label.Width = (int)(panelEdit.Width / 2.2);
            panelEdit.Controls.Add(label);

            textbox = new TextBox();
            textbox.Name = "edit-name";
            textbox.Text = gameobject.name;
            textbox.Width = (int)(panelEdit.Width / 2.2);
            textbox.TextChanged += GameobjectEditControlChanged;
            panelEdit.Controls.Add(textbox);

            label = new Label();
            label.Text = "Z-Index:";
            label.Width = (int)(panelEdit.Width / 2.2);
            panelEdit.Controls.Add(label);

            textbox = new TextBox();
            textbox.Name = "edit-zindex";
            textbox.Text = gameobject.zIndex.ToString();
            textbox.Width = (int)(panelEdit.Width / 2.2);
            textbox.TextChanged += GameobjectEditControlChanged;
            panelEdit.Controls.Add(textbox);

            foreach (var component in gameobject.GetAllComponents())
            {
                if (component.GetType() == typeof(Transform))
                {
                    label = new Label();
                    label.Text = component.GetType().Name;
                    label.Width = (int)(panelEdit.Width / 1.1);
                    panelEdit.Controls.Add(label);
                }
                else
                {
                    label = new Label();
                    label.Text = component.GetType().Name;
                    label.Width = (int)(panelEdit.Width / 2.2);
                    panelEdit.Controls.Add(label);

                    Button delete = new Button();
                    delete.Name = gameobject.GetAllComponents().IndexOf(component).ToString();
                    delete.Text = "Delete Component";
                    delete.Width = (int)(panelEdit.Width / 2.2);
                    delete.Click += GameobjectEditComponentDelete;
                    delete.BackColor = Color.Red;
                    panelEdit.Controls.Add(delete);
                }

                foreach (var field in component.GetType().GetFields(Prefab.FindFields))
                {
                    if (!AllowedTypes.Contains(field.FieldType) || !field.IsDefined(typeof(SerializeAttribute)))
                        continue;

                    label = new Label();
                    label.Text = field.Name + " - " + field.FieldType.Name;
                    label.Width = (int)(panelEdit.Width / 2.2);
                    panelEdit.Controls.Add(label);

                    switch (field.FieldType.Name)
                    {
                        case "String":
                        case "Int32":
                        case "Single":
                        case "Vector2":
                        case "Sprite":
                            textbox = new TextBox();
                            textbox.Name = gameobject.GetAllComponents().IndexOf(component) + "-" + field.Name;
                            textbox.Text = (field.GetValue(component) ?? "").ToString();
                            textbox.Width = (int)(panelEdit.Width / 2.2);
                            textbox.TextChanged += GameobjectEditControlChanged;
                            panelEdit.Controls.Add(textbox);
                            if (field.FieldType.Name == "Sprite")
                                textbox.BackColor = Sprite.Exists(textbox.Text) ? Color.Green : Color.Red;
                            break;
                        case "Color":
                            Button button = new Button();
                            button.Name = gameobject.GetAllComponents().IndexOf(component) + "-" + field.Name;
                            button.Text = "";
                            button.Width = (int)(panelEdit.Width / 2.2);
                            button.Click += GameobjectEditControlChanged;
                            button.BackColor = (Color)field.GetValue(component);
                            panelEdit.Controls.Add(button);
                            break;
                        case "Boolean":
                            CheckBox cb = new CheckBox();
                            cb.Name = gameobject.GetAllComponents().IndexOf(component) + "-" + field.Name;
                            cb.Width = (int)(panelEdit.Width / 2.2);
                            cb.Checked = (bool)field.GetValue(component);
                            cb.CheckedChanged += GameobjectEditControlChanged;
                            panelEdit.Controls.Add(cb);
                            break;
                    }
                }
            }

            Button addComponent = new Button();
            addComponent.Text = "Add Component";
            addComponent.Width = (int)(panelEdit.Width / 1.1);
            addComponent.BackColor = Color.Green;
            addComponent.Click += GameobjectEditComponentAdd;
            panelEdit.Controls.Add(addComponent);
        }

        private void GameobjectEditComponentDelete(object sender, EventArgs e)
        {
            Button button = sender as Button;

            int index = int.Parse(button.Name);

            selectedGameObject.GetComponent(index).Destroy();

            SelectGameobject(selectedGameObject);
        }

        private void GameobjectEditComponentAdd(object sender, EventArgs e)
        {
            SelectComponent selectComponent = new SelectComponent();

            selectComponent.ShowDialog(this);

            if (selectComponent.DialogResult == DialogResult.OK)
            {
                selectedGameObject.AddComponent(selectComponent.SelectedComponent);
            }

            SelectGameobject(selectedGameObject);
        }

        private void GameobjectEditResetToPrefab(object sender, EventArgs e)
        {
            selectedGameObject = selectedGameObject.ResetToPrefab();

            ReloadEditor();
        }

        private void GameobjectEditUpdatePrefabToGameObject(object sender, EventArgs e)
        {
            if (selectedGameObject.fromPrefab && Prefab.Exists(selectedGameObject.prefabName) && MessageBox.Show("This will override the current prefab and cannot be undone.\nAre you sure?", "WATCH OUT!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string path = Prefab.GetPrefab(selectedGameObject.prefabName).path;

                Prefab.DeletePrefab(selectedGameObject.prefabName);

                Prefab prefab = new Prefab(selectedGameObject, selectedGameObject.prefabName, path);

                prefab.SavePrefab();

                CompleteReloadEditor();
            }
        }

        private void GameobjectEditControlChanged(object sender, EventArgs e)
        {
            Control control = sender as Control;

            if (control.Name == "edit-name")
            {
                selectedGameObject.name = (control as TextBox).Text;
            }
            else if (control.Name == "edit-zindex")
            {
                TextBox tb = control as TextBox;

                int value;
                if (int.TryParse(tb.Text, out value))
                    selectedGameObject.zIndex = value;
                else
                    tb.Text = selectedGameObject.zIndex.ToString();
            }
            else
            {
                string[] split = control.Name.Split('-');
                int index = int.Parse(split[0]);
                var component = selectedGameObject.GetComponent(index);
                var field = component.GetType().GetFields(Prefab.FindFields).First(n => n.Name == split[1]);

                switch (field.FieldType.Name)
                {
                    case "String":
                        field.SetValue(component, (control as TextBox).Text);
                        break;
                    case "Int32":
                        TextBox tb_int = control as TextBox;

                        int value_int;
                        if (int.TryParse(tb_int.Text, out value_int))
                            field.SetValue(component, value_int);
                        //else
                        //    tb_int.Text = field.GetValue(component).ToString();
                        break;
                    case "Single":
                        TextBox tb_float = control as TextBox;

                        float value_float;
                        if (float.TryParse(tb_float.Text, out value_float))
                            field.SetValue(component, value_float);
                        //else
                        //    tb_float.Text = field.GetValue(component).ToString();
                        break;
                    case "Vector2":
                        TextBox tb_vector2 = control as TextBox;

                        Vector2 value_vector2;
                        if (Vector2.TryParse(tb_vector2.Text, out value_vector2))
                            field.SetValue(component, value_vector2);
                        //else
                        //    tb_vector2.Text = field.GetValue(component).ToString();
                        break;
                    case "Color":
                        ColorDialog colorDialog = new ColorDialog();
                        colorDialog.Color = control.BackColor;
                        colorDialog.ShowDialog(this);
                        field.SetValue(component, colorDialog.Color);
                        control.BackColor = colorDialog.Color;
                        break;
                    case "Boolean":
                        field.SetValue(component, (control as CheckBox).Checked);
                        break;
                    case "Sprite":
                        TextBox tb_sprite = control as TextBox;
                        if (Sprite.Exists(tb_sprite.Text))
                        {
                            field.SetValue(component, Sprite.GetSprite(tb_sprite.Text));
                            control.BackColor = Color.Green;
                        }
                        else
                            control.BackColor = Color.Red;
                        break;
                }
            }
        }
        #endregion

        private void btnRunCurrentScene_Click(object sender, EventArgs e)
        {
            SaveProject();
            Process game = Process.Start("Lotus.exe", Scene.ActiveScene.name);
        }

        private void btnSceneColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = btnSceneColor.BackColor;
            if (colorDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                btnSceneColor.BackColor = colorDialog.Color;
                Scene.ActiveScene.bgColor = colorDialog.Color;
            }
        }

        private void editTexturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectTextures selectedTextures = new SelectTextures();

            selectedTextures.Text = "Edit Textures";

            selectedTextures.ShowDialog(this);
        }
    }
}