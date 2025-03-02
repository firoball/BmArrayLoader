//#define DEBUG_CONSOLE
using System.Collections.Generic;
#if DEBUG_CONSOLE
using System;
using System.Runtime.InteropServices;
using System.Drawing;
#endif

namespace BmArrayLoader
{
    public static class Printer
    {
#if DEBUG_CONSOLE
        [DllImport("kernel32.dll", EntryPoint = "GetConsoleWindow", SetLastError = true)]
        private static extern IntPtr GetConsoleHandle();
#endif

        static public void PrintImage(Indexmap master, List<byte[]> palette, int offsetX, int offsetY)
        {
#if DEBUG_CONSOLE
            Bitmap bitmap = new Bitmap(master.Width, master.Height);
            for (int i = 0; i < master.Data.Length; i++)
            {
                int index = master.Data[i];
                Color color = Color.FromArgb(palette[index][0], palette[index][1], palette[index][2]);
                bitmap.SetPixel(i % master.Width, i / master.Width, color);
            }
            var handler = GetConsoleHandle();
            using (var graphics = Graphics.FromHwnd(handler))
            {
                using (bitmap)
                {
                    graphics.DrawImage(bitmap, offsetX, offsetY, bitmap.Width, bitmap.Height);
                }
            }
#endif
        }
    }
}
