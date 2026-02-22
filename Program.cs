using System;
using System.Collections.Generic;

namespace BmArrayLoader
{
    public class Program
    {
        private static void Main(string[] args)
        {
            List<Tuple<string, List<string>>> fileInfos;
            fileInfos = ParseArgs(args);

            /*
            char sl = Path.DirectorySeparatorChar;
            string[] fakeargs = new string[14];
            fakeargs[0] = ".." + sl + ".." + sl + "bitmaps" + sl + "michels.lbm";
            fakeargs[1] = "-t";
            fakeargs[2] = "520|72|110|120";
            fakeargs[3] = "-t";
            fakeargs[4] = "64|192|64|64";
            fakeargs[5] = "-t";
            fakeargs[6] = "64|192|64|64";

            fakeargs[7] = ".." + sl + ".." + sl + "bitmaps" + sl + "floortex.pcx";
            fakeargs[8] = "-t";
            fakeargs[9] = "0|0|64|64";
            fakeargs[10] = "-t";
            fakeargs[11] = "64|64|64|64";
            fakeargs[12] = "-t";
            fakeargs[13] = "128|0|64|64";
            fileInfos = ParseArgs(fakeargs);
            */
            
            if (fileInfos != null)
            {
                foreach (var info in fileInfos)
                {
                    IList<int[]> dimensions = ParseDimensions(info.Item2);
                    if (LoadTileset(info.Item1, dimensions, out Tileset tileset))
                        PrintTileset(tileset);
                }
            }
        }

        private static List<Tuple<string, List<string>>> ParseArgs(string[] args)
        {
            bool parseFile = true;
            List<Tuple<string, List<string>>> fileInfos = new List<Tuple<string, List<string>>>();
            Tuple<string, List<string>> fileInfo = null;
                
            if ((args.Length == 0) || 
                args.Contains("-h",  StringComparer.OrdinalIgnoreCase) ||
                args.Contains("--help",  StringComparer.OrdinalIgnoreCase))
            {
                ShowHelp();
                return null;
            }

            foreach (string arg in args)
            {
                if (string.Equals(arg, "-t", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(arg, "--tile", StringComparison.OrdinalIgnoreCase))
                {
                    parseFile = false;
                }
                else if (arg.StartsWith(("-")))
                {
                    Console.WriteLine("ERROR: Invalid argumebnt: " + arg);
                    ShowHelp();
                    return null;
                }
                else
                {
                    if (parseFile) //arg is a file
                    {
                        if (fileInfo != null) //store away previous fileInfo
                            fileInfos.Add(fileInfo);

                        //start another fileInfo
                        fileInfo = new Tuple<string, List<string>>(arg, new List<string>());
                    }
                    else //arg is a dimension
                    {
                        if (fileInfo != null /*&& !fileInfo.Item2.Contains(arg)*/) //add dimension to current fileInfo
                            fileInfo.Item2.Add(arg);
                        parseFile = true;
                    }
                }
            }

            if (fileInfo != null) //store away previous fileInfo
                fileInfos.Add(fileInfo);

            return fileInfos;
        }

        private static  void ShowHelp()
        {
            Console.WriteLine($"Usage: {AppDomain.CurrentDomain.FriendlyName} " +
                              $"<imagefile> [-t <x>|<y>|<width>|<height>] [...] " +
                              $"[<imagefile> [-t <x>|<y>|<width>|<height>] [...]] [...]");
            Console.WriteLine($"Options:");
            Console.WriteLine($"-h, --help                    Show help");
            Console.WriteLine($"-t <x>|<y>|<width>|<height>   Create subset tile at x/y with given width and height in pixels");
        }
        
        private static IList<int[]> ParseDimensions(IList<string> dimensions)
        {
            List<int[]> intDimensions = new List<int[]>();
            
            foreach (string dimension in dimensions)
            {
                bool valid = false;
                int[] values = new int[4];
                string[] defs = dimension.Split('|');
                
                if (defs.Length == 4)
                {
                    valid = true;
                    for (int i = 0; i < 4; i++)
                    {
                        valid = valid && int.TryParse(defs[i], out values[i]);
                    }
                }
                
                if (valid)
                    intDimensions.Add(values);
                else
                    Console.WriteLine("WARNING: ignoring invalid parameter: " + defs);
            }

            return intDimensions;
        }
        
        private static bool LoadTileset(string file, IList<int[]> dimensions, out Tileset tileset)
        {
            tileset = null;
            Loader loader = BmArrayLoader.GetLoader(file);
            if (loader != null)
            {
                Console.WriteLine("Load file of type " + loader.GetFileType() + ": " + file);

                if (loader.Load(file, out tileset))
                {
                    foreach (int[] dimension in dimensions)
                    {
                        if (!tileset.AddTile(dimension[0], dimension[1], dimension[2], dimension[3]))
                        {
                            Console.WriteLine("WARNING: Could not add tile: x{0} y{1} w{2} h{3}",
                                dimension[0], dimension[1], dimension[2], dimension[3]);
                        }
                    }

                    return true;
                }
            }

            return false;
        }
        
        private static void PrintTileset(Tileset tileset)
        {
            foreach (Indexmap tile in tileset.Tiles)
                Printer.PrintImage(tile, tileset.Palette);
        }
    }
}
