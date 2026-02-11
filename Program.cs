using System.IO;

namespace BmArrayLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            Loader loader;
            loader = new LbmLoader();
            char sl = Path.DirectorySeparatorChar;
            if (loader.Load(@".."+sl+".."+sl+"bitmaps"+sl+"michels.lbm"))
            {
                Printer.PrintImage(loader.Master, loader.Palette);
                int tileIdx;
                tileIdx = loader.GetTile(520, 72, 110, 120);
                Printer.PrintImage(loader.Tiles[tileIdx], loader.Palette);
                tileIdx = loader.GetTile(64, 192, 64, 64);
                Printer.PrintImage(loader.Tiles[tileIdx], loader.Palette);
                tileIdx = loader.GetTile(64, 192, 64, 64);
                Printer.PrintImage(loader.Tiles[tileIdx], loader.Palette);
            }
            loader = new PcxLoader();
            if (loader.Load(@".."+sl+".."+sl+"bitmaps"+sl+"floortex.pcx"))
            {
                Printer.PrintImage(loader.Master, loader.Palette);
                int tileIdx;
                tileIdx = loader.GetTile(0, 0, 64, 64);
                Printer.PrintImage(loader.Tiles[tileIdx], loader.Palette);
                tileIdx = loader.GetTile(64, 64, 64, 64);
                Printer.PrintImage(loader.Tiles[tileIdx], loader.Palette);
                tileIdx = loader.GetTile(128, 0, 64, 64);
                Printer.PrintImage(loader.Tiles[tileIdx], loader.Palette);
            }
        }
    }
}
