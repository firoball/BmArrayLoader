namespace BmArrayLoader
{
    public class Indexmap
    {
        private readonly int m_offsetX;
        private readonly int m_offsetY;
        private readonly int m_width;
        private readonly int m_height;
        private readonly byte[] m_data;

        public Indexmap(int width, int height) : this(0, 0, width, height) { }
        
        public Indexmap(int offsetX, int offsetY, int width, int height)
        {
            m_offsetX = offsetX;
            m_offsetY = offsetY;
            m_width = width;
            m_height = height;
            m_data = new byte[width * height];
        }

        public byte this[int i]
        {
            get => (i >= 0 && i < m_data.Length) ? m_data[i] : (byte)0;
            set
            {
                if (i >= 0 && i < m_data.Length)
                    m_data[i] = value;
            }
        }
        
        public byte this[int x, int y]
        {
            get => this[y * m_width + x];
            set => this[y * m_width + x] = value;
        }
        
        public int Width { get => m_width; }
        public int Height { get => m_height; }
        public int Size { get => m_data.Length; }

        public bool Equals(int offsetX, int offsetY, int width, int height)
        {
            if (offsetX == m_offsetX && offsetY == m_offsetY &&
                width == m_width && height == m_height)
                return true;
            else
                return false;
        }
    }
}
