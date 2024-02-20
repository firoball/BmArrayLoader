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
                Printer.PrintImage(loader.Width, loader.Height, loader.IndexData, loader.Palette);
            }
        }
    }
}
