namespace LotusEditor
{
    partial class SelectTextures
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
            this.lbTextures = new System.Windows.Forms.ListBox();
            this.btnDone = new System.Windows.Forms.Button();
            this.btnAddTextures = new System.Windows.Forms.Button();
            this.openTextures = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // lbTextures
            // 
            this.lbTextures.FormattingEnabled = true;
            this.lbTextures.Location = new System.Drawing.Point(12, 41);
            this.lbTextures.Name = "lbTextures";
            this.lbTextures.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbTextures.Size = new System.Drawing.Size(235, 160);
            this.lbTextures.TabIndex = 0;
            this.lbTextures.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lbTextures_KeyUp);
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(12, 207);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(235, 23);
            this.btnDone.TabIndex = 1;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // btnAddTextures
            // 
            this.btnAddTextures.Location = new System.Drawing.Point(12, 12);
            this.btnAddTextures.Name = "btnAddTextures";
            this.btnAddTextures.Size = new System.Drawing.Size(235, 23);
            this.btnAddTextures.TabIndex = 2;
            this.btnAddTextures.Text = "Add Textures";
            this.btnAddTextures.UseVisualStyleBackColor = true;
            this.btnAddTextures.Click += new System.EventHandler(this.btnAddTextures_Click);
            // 
            // openTextures
            // 
            this.openTextures.Filter = "Any|*.*|BMP|*.bmp|GIF|*.gif|EXIF|*.exif|JPG|*.jpg|PGN|*.png|TIFF|*.tiff";
            this.openTextures.Multiselect = true;
            this.openTextures.Title = "Select Textures";
            // 
            // SelectTextures
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 243);
            this.Controls.Add(this.btnAddTextures);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.lbTextures);
            this.Name = "SelectTextures";
            this.Text = "Select Textures";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbTextures;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.Button btnAddTextures;
        private System.Windows.Forms.OpenFileDialog openTextures;
    }
}