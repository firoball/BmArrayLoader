using System;
using System.IO;

namespace BmArrayLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            LoaderSelector ls = new LoaderSelector();
            ls.RegisterLoader(new PcxLoader());
            ls.RegisterLoader(new LbmLoader());

            char sl = Path.DirectorySeparatorChar;

            Loader loader;
            Tileset tileset;
            string file;

            file = ".." + sl + ".." + sl + "bitmaps" + sl + "michels.lbm";
            loader = ls.GetLoader(file);
            if (loader.Load(file, out tileset))
            {
                Console.WriteLine("Load file of type " + loader.GetType() + ": "+ file);
                Printer.PrintImage(tileset.Master, tileset.Palette);
                int tileIdx;
                tileIdx = tileset.GetTile(520, 72, 110, 120);
                Printer.PrintImage(tileset.Tiles[tileIdx], tileset.Palette);
                tileIdx = tileset.GetTile(64, 192, 64, 64);
                Printer.PrintImage(tileset.Tiles[tileIdx], tileset.Palette);
                tileIdx = tileset.GetTile(64, 192, 64, 64);
                Printer.PrintImage(tileset.Tiles[tileIdx], tileset.Palette);
            }
            
            file = ".." + sl + ".." + sl + "bitmaps" + sl + "floortex.pcx";
            loader = ls.GetLoader(file);
            if (loader.Load(file, out tileset))
            {
                Console.WriteLine("Load file of type " + loader.GetType() + ": "+ file);
                Printer.PrintImage(tileset.Master, tileset.Palette);
                int tileIdx;
                tileIdx = tileset.GetTile(0, 0, 64, 64);
                Printer.PrintImage(tileset.Tiles[tileIdx], tileset.Palette);
                tileIdx = tileset.GetTile(64, 64, 64, 64);
                Printer.PrintImage(tileset.Tiles[tileIdx], tileset.Palette);
                tileIdx = tileset.GetTile(128, 0, 64, 64);
                Printer.PrintImage(tileset.Tiles[tileIdx], tileset.Palette);
            }
        }
    }
}
