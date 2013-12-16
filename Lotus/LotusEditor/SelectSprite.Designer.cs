namespace LotusEditor
{
    partial class SelectSprite
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
            this.lbSprites = new System.Windows.Forms.ListBox();
            this.btnAddSprite = new System.Windows.Forms.Button();
            this.btnDeleteSprite = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbSprites
            // 
            this.lbSprites.FormattingEnabled = true;
            this.lbSprites.Location = new System.Drawing.Point(12, 12);
            this.lbSprites.Name = "lbSprites";
            this.lbSprites.Size = new System.Drawing.Size(240, 186);
            this.lbSprites.TabIndex = 0;
            this.lbSprites.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbSprites_MouseDoubleClick);
            // 
            // btnAddSprite
            // 
            this.btnAddSprite.Location = new System.Drawing.Point(12, 204);
            this.btnAddSprite.Name = "btnAddSprite";
            this.btnAddSprite.Size = new System.Drawing.Size(117, 23);
            this.btnAddSprite.TabIndex = 1;
            this.btnAddSprite.Text = "Add Sprite";
            this.btnAddSprite.UseVisualStyleBackColor = true;
            this.btnAddSprite.Click += new System.EventHandler(this.btnAddSprite_Click);
            // 
            // btnDeleteSprite
            // 
            this.btnDeleteSprite.Location = new System.Drawing.Point(135, 204);
            this.btnDeleteSprite.Name = "btnDeleteSprite";
            this.btnDeleteSprite.Size = new System.Drawing.Size(117, 23);
            this.btnDeleteSprite.TabIndex = 2;
            this.btnDeleteSprite.Text = "Delete Sprite";
            this.btnDeleteSprite.UseVisualStyleBackColor = true;
            this.btnDeleteSprite.Click += new System.EventHandler(this.btnDeleteSprite_Click);
            // 
            // SelectSprite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(263, 239);
            this.Controls.Add(this.btnDeleteSprite);
            this.Controls.Add(this.btnAddSprite);
            this.Controls.Add(this.lbSprites);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SelectSprite";
            this.Text = "Edit Sprites";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbSprites;
        private System.Windows.Forms.Button btnAddSprite;
        private System.Windows.Forms.Button btnDeleteSprite;
    }
}