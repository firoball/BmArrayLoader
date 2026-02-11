//#define DEBUG_CONSOLE
using System.Collections.Generic;
#if DEBUG_CONSOLE
using System;
using System.Text;
#endif

namespace BmArrayLoader
{
    public static class Printer
    {

        public static void PrintImage(Indexmap master, List<byte[]> palette)
        {
#if DEBUG_CONSOLE
            Console.OutputEncoding = Encoding.UTF8;
            for (int i = 0; i < master.Data.Length; i++)
            {
                int index = master.Data[i];
                int x = i % master.Width;
                if (x < Console.BufferWidth)
                {
                    string color = GetAnsiColor(palette[index][0], palette[index][1], palette[index][2]);
                    Console.Write(color + "█");
                }
                if (x== master.Width - 1) Console.Write("\x1b[0m\n");
            }
            Console.Write("\x1b[0m\n");
#endif
        }

#if DEBUG_CONSOLE
        static string GetAnsiColor(byte r, byte g, byte b)
        {
            // 24-bit Truecolor ANSI
            return $"\x1b[38;2;{r};{g};{b}m";
        }
#endif
    }
}
