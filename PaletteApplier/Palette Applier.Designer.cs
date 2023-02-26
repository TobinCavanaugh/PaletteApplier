using System.ComponentModel;

namespace PaletteApplier
{
    partial class Palette_Applier
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.openPalette = new System.Windows.Forms.Button();
            this.pbPalette = new System.Windows.Forms.PictureBox();
            this.openImage = new System.Windows.Forms.Button();
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.pbResult = new System.Windows.Forms.PictureBox();
            this.saveImage = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbPalette)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbResult)).BeginInit();
            this.SuspendLayout();
            // 
            // openPalette
            // 
            this.openPalette.Location = new System.Drawing.Point(12, 12);
            this.openPalette.Name = "openPalette";
            this.openPalette.Size = new System.Drawing.Size(218, 60);
            this.openPalette.TabIndex = 0;
            this.openPalette.Text = "Open Palette";
            this.openPalette.UseVisualStyleBackColor = true;
            // 
            // pbPalette
            // 
            this.pbPalette.Location = new System.Drawing.Point(236, 12);
            this.pbPalette.Name = "pbPalette";
            this.pbPalette.Size = new System.Drawing.Size(128, 128);
            this.pbPalette.TabIndex = 1;
            this.pbPalette.TabStop = false;
            // 
            // openImage
            // 
            this.openImage.Location = new System.Drawing.Point(12, 146);
            this.openImage.Name = "openImage";
            this.openImage.Size = new System.Drawing.Size(218, 60);
            this.openImage.TabIndex = 1;
            this.openImage.Text = "Open Image";
            this.openImage.UseVisualStyleBackColor = true;
            // 
            // pbImage
            // 
            this.pbImage.Location = new System.Drawing.Point(236, 146);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(128, 128);
            this.pbImage.TabIndex = 3;
            this.pbImage.TabStop = false;
            // 
            // pbResult
            // 
            this.pbResult.Location = new System.Drawing.Point(236, 280);
            this.pbResult.Name = "pbResult";
            this.pbResult.Size = new System.Drawing.Size(128, 128);
            this.pbResult.TabIndex = 4;
            this.pbResult.TabStop = false;
            // 
            // saveImage
            // 
            this.saveImage.Location = new System.Drawing.Point(12, 348);
            this.saveImage.Name = "saveImage";
            this.saveImage.Size = new System.Drawing.Size(218, 60);
            this.saveImage.TabIndex = 3;
            this.saveImage.Text = "Save Image";
            this.saveImage.UseVisualStyleBackColor = true;
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(12, 282);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(218, 60);
            this.refreshButton.TabIndex = 2;
            this.refreshButton.Text = "Refresh Preview";
            this.refreshButton.UseVisualStyleBackColor = true;
            // 
            // Palette_Applier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(373, 428);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.saveImage);
            this.Controls.Add(this.pbResult);
            this.Controls.Add(this.pbImage);
            this.Controls.Add(this.openImage);
            this.Controls.Add(this.pbPalette);
            this.Controls.Add(this.openPalette);
            this.Name = "Palette_Applier";
            this.Text = "Palette_Applier";
            ((System.ComponentModel.ISupportInitialize)(this.pbPalette)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbResult)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ComboBox comboBox1;

        private System.Windows.Forms.Button refreshButton;

        private System.Windows.Forms.Button openPalette;

        private System.Windows.Forms.Button saveImage;

        private System.Windows.Forms.PictureBox pbImage;
        private System.Windows.Forms.PictureBox pbResult;

        private System.Windows.Forms.Button openImage;

        private System.Windows.Forms.PictureBox pbPalette;

        #endregion
    }
}