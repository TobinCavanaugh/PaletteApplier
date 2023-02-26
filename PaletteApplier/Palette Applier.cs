using System.Drawing;
using System.Windows.Forms;

namespace PaletteApplier
{
    public partial class Palette_Applier : Form
    {
        private (Color[], int, int, Bitmap) PaletteTuple;
        private (Color[], int, int, Bitmap) ImageTuple;
        private MapTypes MapType = MapTypes.Distance;


        private Color darkest = ColorTranslator.FromHtml("#233142");
        private Color dark = ColorTranslator.FromHtml("#455D7A");
        private Color light = ColorTranslator.FromHtml("#F95959");
        private Color lightest = ColorTranslator.FromHtml("#FACF5A");

        public Palette_Applier()
        {
            InitializeComponent();

            #region Actions

            this.openPalette.Click += (sender, args) => { OpenPalette(); };

            this.openImage.Click += (sender, args) => { OpenImage(); };

            this.saveImage.Click += (sender, args) => { SaveImage(); };

            this.refreshButton.Click += (sender, args) => { RefreshPreview(); };


            this.openPalette.MouseEnter += (sender, args) => MouseOver(openPalette);
            this.openPalette.MouseLeave += (sender, args) => MouseExit(openPalette);

            this.openImage.MouseEnter += (sender, args) => MouseOver(openImage);
            this.openImage.MouseLeave += (sender, args) => MouseExit(openImage);

            this.saveImage.MouseEnter += (sender, args) => MouseOver(saveImage);
            this.saveImage.MouseLeave += (sender, args) => MouseExit(saveImage);

            this.refreshButton.MouseEnter += (sender, args) => MouseOver(refreshButton);
            this.refreshButton.MouseLeave += (sender, args) => MouseExit(refreshButton);

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
            SetColors(openPalette, dark, lightest);
            SetColors(openImage, dark, lightest);
            SetColors(saveImage, dark, lightest);
            SetColors(refreshButton, dark, lightest);
        }

        private void MouseOver(Button b)
        {
            b.BackColor = darkest;
        }

        private void MouseExit(Button b)
        {
            b.BackColor = dark;
        }

        private void SetColors(Button b, Color _dark, Color _lightest)
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
            if (p.Item4 != null)
            {
                PaletteTuple = p;
                this.pbPalette.Image = p.Item4;
            }
        }

        /// <summary>
        /// Opens the file open dialogue and allows for an image to be opened
        /// </summary>
        private void OpenImage()
        {
            var p = PaletteApplierBackend.RequestImage(RequestTypes.Image);

            //We do this check so that if the user decides against opening the image, it doesnt remove it
            if (p.Item4 != null)
            {
                ImageTuple = p;
                this.pbImage.Image = p.Item4;
            }
        }

        /// <summary>
        /// Opens the file open dialogue and allows for the processed image to be saved
        /// </summary>
        private void SaveImage()
        {
            if (IsReadyForProcessing())
            {
                PaletteApplierBackend.SaveImage(PaletteTuple, ImageTuple, MapType);
            }
        }

        /// <summary>
        /// Refreshes the preview, if i knew how, this is where i would downscale the bitmap before processing it
        /// </summary>
        private void RefreshPreview()
        {
            if (IsReadyForProcessing())
            {
                this.pbResult.Image = PaletteApplierBackend.GetProcessedBitmap(PaletteTuple, ImageTuple, MapType);
            }
        }

        /// <summary>
        /// Returns true if the image is ready to be processed
        /// </summary>
        /// <returns></returns>
        private bool IsReadyForProcessing()
        {
            return (PaletteTuple.Item4 != null && ImageTuple.Item4 != null);
        }
    }
}