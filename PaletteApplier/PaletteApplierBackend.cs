using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using MoreLinq;

namespace PaletteApplier
{
    public class PaletteApplierBackend
    {
        #region Mapping

        // private static Color[] MapPaletteColorsToImageColors(Color[] palette, Color[] image)
        // {
        //     for (int i = 0; i < image.Length; i++)
        //     {
        //         for (int p = 0; p < palette.Length; p++)
        //         {
        //             image[i] = FindClosestColor(palette, image[i]);
        //         }
        //     }
        //
        //     return image;
        // }

        private static Color[] MapPaletteColorsToImageColors(Color[] palette, Color[] image, MapTypes mapType)
        {
            //Color[] result = new Color[image.Length];

            if (mapType == MapTypes.Distance)
            {
                Parallel.For(0, image.Length, i => { image[i] = FindClosestColor(palette, image[i]); });
            }

            return image;
        }


        private static float ColorAverage(Color c)
        {
            return ((float)c.R + (float)c.G + (float)c.B) / 3f;
        }

        private static Color FindClosestColor(Color[] colors, Color targetColor)
        {
            Color closestColor = colors[0];
            float closestDistance = CalculateDistance(colors[0], targetColor);

            // Parallel.For(1, colors.Length, i =>
            // {
            //     float distance = CalculateDistance(colors[i], targetColor);
            //     if (distance < closestDistance)
            //     {
            //         closestColor = colors[i];
            //         closestDistance = distance;
            //     }
            // });

            for (int i = 1; i < colors.Length; i++)
            {
                float distance = CalculateDistance(colors[i], targetColor);
                if (distance < closestDistance)
                {
                    closestColor = colors[i];
                    closestDistance = distance;
                }
            }

            return closestColor;
        }

        private static float CalculateDistance(Color color1, Color color2)
        {
            int rDifference = color1.R - color2.R;
            int gDifference = color1.G - color2.G;
            int bDifference = color1.B - color2.B;

            return (rDifference * rDifference + gDifference * gDifference + bDifference * bDifference);
        }

        #endregion

        #region File Handling

        [STAThread]
        public static (Color[], int, int, Bitmap) RequestImage(RequestTypes rq)
        {
            OpenFileDialog paletteRequest = new OpenFileDialog();

            if (rq == RequestTypes.Image)
            {
                paletteRequest.Filter = @"Image (*.png)|*.png";
            }
            else
            {
                paletteRequest.Filter = @"Image Palette(*.png;)|*.png;|Palette File(*.pal;)|*.pal;";
                //Filter = "Image Palette(*.png;)|*.png;|Palette File(*.pal;)|*.pal;"
            }

            //Failed to open file
            if (paletteRequest.ShowDialog() != DialogResult.OK)
            {
                return (null, 0, 0, null);
            }

            string filPath = paletteRequest.FileName;

            if (filPath.EndsWith(".pal"))
            {
                return GetPaletteFromPal(rq, filPath);
            }
            else
            {
                return GetPaletteFromImage(rq, filPath);
            }
        }

        private static (Color[], int, int, Bitmap) GetPaletteFromPal(RequestTypes rq, string filPath)
        {
            string[] lines = File.ReadAllLines(filPath);

            List<Color> colors = new List<Color>();

            Bitmap bitmap = new Bitmap(Int32.Parse(lines[2]), 1, PixelFormat.Format32bppArgb);


            for (int i = 3; i < lines.Length; i++)
            {
                Color c = ParseLine(lines[i]);
                bitmap.SetPixel(i - 3, 0, c);
                colors.Add(c);
            }

            return (colors.ToArray(), bitmap.Width, bitmap.Height, bitmap);
        }

        private static Color ParseLine(string line)
        {
            string[] vals = line.Split(' ');

            Color c = Color.FromArgb(Int32.Parse(vals[0]), Int32.Parse(vals[1]), Int32.Parse(vals[2]));

            //Including alpha
            if (vals.Length == 3)
            {
                //c = Color.FromArgb(Int32.Parse(vals[3]), Int32.Parse(vals[0]), Int32.Parse(vals[0]), Int32.Parse(vals[0]));
            }

            return c;
        }

        private static (Color[], int, int, Bitmap) GetPaletteFromImage(RequestTypes rq, string filPath)
        {
            Bitmap bitmap = new Bitmap(filPath);

            List<Color> paletteColors = new List<Color>();


            //File is not a pal file
            //Loop thru all the pixels
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    var pixCol = bitmap.GetPixel(x, y);

                    //Palettes only need one instance of the color
                    if (rq == RequestTypes.Palette)
                    {
                        if (!paletteColors.Contains(pixCol))
                        {
                            paletteColors.Add(pixCol);
                        }
                    }
                    //Images need all the colors
                    else if (rq == RequestTypes.Image)
                    {
                        paletteColors.Add(pixCol);
                    }
                }
            }

            return (paletteColors.ToArray(), bitmap.Width, bitmap.Height, bitmap);
        }

        #endregion

        private static unsafe Bitmap CopyColorArrayToBitmap(Color[] colors, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height);

            // for (int y = 0; y < height; y++)
            // {
            //     for (int x = 0; x < width; x++)
            //     {
            //         int index = y * width + x;
            //         bitmap.SetPixel(x, y, colors[index]);
            //     }
            // }


            var dat = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb);
            var ptr = (int*)dat.Scan0;

            for (int i = 0; i < colors.Length; i++)
            {
                *ptr++ = (colors[i].A << 24) | (colors[i].R << 16) | (colors[i].G << 8) | colors[i].B;
            }

            bitmap.UnlockBits(dat);

            return bitmap;
        }

        public static void SaveImage((Color[], int, int, Bitmap) paletteColors, (Color[], int, int, Bitmap) imageColors, MapTypes mapType)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = "test";


            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog.FileName;


                var bm = GetProcessedBitmap(paletteColors, imageColors, mapType);

                bm.Save(fileName, ImageFormat.Png);

                Console.WriteLine("Saved");
            }
        }

        public static Bitmap GetProcessedBitmap((Color[], int, int, Bitmap) paletteColors,
            (Color[], int, int, Bitmap) imageColors, MapTypes mapType)
        {
            var newCols = MapPaletteColorsToImageColors(paletteColors.Item1, imageColors.Item1, mapType);
            return CopyColorArrayToBitmap(newCols, imageColors.Item2, imageColors.Item3);
        }
    }

    public enum RequestTypes
    {
        Image,
        Palette
    }

    public enum MapTypes
    {
        
        Distance
    }
}