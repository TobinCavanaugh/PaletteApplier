using System;

namespace PaletteApplier
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Palette_Applier paletteApplier = new Palette_Applier();

            paletteApplier.ShowDialog();
        }
    }
}