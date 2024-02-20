using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BmArrayLoader
{
    abstract class Loader
    {
        protected bool m_loaded;
        protected byte[] m_indexData;
        protected List<byte[]> m_paletteData;
        protected byte[] m_bytes;
        private List<Bitmap> m_bitmaps;

        public List<Bitmap> Bitmaps { get => m_bitmaps; }
        public List<byte[]> Palette { get => m_paletteData; }

        public Loader()
        {
            m_paletteData = new List<byte[]>(256);
            m_bitmaps = new List<Bitmap>();
            m_loaded = false;
        }

        public virtual bool Load(string fileName)
        {
            try
            {
                m_bytes = File.ReadAllBytes(fileName);
                m_loaded = true;
            }
            catch
            {
                m_loaded = false;
                Console.WriteLine("Loader ERROR: Loading " + fileName + " failed!");
            }
            return m_loaded;
        }

        public int GetBitmap(int offsetX, int offsetY, int width, int height)
        {
            int offsetX0 = offsetX - 1;
            int offsetY0 = offsetY - 1;
            int sizeX = offsetX0 + width;
            int sizeY = offsetY0 + height;

            if (m_loaded &&
                sizeX > 0 && sizeY > 0 &&
                width > 0 && height > 0 &&
                sizeX * sizeY < m_indexData.Length)
            {
                Bitmap bitmap = new Bitmap(8, width, height);
                int srcIdx = 0;
                int tgtIdx = 0;
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        srcIdx = sizeX * (offsetY0 + y) + offsetX0 + x;
                        bitmap.Data[tgtIdx] = m_indexData[srcIdx];
                        tgtIdx++;
                    }
                }
                m_bitmaps.Add(bitmap);
                return m_bitmaps.Count - 1;
            }
            else
                return -1;
        }
    }
}
