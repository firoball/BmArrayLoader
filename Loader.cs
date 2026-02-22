using System;
using System.IO;

namespace BmArrayLoader
{
    public abstract class Loader
    {
        protected byte[] m_pattern;
        protected string m_type;
        protected byte[] m_bytes;
        protected Tileset m_tileset;
        
        public Loader()
        {
            m_type = string.Empty;
        }

        public bool Load(string fileName, out Tileset tileset)
        {
            bool loaded;
            try
            {
                m_bytes = File.ReadAllBytes(fileName);
                m_tileset = new Tileset();
                loaded = LoadInternal();
            }
            catch
            {
                loaded = false;
                Console.WriteLine("Loader ERROR: Loading " + fileName + " failed!");
            }
            tileset = loaded ? m_tileset : null;
            
            return loaded;
        }

        public bool Accepts(string fileName)
        {
            bool retval = false;

            if (m_pattern == null)
                return retval;
            
            byte[] block = new byte[m_pattern.Length];
            try
            {
                using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    int bytes = stream.Read(block, 0, m_pattern.Length);
                    if (bytes == m_pattern.Length & block.SequenceEqual(m_pattern))
                        retval = true;
                    stream.Close();
                }
            }
            catch
            {
                retval = false; //can't open file, therefore is unsupported anyway - don't throw anything at user
            } 
 
            return retval;
        }

        public string GetFileType()
        {
            return m_type;
        }

        protected virtual bool LoadInternal()
        {
            Console.WriteLine("Loader ERROR: internal load routine not implemented");
            return false;
        }
    }
}
