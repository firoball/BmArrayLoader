using System;
using System.Collections.Generic;
using System.IO;

namespace BmArrayLoader
{
    public abstract class Loader
    {
        protected byte[] m_pattern;
        protected string m_type;
        protected byte[] m_bytes;
        protected Tileset m_tileset;
        
        private bool m_loaded;
        private bool m_ready;

        public Loader()
        {
            m_type = string.Empty;
            m_loaded = false;
            m_ready = false;
        }

        public virtual bool Load(string fileName, out Tileset tileset)
        {
            tileset = null;
            try
            {
                m_bytes = File.ReadAllBytes(fileName);
                m_loaded = true;
                m_tileset = new Tileset();
                tileset = m_tileset;
            }
            catch
            {
                m_loaded = false;
                Console.WriteLine("Loader ERROR: Loading " + fileName + " failed!");
            }
            return m_loaded;
        }

        public bool Accepts(string fileName)
        {
            bool retval = false;

            if (m_pattern == null)
                return retval;
            
            byte[] block = new byte[m_pattern.Length];
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                int bytes = stream.Read(block, 0, m_pattern.Length);
                if (bytes == m_pattern.Length & block.SequenceEqual(m_pattern))
                {
                    retval = true;
                }
                stream.Close();
            }

            return retval;
        }

        public string GetType()
        {
            return m_type;
        }
        
        protected void ConfirmReady()
        {
            m_ready = true;
        }

    }
}
