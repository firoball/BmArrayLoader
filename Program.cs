using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmArrayLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            LbmLoader loader = new LbmLoader();
            if (loader.Load(@"..\..\bitmaps\michels.lbm"))
            {
                Printer.PrintImage(loader.Master, loader.Palette, 50, 50);
                int tileIdx;
                tileIdx = loader.GetTile(520, 72, 110, 120);
                Printer.PrintImage(loader.Tiles[tileIdx], loader.Palette, 50, 320);
                tileIdx = loader.GetTile(64, 192, 64, 64);
                Printer.PrintImage(loader.Tiles[tileIdx], loader.Palette, 180, 320);
                tileIdx = loader.GetTile(64, 192, 64, 64);
                Printer.PrintImage(loader.Tiles[tileIdx], loader.Palette, 250, 320);
            }
        }
    }
}
