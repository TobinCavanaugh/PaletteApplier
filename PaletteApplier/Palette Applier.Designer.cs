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
            this.bOpenPalette = new System.Windows.Forms.Button();
            this.pbPalette = new System.Windows.Forms.PictureBox();
            this.bOpenImage = new System.Windows.Forms.Button();
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.pbResult = new System.Windows.Forms.PictureBox();
            this.bSaveImage = new System.Windows.Forms.Button();
            this.bRefreshPreview = new System.Windows.Forms.Button();
            this.llPreview = new System.Windows.Forms.Label();
            this.bBatchOpen = new System.Windows.Forms.Button();
            this.bSaveBatch = new System.Windows.Forms.Button();
            this.llLoadedAmount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbPalette)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbResult)).BeginInit();
            this.SuspendLayout();
            // 
            // bOpenPalette
            // 
            this.bOpenPalette.Location = new System.Drawing.Point(12, 12);
            this.bOpenPalette.Name = "bOpenPalette";
            this.bOpenPalette.Size = new System.Drawing.Size(218, 60);
            this.bOpenPalette.TabIndex = 0;
            this.bOpenPalette.Text = "Open Palette";
            this.bOpenPalette.UseVisualStyleBackColor = true;
            // 
            // pbPalette
            // 
            this.pbPalette.Location = new System.Drawing.Point(236, 12);
            this.pbPalette.Name = "pbPalette";
            this.pbPalette.Size = new System.Drawing.Size(128, 128);
            this.pbPalette.TabIndex = 1;
            this.pbPalette.TabStop = false;
            // 
            // bOpenImage
            // 
            this.bOpenImage.Location = new System.Drawing.Point(12, 146);
            this.bOpenImage.Name = "bOpenImage";
            this.bOpenImage.Size = new System.Drawing.Size(218, 60);
            this.bOpenImage.TabIndex = 1;
            this.bOpenImage.Text = "Open Image";
            this.bOpenImage.UseVisualStyleBackColor = true;
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
            // bSaveImage
            // 
            this.bSaveImage.Location = new System.Drawing.Point(12, 348);
            this.bSaveImage.Name = "bSaveImage";
            this.bSaveImage.Size = new System.Drawing.Size(218, 60);
            this.bSaveImage.TabIndex = 3;
            this.bSaveImage.Text = "Save Image";
            this.bSaveImage.UseVisualStyleBackColor = true;
            // 
            // bRefreshPreview
            // 
            this.bRefreshPreview.Location = new System.Drawing.Point(370, 285);
            this.bRefreshPreview.Name = "bRefreshPreview";
            this.bRefreshPreview.Size = new System.Drawing.Size(218, 60);
            this.bRefreshPreview.TabIndex = 2;
            this.bRefreshPreview.Text = "Refresh Preview";
            this.bRefreshPreview.UseVisualStyleBackColor = true;
            // 
            // llPreview
            // 
            this.llPreview.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.llPreview.Location = new System.Drawing.Point(370, 348);
            this.llPreview.Name = "llPreview";
            this.llPreview.Size = new System.Drawing.Size(218, 60);
            this.llPreview.TabIndex = 5;
            this.llPreview.Text = "*Preview images are downscaled";
            this.llPreview.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bBatchOpen
            // 
            this.bBatchOpen.Location = new System.Drawing.Point(370, 12);
            this.bBatchOpen.Name = "bBatchOpen";
            this.bBatchOpen.Size = new System.Drawing.Size(218, 60);
            this.bBatchOpen.TabIndex = 6;
            this.bBatchOpen.Text = "Open Image Folder (For Batch)";
            this.bBatchOpen.UseVisualStyleBackColor = true;
            // 
            // bSaveBatch
            // 
            this.bSaveBatch.Location = new System.Drawing.Point(370, 146);
            this.bSaveBatch.Name = "bSaveBatch";
            this.bSaveBatch.Size = new System.Drawing.Size(218, 60);
            this.bSaveBatch.TabIndex = 7;
            this.bSaveBatch.Text = "Save Images (For Batch)";
            this.bSaveBatch.UseVisualStyleBackColor = true;
            // 
            // llLoadedAmount
            // 
            this.llLoadedAmount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.llLoadedAmount.Location = new System.Drawing.Point(370, 75);
            this.llLoadedAmount.Name = "llLoadedAmount";
            this.llLoadedAmount.Size = new System.Drawing.Size(218, 60);
            this.llLoadedAmount.TabIndex = 8;
            this.llLoadedAmount.Text = "No images loaded";
            this.llLoadedAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Palette_Applier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(604, 428);
            this.Controls.Add(this.llLoadedAmount);
            this.Controls.Add(this.bSaveBatch);
            this.Controls.Add(this.bBatchOpen);
            this.Controls.Add(this.llPreview);
            this.Controls.Add(this.bRefreshPreview);
            this.Controls.Add(this.bSaveImage);
            this.Controls.Add(this.pbResult);
            this.Controls.Add(this.pbImage);
            this.Controls.Add(this.bOpenImage);
            this.Controls.Add(this.pbPalette);
            this.Controls.Add(this.bOpenPalette);
            this.Name = "Palette_Applier";
            this.Text = "Palette_Applier";
            ((System.ComponentModel.ISupportInitialize)(this.pbPalette)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbResult)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label llLoadedAmount;

        private System.Windows.Forms.Button bSaveBatch;

        private System.Windows.Forms.Button bBatchOpen;

        private System.Windows.Forms.Label llPreview;

        private System.Windows.Forms.ComboBox comboBox1;

        private System.Windows.Forms.Button bRefreshPreview;

        private System.Windows.Forms.Button bOpenPalette;

        private System.Windows.Forms.Button bSaveImage;

        private System.Windows.Forms.PictureBox pbImage;
        private System.Windows.Forms.PictureBox pbResult;

        private System.Windows.Forms.Button bOpenImage;

        private System.Windows.Forms.PictureBox pbPalette;

        #endregion
    }
}