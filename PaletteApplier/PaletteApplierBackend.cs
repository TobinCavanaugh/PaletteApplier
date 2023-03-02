using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MoreLinq;

namespace PaletteApplier
{
    /// <summary>
    /// All the backend for the image processing, if I were smarter this would be broken up into tasks, but i also did this in an afternoon... so there
    /// </summary>
    public class PaletteApplierBackend
    {
        /// <summary>
        /// Returns the image array of colors after processing them
        /// </summary>
        /// <param name="palette">The palette that should be applied to the image</param>
        /// <param name="image">The colors of the image</param>
        /// <param name="mapType">The type of map, currently only one exists</param>
        /// <returns></returns>
        private static Color[] MapPaletteColorsToImageColors(Color[] palette, Color[] image, MapTypes mapType)
        {
            if (mapType == MapTypes.Distance)
            {
                //Sets each color in image to its nearest value on the palette
                Parallel.For(0, image.Length, i => { image[i] = FindClosestColor(palette, image[i]); });
            }

            return image;
        }


        /// <summary>
        /// Returns the nearest color in colors to the target color
        /// </summary>
        /// <param name="colors">The colors to be checked</param>
        /// <param name="targetColor">The color to be found</param>
        /// <returns></returns>
        private static Color FindClosestColor(Color[] colors, Color targetColor)
        {
            //Precalculate the closest
            Color closestColor = colors[0];
            float closestDistance = CalculateDistance(colors[0], targetColor);

            //Search through them all and get the closest distance
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

        /// <summary>
        /// Returns the distance between the two colors
        /// </summary>
        /// <param name="color1"></param>
        /// <param name="color2"></param>
        /// <returns></returns>
        private static float CalculateDistance(Color color1, Color color2)
        {
            int rDifference = color1.R - color2.R;
            int gDifference = color1.G - color2.G;
            int bDifference = color1.B - color2.B;

            return (rDifference * rDifference + gDifference * gDifference + bDifference * bDifference);
        }


        /// <summary>
        /// Request an image
        /// </summary>
        /// <param name="rq">The type of image being requested</param>
        /// <returns>A tuple of the image colors, the width, the height, and the bitmap</returns>
        [STAThread]
        public static ImageFile RequestImage(RequestTypes rq, bool doDownscale = false)
        {
            OpenFileDialog paletteRequest = new OpenFileDialog();

            if (rq == RequestTypes.Image)
            {
                paletteRequest.Filter = @"Image (*.png)|*.png";
            }
            else
            {
                //This allows for choosing either pal files or png files
                paletteRequest.Filter = @"Image Palette(*.png;)|*.png;|Palette File(*.pal;)|*.pal;";
                //Filter = "Image Palette(*.png;)|*.png;|Palette File(*.pal;)|*.pal;"
            }

            //Failed to open file, or file wasnt chosen
            if (paletteRequest.ShowDialog() != DialogResult.OK)
            {
                return new ImageFile();
            }


            string filPath = paletteRequest.FileName;

            //This isnt ideal, but it works
            if (filPath.EndsWith(".pal"))
            {
                return GetPaletteFromPal(filPath);
            }
            else
            {
                return GetFromImage(rq, filPath);
            }
        }

        /// <summary>
        /// Gets a .pal file in data we can use
        /// </summary>
        /// <param name="filPath">The path of the .pal file</param>
        /// <returns>A tuple of the image colors, the width, the height, and the bitmap</returns>
        private static ImageFile GetPaletteFromPal(string filPath)
        {
            /*
             * Example .pal file (line numbers added for clarity)
             *
             *  JASC-PAL // I guess this declares that its a pal?
             *  0100 //Genuinely I dont know
             *  3 //This is the number of colors in the palette
             *  255 0 255 0 // I have to assume that the palette goes RGBA when it has 4 valyes
             *  17 17 17 // The rest of the values are always RGB
             *  34 34 34
             *  ...
             */

            //Luckily we can read the PAL file as cleartext
            string[] lines = File.ReadAllLines(filPath);
            List<Color> colors = new List<Color>();

            //We also make a bitmap with a width of the given value in the PAL and a height of 1, just like a regular palette
            Bitmap bitmap = new Bitmap(Int32.Parse(lines[2]), 1, PixelFormat.Format32bppArgb);


            //We skip the first three lines
            for (int i = 3; i < lines.Length; i++)
            {
                //Parse each individual line
                Color c = ParsePalLine(lines[i]);

                //Subtract, because we are offset by 3
                bitmap.SetPixel(i - 3, 0, c);
                colors.Add(c);
            }

            var s = filPath.Split('/');
            var name = s[s.Length - 1];

            return new ImageFile(colors.ToArray(), bitmap.Width, bitmap.Height, bitmap, name);
            //return (colors.ToArray(), bitmap.Width, bitmap.Height, bitmap, name);
        }

        /// <summary>
        /// Parses a line of PAL data
        /// </summary>
        /// <param name="line">A line of PAL data, it has to be RGBA</param>
        /// <returns></returns>
        private static Color ParsePalLine(string line)
        {
            //Split at the spaces
            string[] vals = line.Split(' ');

            //Make a color by parsing each split
            Color c = Color.FromArgb(Int32.Parse(vals[0]), Int32.Parse(vals[1]), Int32.Parse(vals[2]));

            return c;
        }

        /// <summary>
        /// Gets a palette or an image from an image at filPath
        /// </summary>
        /// <param name="rq">The request type, should it be read as a palette or as an image</param>
        /// <param name="filPath">The location of the image</param>
        /// <returns></returns>
        public static ImageFile GetFromImage(RequestTypes rq, string filPath)
        {
            Bitmap bitmap = new Bitmap(filPath);

            List<Color> paletteColors = new List<Color>();

            //Loop thru all the pixels
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    //Getting pixel is NOT ideal, this should be changed, but the alternative is scary bitwise stuff
                    var pixCol = bitmap.GetPixel(x, y);

                    //Palettes only need one instance of the color
                    if (rq == RequestTypes.Palette)
                    {
                        //Apparently this really sucks, im not sure what the fix is, besides just telling people to use low bit palettes
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

            var s = filPath.Split('/');
            var name = s[s.Length - 1];

            Console.WriteLine(paletteColors.ToArray().Length);

            return new ImageFile(paletteColors.ToArray(), bitmap.Width, bitmap.Height, bitmap, name);

            //return (paletteColors.ToArray(), bitmap.Width, bitmap.Height, bitmap, name);
        }

        /// <summary>
        /// Recalculates the image and opens the save file dialogue
        /// </summary>
        /// <param name="paletteColors">The palette colors used to color the image</param>
        /// <param name="imageColors">All the colors of the image</param>
        /// <param name="mapType">The type of processing we do</param>
        public static void SaveImage(ImageFile paletteColors, ImageFile imageColors,
            MapTypes mapType, string fileLocation = "")
        {
            if (fileLocation != "")
            {
                //Grab the processed bitmap
                var bm = GetProcessedBitmap(paletteColors, imageColors, mapType);

                GC.Collect();
                
                Console.WriteLine(fileLocation);
                //Save that to the harddrive, if we wanted another image format we would put it here
                
                bm.Save(fileLocation, ImageFormat.Png);
                
                Console.WriteLine("Saved");

                return;
            }

            //Currently this is set to only save as PNG, however adding JPG etc support is easy
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            //TODO this should be changed to use the initial file name
            saveFileDialog.FileName = "test";


            //If the dialogue worked
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog.FileName;

                //Grab the processed bitmap
                var bm = GetProcessedBitmap(paletteColors, imageColors, mapType);

                //Save that to the harddrive, if we wanted another image format we would put it here
                bm.Save(fileName, ImageFormat.Png);
                Console.WriteLine("Saved");
            }
        }

        /// <summary>
        /// Creates and returns a new bitmap, colored by colors
        /// </summary>
        /// <param name="colors">All the colors of the image</param>
        /// <param name="width">The width of the bitmap</param>
        /// <param name="height">The height of the bitmap</param>
        /// <returns></returns>
        public static unsafe Bitmap CopyColorArrayToBitmap(Color[] colors, int width, int height)
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

            //Lock the bitmap
            var dat = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb);
            var ptr = (int*)dat.Scan0;

            for (int i = 0; i < colors.Length; i++)
            {
                //These bitwise offsets are specifically for Format32bppArgb, which i chose on a whim, so dont change this unless you know what you are doing
                *ptr++ = (colors[i].A << 24) | (colors[i].R << 16) | (colors[i].G << 8) | colors[i].B;
            }

            // Parallel.For(0, colors.Length, i =>
            // {
            //     //These bitwise offsets are specifically for Format32bppArgb, which i chose on a whim, so dont change this unless you know what you are doing
            //     *ptr++ = (colors[i].A << 24) | (colors[i].R << 16) | (colors[i].G << 8) | colors[i].B;
            //
            // });

            //Unlock it
            bitmap.UnlockBits(dat);

            return bitmap;
        }

        public static (Color[], int, int) DownscaleColorArray(Color[] oldPixels, int oldWidth, int oldHeight)
        {
            int newWidth = oldWidth / 2;
            int newHeight = oldHeight / 2;

            Color[] newPixels = new Color[newWidth * newHeight];

            for (int y = 0; y < newHeight; y += 1)
            {
                for (int x = 0; x < newWidth; x += 1)
                {
                    // Compute the index of the pixel in the old image
                    int oldIndex = (y * 2) * (oldWidth) + (x * 2);

                    // Compute the index of the pixel in the new image
                    int newIndex = y * newWidth + x;

                    newPixels[newIndex] = oldPixels[oldIndex];
                }
            }

            oldWidth /= 2;
            oldHeight /= 2;


            Console.WriteLine($"{newWidth} {newHeight}");

            if ((oldWidth * oldHeight) <= (DownscaleMaxWidth * DownscaleMaxWidth))
            {
                return (newPixels, oldWidth, oldHeight);
            }

            return DownscaleColorArray(newPixels, oldWidth, oldHeight);
        }

        private static int DownscaleMaxWidth = 128;

        /// <summary>
        /// Processes and returns the bitmap
        /// </summary>
        /// <param name="paletteColors">The palette colors to be used in coloring the image</param>
        /// <param name="imageColors">The image to be colored</param>
        /// <param name="mapType">The type of processing that is to be done</param>
        /// <returns></returns>
        public static Bitmap GetProcessedBitmap(ImageFile paletteColors,
            ImageFile imageColors, MapTypes mapType, bool doDownscale = false)
        {
            Color[] newCols;

            if (doDownscale)
            {
                if ((imageColors.width * imageColors.height) >= (DownscaleMaxWidth * DownscaleMaxWidth))
                {
                    var img = DownscaleColorArray(imageColors.colors, imageColors.width, imageColors.height);

                    imageColors.colors = img.Item1;
                    imageColors.width = img.Item2;
                    imageColors.height = img.Item3;
                }
            }

            Console.WriteLine(
                $"Width = {imageColors.width}, Height = {imageColors.height}, pixels = {imageColors.colors.Length}");

            newCols = MapPaletteColorsToImageColors(paletteColors.colors, imageColors.colors, mapType);

            return CopyColorArrayToBitmap(newCols, imageColors.width, imageColors.height);
        }
    }
}

public struct ImageFile
{
    public Color[] colors;
    public int width;
    public int height;
    public Bitmap bitMap;
    public string fileName;

    public ImageFile(Color[] colors, int width, int height, Bitmap bitMap, string fileName) : this()
    {
        this.colors = colors;
        this.width = width;
        this.height = height;
        this.bitMap = bitMap;
        this.fileName = fileName;
    }
}

public enum RequestTypes
{
    Image,
    Palette
}

/// <summary>
/// The type of processing to be done, if you want to add things, add another enum value and do stuff in MapPaletteColorsToImageColors
/// </summary>
public enum MapTypes
{
    Distance
}