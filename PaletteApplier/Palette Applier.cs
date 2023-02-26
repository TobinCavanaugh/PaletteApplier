using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MoreLinq;

namespace PaletteApplier
{
    public partial class Palette_Applier : Form
    {
        public (Color[], int, int, Bitmap) PaletteTuple;
        public (Color[], int, int, Bitmap) ImageTuple;
        public MapTypes MapType = MapTypes.Distance;


        Color darkest = ColorTranslator.FromHtml("#233142");
        Color dark = ColorTranslator.FromHtml("#455D7A");
        Color light = ColorTranslator.FromHtml("#F95959");
        Color lightest = ColorTranslator.FromHtml("#FACF5A");

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

            this.FormBorderStyle = FormBorderStyle.FixedDialog;


            this.pbPalette.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pbImage.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pbResult.SizeMode = PictureBoxSizeMode.StretchImage;


            BackColor = darkest;


            SetColors(openPalette, dark, lightest);
            SetColors(openImage, dark, lightest);
            SetColors(saveImage, dark, lightest);
            SetColors(refreshButton, dark, lightest);
            
            //this.ddFunctionType.BackColor = dark;
            //this.ddFunctionType.ForeColor = lightest;


            //ddFunctionType.Items.Clear();
            //Enum.GetNames(typeof(MapTypes)).ForEach(x =>
            //{
            //    ddFunctionType.Items.Add(x);
            //});
        }

        private void ChangeFunctionType()
        {
            //int index = ddFunctionType.SelectedIndex;
            //MapType = (MapTypes)index;
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


        private void OpenPalette()
        {
            var p = PaletteApplierBackend.RequestImage(RequestTypes.Palette);
            if (p.Item4 != null)
            {
                PaletteTuple = p;
                this.pbPalette.Image = p.Item4;
            }
        }

        private void OpenImage()
        {
            var p = PaletteApplierBackend.RequestImage(RequestTypes.Image);

            if (p.Item4 != null)
            {
                ImageTuple = p;
                this.pbImage.Image = p.Item4;
            }
        }

        private void SaveImage()
        {
            if (IsReadyForProcessing())
            {
                PaletteApplierBackend.SaveImage(PaletteTuple, ImageTuple, MapType);
            }
        }

        private void RefreshPreview()
        {
            if (IsReadyForProcessing())
            {
                this.pbResult.Image = PaletteApplierBackend.GetProcessedBitmap(PaletteTuple, ImageTuple, MapType);
            }
        }

        private bool IsReadyForProcessing()
        {
            return (PaletteTuple.Item4 != null && ImageTuple.Item4 != null);
        }
    }
}