using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;

namespace BmArrayLoader
{
    static class Printer
    {
        [DllImport("kernel32.dll", EntryPoint = "GetConsoleWindow", SetLastError = true)]
        private static extern IntPtr GetConsoleHandle();

        static public void PrintImage(int width, int height, byte[] indexData, List<byte[]> palette)
        {
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(width, height);
            for (int i = 0; i < indexData.Length; i++)
            {
                int index = indexData[i];
                Color color = Color.FromArgb(palette[index][0], palette[index][1], palette[index][2]);
                bitmap.SetPixel(i % width, i / width, color);
            }
            var handler = GetConsoleHandle();
            using (var graphics = Graphics.FromHwnd(handler))
            {
                using (bitmap)
                {
                    graphics.DrawImage(bitmap, 50, 50, bitmap.Width, bitmap.Height);
                }
            }
        }
    }
}
