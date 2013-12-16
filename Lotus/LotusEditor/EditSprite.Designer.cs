namespace LotusEditor
{
    partial class EditSprite
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
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.lbImages = new System.Windows.Forms.ListBox();
            this.nudSpriteFps = new System.Windows.Forms.NumericUpDown();
            this.lblFps = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSpriteName = new System.Windows.Forms.TextBox();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.btnSaveSprite = new System.Windows.Forms.Button();
            this.btnAddImage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpriteFps)).BeginInit();
            this.SuspendLayout();
            // 
            // pbImage
            // 
            this.pbImage.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pbImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbImage.Location = new System.Drawing.Point(217, 64);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(198, 176);
            this.pbImage.TabIndex = 0;
            this.pbImage.TabStop = false;
            // 
            // lbImages
            // 
            this.lbImages.FormattingEnabled = true;
            this.lbImages.Location = new System.Drawing.Point(12, 64);
            this.lbImages.Name = "lbImages";
            this.lbImages.Size = new System.Drawing.Size(199, 147);
            this.lbImages.TabIndex = 1;
            this.lbImages.SelectedIndexChanged += new System.EventHandler(this.lbImages_SelectedIndexChanged);
            this.lbImages.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbImages_KeyDown);
            // 
            // nudSpriteFps
            // 
            this.nudSpriteFps.DecimalPlaces = 2;
            this.nudSpriteFps.Location = new System.Drawing.Point(287, 38);
            this.nudSpriteFps.Name = "nudSpriteFps";
            this.nudSpriteFps.Size = new System.Drawing.Size(128, 20);
            this.nudSpriteFps.TabIndex = 2;
            // 
            // lblFps
            // 
            this.lblFps.AutoSize = true;
            this.lblFps.Location = new System.Drawing.Point(216, 40);
            this.lblFps.Name = "lblFps";
            this.lblFps.Size = new System.Drawing.Size(57, 13);
            this.lblFps.TabIndex = 3;
            this.lblFps.Text = "Sprite FPS";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(216, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Sprite Name";
            // 
            // txtSpriteName
            // 
            this.txtSpriteName.Location = new System.Drawing.Point(287, 12);
            this.txtSpriteName.Name = "txtSpriteName";
            this.txtSpriteName.Size = new System.Drawing.Size(128, 20);
            this.txtSpriteName.TabIndex = 5;
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Location = new System.Drawing.Point(12, 217);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(92, 23);
            this.btnMoveUp.TabIndex = 6;
            this.btnMoveUp.Text = "Move Up";
            this.btnMoveUp.UseVisualStyleBackColor = true;
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Location = new System.Drawing.Point(110, 217);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(101, 23);
            this.btnMoveDown.TabIndex = 7;
            this.btnMoveDown.Text = "Move Down";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnSaveSprite
            // 
            this.btnSaveSprite.Location = new System.Drawing.Point(15, 247);
            this.btnSaveSprite.Name = "btnSaveSprite";
            this.btnSaveSprite.Size = new System.Drawing.Size(400, 23);
            this.btnSaveSprite.TabIndex = 8;
            this.btnSaveSprite.Text = "Save Sprite";
            this.btnSaveSprite.UseVisualStyleBackColor = true;
            this.btnSaveSprite.Click += new System.EventHandler(this.btnSaveSprite_Click);
            // 
            // btnAddImage
            // 
            this.btnAddImage.Location = new System.Drawing.Point(15, 12);
            this.btnAddImage.Name = "btnAddImage";
            this.btnAddImage.Size = new System.Drawing.Size(195, 46);
            this.btnAddImage.TabIndex = 9;
            this.btnAddImage.Text = "Add Image";
            this.btnAddImage.UseVisualStyleBackColor = true;
            this.btnAddImage.Click += new System.EventHandler(this.btnAddImage_Click);
            // 
            // EditSprite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 281);
            this.Controls.Add(this.btnAddImage);
            this.Controls.Add(this.btnSaveSprite);
            this.Controls.Add(this.btnMoveDown);
            this.Controls.Add(this.btnMoveUp);
            this.Controls.Add(this.txtSpriteName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblFps);
            this.Controls.Add(this.nudSpriteFps);
            this.Controls.Add(this.lbImages);
            this.Controls.Add(this.pbImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "EditSprite";
            this.Text = "Edit Sprite";
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpriteFps)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbImage;
        private System.Windows.Forms.ListBox lbImages;
        private System.Windows.Forms.NumericUpDown nudSpriteFps;
        private System.Windows.Forms.Label lblFps;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSpriteName;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.Button btnSaveSprite;
        private System.Windows.Forms.Button btnAddImage;
    }
}