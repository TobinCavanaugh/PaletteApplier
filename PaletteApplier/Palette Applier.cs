using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MoreLinq;
using Ookii.Dialogs.Wpf;

namespace PaletteApplier
{
    public partial class Palette_Applier : Form
    {
        private ImageFile Palette;
        private ImageFile Image;

        private MapTypes MapType = MapTypes.Distance;

        private Color darkest = ColorTranslator.FromHtml("#233142");
        private Color dark = ColorTranslator.FromHtml("#455D7A");
        private Color light = ColorTranslator.FromHtml("#F95959");
        private Color lightest = ColorTranslator.FromHtml("#FACF5A");

        public Palette_Applier()
        {
            InitializeComponent();

            #region Actions

            this.bOpenPalette.Click += (sender, args) => { OpenPalette(); };

            this.bOpenImage.Click += (sender, args) => { OpenImage(); };

            this.bSaveImage.Click += (sender, args) => { SaveImage(); };

            this.bRefreshPreview.Click += (sender, args) => { RefreshPreview(); };

            this.bBatchOpen.Click += (sender, args) => { LoadImagesBatch(); };
            this.bSaveBatch.Click += (sender, args) => { SaveImagesBatch(); };

            this.bOpenPalette.MouseEnter += (sender, args) => MouseOver(bOpenPalette);
            this.bOpenPalette.MouseLeave += (sender, args) => MouseExit(bOpenPalette);

            this.bOpenImage.MouseEnter += (sender, args) => MouseOver(bOpenImage);
            this.bOpenImage.MouseLeave += (sender, args) => MouseExit(bOpenImage);

            this.bSaveImage.MouseEnter += (sender, args) => MouseOver(bSaveImage);
            this.bSaveImage.MouseLeave += (sender, args) => MouseExit(bSaveImage);

            this.bRefreshPreview.MouseEnter += (sender, args) => MouseOver(bRefreshPreview);
            this.bRefreshPreview.MouseLeave += (sender, args) => MouseExit(bRefreshPreview);

            this.bBatchOpen.MouseEnter += (sender, args) => MouseOver(bRefreshPreview);
            this.bBatchOpen.MouseLeave += (sender, args) => MouseExit(bRefreshPreview);


            //this.ddFunctionType.SelectedIndexChanged += (sender, args) => ChangeFunctionType();

            #endregion

            //The Designer was being janky in jetbrains so i had to do this stuff manually

            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            //Set the picture box stretch modes
            this.pbPalette.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pbImage.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pbResult.SizeMode = PictureBoxSizeMode.StretchImage;

            BackColor = darkest;

            //Set the button colors
            SetColors(bOpenPalette, dark, lightest);
            SetColors(bOpenImage, dark, lightest);
            SetColors(bSaveImage, dark, lightest);
            SetColors(bRefreshPreview, dark, lightest);


            llPreview.ForeColor = lightest;
            llLoadedAmount.ForeColor = lightest;

            //SetColors(lbPreview, dark, lightest);
        }


        private ImageFile[] BatchImages;


        private void LoadImagesBatch()
        {
            VistaFolderBrowserDialog folderBrowserDialog = new VistaFolderBrowserDialog();

            if (!folderBrowserDialog.ShowDialog().Value)
            {
                Console.WriteLine("failed");
                return;
            }

            string directoryPath = folderBrowserDialog.SelectedPath;
            string[] pngFiles = Directory.GetFiles(directoryPath)
                .Where(f => (f.Contains(".bmp") || f.Contains(".jpg") || f.Contains(".png"))).ToArray();

            llLoadedAmount.Text = $"{pngFiles.Length} images found and loaded!";

            BatchImages = new ImageFile[pngFiles.Length];

            for (int i = 0; i < pngFiles.Length; i++)
            {
                BatchImages[i] = PaletteApplierBackend.GetFromImage(RequestTypes.Image, pngFiles[i]);

                var split = pngFiles[i].Split('/');
                BatchImages[i].fileName = split[split.Length - 1];
            }

            Console.WriteLine(BatchImages.Length);
        }

        private void SaveImagesBatch()
        {
            if (Palette.bitMap == null)
            {
                var mb = MessageBox.Show("No Palette is assigned");
                
                return;
            }
            
            
            VistaFolderBrowserDialog saveFolderBrowserDialogue = new VistaFolderBrowserDialog();

            if (!saveFolderBrowserDialogue.ShowDialog().Value)
            {
                Console.WriteLine("failed");
                return;
            }

            string directoryPath = saveFolderBrowserDialogue.SelectedPath;
            

            if (BatchImages.Length > 0)
            {
                BatchImages.ForEach(x =>
                {
                    Console.WriteLine(directoryPath + x.fileName);
                    PaletteApplierBackend.SaveImage(Palette, x, MapTypes.Distance, directoryPath + x.fileName);
                });
            }
        }

        private void MouseOver(Button b)
        {
            b.BackColor = darkest;
        }

        private void MouseExit(Button b)
        {
            b.BackColor = dark;
        }

        private void SetColors(Control b, Color _dark, Color _lightest)
        {
            b.BackColor = _dark;
            b.ForeColor = _lightest;
        }


        /// <summary>
        /// Opens the file open dialogue and allows for an palette to be opened
        /// </summary>
        private void OpenPalette()
        {
            var p = PaletteApplierBackend.RequestImage(RequestTypes.Palette);

            //We do this check so that if the user decides against opening the palette, it doesnt remove it
            if (p.bitMap != null)
            {
                Palette = p;
                this.pbPalette.Image = p.bitMap;
            }
        }

        /// <summary>
        /// Opens the file open dialogue and allows for an image to be opened
        /// </summary>
        private void OpenImage()
        {
            var p = PaletteApplierBackend.RequestImage(RequestTypes.Image);

            Console.WriteLine(p.colors.Length);

            //We do this check so that if the user decides against opening the image, it doesnt remove it
            if (p.bitMap != null)
            {
                Console.WriteLine("WAdawdawdadwdwa");
                var ds = PaletteApplierBackend.DownscaleColorArray(p.colors, p.bitMap.Width, p.bitMap.Height);

                var bm = PaletteApplierBackend.CopyColorArrayToBitmap(ds.Item1, ds.Item2, ds.Item3);

                Image = p;
                this.pbImage.Image = bm;
            }
            else
            {
                Console.WriteLine("no bitmap");
            }
        }

        /// <summary>
        /// Opens the file open dialogue and allows for the processed image to be saved
        /// </summary>
        private void SaveImage()
        {
            if (IsReadyForProcessing())
            {
                PaletteApplierBackend.SaveImage(Palette, Image, MapType);
            }
        }

        /// <summary>
        /// Refreshes the preview, if i knew how, this is where i would downscale the bitmap before processing it
        /// </summary>
        private void RefreshPreview()
        {
            if (IsReadyForProcessing())
            {
                this.pbResult.Image = PaletteApplierBackend.GetProcessedBitmap(Palette, Image, MapType, true);
            }
        }

        /// <summary>
        /// Returns true if the image is ready to be processed
        /// </summary>
        /// <returns></returns>
        private bool IsReadyForProcessing()
        {
            return (Palette.bitMap != null && Image.bitMap != null);
        }
    }
}