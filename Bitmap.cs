using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmArrayLoader
{
    class Bitmap
    {
        private int m_format;
        private int m_width;
        private int m_height;
        private byte[] m_data;

        public Bitmap(int format, int width, int height)
        {
            m_format = format;
            m_width = width;
            m_height = height;
            m_data = new byte[width * height];
        }

        public int Format { get => m_format; }
        public int Width { get => m_width; }
        public int Height { get => m_height; }
        public byte[] Data { get => m_data; }
    }
}
