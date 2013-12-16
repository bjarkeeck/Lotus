namespace LotusEditor
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.openGLControl = new SharpGL.OpenGLControl();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openSceneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.emptyGameobjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editBitmapsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editTexturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editViewsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timerGameLoop = new System.Windows.Forms.Timer(this.components);
            this.lbSceneObjects = new System.Windows.Forms.ListBox();
            this.lbPrefabs = new System.Windows.Forms.ListBox();
            this.lblEditObject = new System.Windows.Forms.Label();
            this.lblSceneObjects = new System.Windows.Forms.Label();
            this.lblPrefabs = new System.Windows.Forms.Label();
            this.panelEdit = new System.Windows.Forms.FlowLayoutPanel();
            this.sceneSaver = new System.Windows.Forms.SaveFileDialog();
            this.sceneOpener = new System.Windows.Forms.OpenFileDialog();
            this.btnRunCurrentScene = new System.Windows.Forms.Button();
            this.btnSceneColor = new System.Windows.Forms.Button();
            this.cbDrawGUI = new System.Windows.Forms.CheckBox();
            this.lblViewPosition = new System.Windows.Forms.Label();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetZoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // openGLControl
            // 
            this.openGLControl.BitDepth = 24;
            this.openGLControl.DrawFPS = true;
            this.openGLControl.FrameRate = 200;
            this.openGLControl.Location = new System.Drawing.Point(12, 56);
            this.openGLControl.Name = "openGLControl";
            this.openGLControl.RenderContextType = SharpGL.RenderContextType.NativeWindow;
            this.openGLControl.Size = new System.Drawing.Size(789, 650);
            this.openGLControl.TabIndex = 0;
            this.openGLControl.TabStop = false;
            this.openGLControl.OpenGLInitialized += new System.EventHandler(this.openGLControl_OpenGLInitialized);
            this.openGLControl.OpenGLDraw += new System.Windows.Forms.PaintEventHandler(this.openGLControl_OpenGLDraw);
            this.openGLControl.MouseEnter += new System.EventHandler(this.openGLControl_MouseEnter);
            this.openGLControl.MouseLeave += new System.EventHandler(this.openGLControl_MouseLeave);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1211, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reloadAllToolStripMenuItem,
            this.reloadToolStripMenuItem,
            this.saveProjectToolStripMenuItem,
            this.newSceneToolStripMenuItem,
            this.openSceneToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // reloadAllToolStripMenuItem
            // 
            this.reloadAllToolStripMenuItem.Name = "reloadAllToolStripMenuItem";
            this.reloadAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.R)));
            this.reloadAllToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.reloadAllToolStripMenuItem.Text = "Complete Reload";
            this.reloadAllToolStripMenuItem.Click += new System.EventHandler(this.reloadAllToolStripMenuItem_Click);
            // 
            // reloadToolStripMenuItem
            // 
            this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            this.reloadToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.reloadToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.reloadToolStripMenuItem.Text = "Reload";
            this.reloadToolStripMenuItem.Click += new System.EventHandler(this.reloadToolStripMenuItem_Click);
            // 
            // saveProjectToolStripMenuItem
            // 
            this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            this.saveProjectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveProjectToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.saveProjectToolStripMenuItem.Text = "Save";
            this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.saveProjectToolStripMenuItem_Click);
            // 
            // newSceneToolStripMenuItem
            // 
            this.newSceneToolStripMenuItem.Name = "newSceneToolStripMenuItem";
            this.newSceneToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newSceneToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.newSceneToolStripMenuItem.Text = "New Scene";
            this.newSceneToolStripMenuItem.Click += new System.EventHandler(this.newSceneToolStripMenuItem_Click);
            // 
            // openSceneToolStripMenuItem
            // 
            this.openSceneToolStripMenuItem.Name = "openSceneToolStripMenuItem";
            this.openSceneToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openSceneToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.openSceneToolStripMenuItem.Text = "Open Scene";
            this.openSceneToolStripMenuItem.Click += new System.EventHandler(this.openSceneToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createToolStripMenuItem,
            this.editBitmapsToolStripMenuItem,
            this.editTexturesToolStripMenuItem,
            this.editViewsToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // createToolStripMenuItem
            // 
            this.createToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.emptyGameobjectToolStripMenuItem});
            this.createToolStripMenuItem.Name = "createToolStripMenuItem";
            this.createToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.createToolStripMenuItem.Text = "Create";
            // 
            // emptyGameobjectToolStripMenuItem
            // 
            this.emptyGameobjectToolStripMenuItem.Name = "emptyGameobjectToolStripMenuItem";
            this.emptyGameobjectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.emptyGameobjectToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.emptyGameobjectToolStripMenuItem.Text = "Empty Gameobject";
            this.emptyGameobjectToolStripMenuItem.Click += new System.EventHandler(this.emptyGameObjectToolStripMenuItem_Click);
            // 
            // editBitmapsToolStripMenuItem
            // 
            this.editBitmapsToolStripMenuItem.Name = "editBitmapsToolStripMenuItem";
            this.editBitmapsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.editBitmapsToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.editBitmapsToolStripMenuItem.Text = "Edit Sprites";
            this.editBitmapsToolStripMenuItem.Click += new System.EventHandler(this.editBitmapsToolStripMenuItem_Click);
            // 
            // editTexturesToolStripMenuItem
            // 
            this.editTexturesToolStripMenuItem.Name = "editTexturesToolStripMenuItem";
            this.editTexturesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.editTexturesToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.editTexturesToolStripMenuItem.Text = "Edit Textures";
            this.editTexturesToolStripMenuItem.Click += new System.EventHandler(this.editTexturesToolStripMenuItem_Click);
            // 
            // editViewsToolStripMenuItem
            // 
            this.editViewsToolStripMenuItem.Name = "editViewsToolStripMenuItem";
            this.editViewsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.editViewsToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.editViewsToolStripMenuItem.Text = "Edit Views";
            this.editViewsToolStripMenuItem.Click += new System.EventHandler(this.editViewsToolStripMenuItem_Click);
            // 
            // timerGameLoop
            // 
            this.timerGameLoop.Interval = 1;
            this.timerGameLoop.Tick += new System.EventHandler(this.timerGameLoop_Tick);
            // 
            // lbSceneObjects
            // 
            this.lbSceneObjects.FormattingEnabled = true;
            this.lbSceneObjects.Location = new System.Drawing.Point(807, 481);
            this.lbSceneObjects.Name = "lbSceneObjects";
            this.lbSceneObjects.Size = new System.Drawing.Size(193, 225);
            this.lbSceneObjects.TabIndex = 2;
            this.lbSceneObjects.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbSceneObjects_MouseDown);
            // 
            // lbPrefabs
            // 
            this.lbPrefabs.AllowDrop = true;
            this.lbPrefabs.FormattingEnabled = true;
            this.lbPrefabs.Location = new System.Drawing.Point(1006, 481);
            this.lbPrefabs.Name = "lbPrefabs";
            this.lbPrefabs.Size = new System.Drawing.Size(193, 225);
            this.lbPrefabs.TabIndex = 3;
            this.lbPrefabs.DragDrop += new System.Windows.Forms.DragEventHandler(this.lbPrefabs_DragDrop);
            this.lbPrefabs.DragEnter += new System.Windows.Forms.DragEventHandler(this.lbPrefabs_DragEnter);
            this.lbPrefabs.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbPrefabs_MouseDown);
            // 
            // lblEditObject
            // 
            this.lblEditObject.AutoSize = true;
            this.lblEditObject.Location = new System.Drawing.Point(807, 37);
            this.lblEditObject.Name = "lblEditObject";
            this.lblEditObject.Size = new System.Drawing.Size(59, 13);
            this.lblEditObject.TabIndex = 5;
            this.lblEditObject.Text = "Edit Object";
            // 
            // lblSceneObjects
            // 
            this.lblSceneObjects.AutoSize = true;
            this.lblSceneObjects.Location = new System.Drawing.Point(807, 465);
            this.lblSceneObjects.Name = "lblSceneObjects";
            this.lblSceneObjects.Size = new System.Drawing.Size(77, 13);
            this.lblSceneObjects.TabIndex = 6;
            this.lblSceneObjects.Text = "Scene Objects";
            // 
            // lblPrefabs
            // 
            this.lblPrefabs.AutoSize = true;
            this.lblPrefabs.Location = new System.Drawing.Point(1003, 465);
            this.lblPrefabs.Name = "lblPrefabs";
            this.lblPrefabs.Size = new System.Drawing.Size(43, 13);
            this.lblPrefabs.TabIndex = 7;
            this.lblPrefabs.Text = "Prefabs";
            // 
            // panelEdit
            // 
            this.panelEdit.AutoScroll = true;
            this.panelEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelEdit.Location = new System.Drawing.Point(810, 56);
            this.panelEdit.Name = "panelEdit";
            this.panelEdit.Size = new System.Drawing.Size(389, 406);
            this.panelEdit.TabIndex = 8;
            // 
            // sceneSaver
            // 
            this.sceneSaver.DefaultExt = "scene";
            this.sceneSaver.FileName = "newScene";
            this.sceneSaver.Filter = "Scene file|*.scene";
            // 
            // sceneOpener
            // 
            this.sceneOpener.DefaultExt = "scene";
            this.sceneOpener.Filter = "Scene file|*.scene";
            this.sceneOpener.Title = "Choose a scene to load";
            // 
            // btnRunCurrentScene
            // 
            this.btnRunCurrentScene.Location = new System.Drawing.Point(12, 27);
            this.btnRunCurrentScene.Name = "btnRunCurrentScene";
            this.btnRunCurrentScene.Size = new System.Drawing.Size(115, 23);
            this.btnRunCurrentScene.TabIndex = 9;
            this.btnRunCurrentScene.Text = "Run Current Scene";
            this.btnRunCurrentScene.UseVisualStyleBackColor = true;
            this.btnRunCurrentScene.Click += new System.EventHandler(this.btnRunCurrentScene_Click);
            // 
            // btnSceneColor
            // 
            this.btnSceneColor.Location = new System.Drawing.Point(133, 27);
            this.btnSceneColor.Name = "btnSceneColor";
            this.btnSceneColor.Size = new System.Drawing.Size(94, 23);
            this.btnSceneColor.TabIndex = 10;
            this.btnSceneColor.Text = "Scene Color";
            this.btnSceneColor.UseVisualStyleBackColor = true;
            this.btnSceneColor.Click += new System.EventHandler(this.btnSceneColor_Click);
            // 
            // cbDrawGUI
            // 
            this.cbDrawGUI.AutoSize = true;
            this.cbDrawGUI.Location = new System.Drawing.Point(233, 31);
            this.cbDrawGUI.Name = "cbDrawGUI";
            this.cbDrawGUI.Size = new System.Drawing.Size(73, 17);
            this.cbDrawGUI.TabIndex = 11;
            this.cbDrawGUI.Text = "Draw GUI";
            this.cbDrawGUI.UseVisualStyleBackColor = true;
            // 
            // lblViewPosition
            // 
            this.lblViewPosition.AutoSize = true;
            this.lblViewPosition.Location = new System.Drawing.Point(331, 32);
            this.lblViewPosition.Name = "lblViewPosition";
            this.lblViewPosition.Size = new System.Drawing.Size(22, 13);
            this.lblViewPosition.TabIndex = 12;
            this.lblViewPosition.Text = "0;0";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetZoomToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // resetZoomToolStripMenuItem
            // 
            this.resetZoomToolStripMenuItem.Name = "resetZoomToolStripMenuItem";
            this.resetZoomToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Z)));
            this.resetZoomToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.resetZoomToolStripMenuItem.Text = "Reset Zoom";
            this.resetZoomToolStripMenuItem.Click += new System.EventHandler(this.resetZoomToolStripMenuItem_Click);
            // 
            // MainWindow
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1211, 718);
            this.Controls.Add(this.lblViewPosition);
            this.Controls.Add(this.cbDrawGUI);
            this.Controls.Add(this.btnSceneColor);
            this.Controls.Add(this.btnRunCurrentScene);
            this.Controls.Add(this.panelEdit);
            this.Controls.Add(this.lblPrefabs);
            this.Controls.Add(this.lblSceneObjects);
            this.Controls.Add(this.lblEditObject);
            this.Controls.Add(this.lbPrefabs);
            this.Controls.Add(this.lbSceneObjects);
            this.Controls.Add(this.openGLControl);
            this.Controls.Add(this.menuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainWindow";
            this.Text = "Lotus Editor v1.0";
            this.Activated += new System.EventHandler(this.MainWindow_Activated);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainWindow_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainWindow_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SharpGL.OpenGLControl openGLControl;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.Timer timerGameLoop;
        private System.Windows.Forms.ListBox lbSceneObjects;
        private System.Windows.Forms.ListBox lbPrefabs;
        private System.Windows.Forms.Label lblEditObject;
        private System.Windows.Forms.Label lblSceneObjects;
        private System.Windows.Forms.Label lblPrefabs;
        private System.Windows.Forms.FlowLayoutPanel panelEdit;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newSceneToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog sceneSaver;
        private System.Windows.Forms.ToolStripMenuItem openSceneToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog sceneOpener;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem emptyGameobjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editBitmapsToolStripMenuItem;
        private System.Windows.Forms.Button btnRunCurrentScene;
        private System.Windows.Forms.ToolStripMenuItem editViewsToolStripMenuItem;
        private System.Windows.Forms.Button btnSceneColor;
        private System.Windows.Forms.CheckBox cbDrawGUI;
        private System.Windows.Forms.ToolStripMenuItem editTexturesToolStripMenuItem;
        private System.Windows.Forms.Label lblViewPosition;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetZoomToolStripMenuItem;
    }
}

