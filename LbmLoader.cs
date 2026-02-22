using System;

namespace BmArrayLoader
{
    public class LbmLoader : Loader
    {
        private int m_width;
        private int m_height;
        private int m_compressed;
        private int m_offset;

        public LbmLoader() : base()
        {
            m_pattern = new byte [] { 0x46, 0x4F, 0x52, 0x4D, 0x0 }; //LbmChunk.Form
            m_type = "LBM";
            m_offset = 0;
        }

        protected override bool LoadInternal()
        {
            return ReadChunks();
        }

        private bool ReadChunks()
        {
            bool status = false;
            bool first = true;
            if (m_bytes.Length > 8)
            {
                do
                {
                    int chunkId = ReadInt32();
                    if (first && chunkId != (int)LbmChunk.Form) //first chunk must be FORM
                    {
                        Console.WriteLine("LbmLoader ERROR: Unsupported image format found.");
                        return false;
                    }
                    first = false;
                    int length = ReadInt32();
                    int padding = length % 2;
                    if (m_offset + length + padding <= m_bytes.Length)
                    {
                        switch (chunkId)
                        {
                            case (int)LbmChunk.Form:
                                status = ReadForm(length);
                                //FORM is container for all other chunks, therefore don't apply padding here - it is at eof
                                break;

                            case (int)LbmChunk.Bmhd:
                                status = ReadHeader(length);
                                m_offset += padding;
                                break;

                            case (int)LbmChunk.Cmap:
                                status = ReadColormap(length);
                                m_offset += padding;
                                break;

                            case (int)LbmChunk.Body:
                                status = ReadBody(length);
                                m_offset += padding;
                                //BODY is optional - file might only contain color palette
                                break;

                            default:
                                status = SkipChunk(length);
                                m_offset += padding;
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("LbmLoader ERROR: Truncated chunk found.");
                        status = false;
                    }
                } while (status && (m_bytes.Length - m_offset > 4)); //make sure padding byte does not start another iteration
            }

            return status;
        }

        private bool ReadForm(int length)
        {
            int type = ReadInt32();
            switch (type)
            {
                case (int)LbmType.Pbm:
                    return true;

                case (int)LbmType.Ilbm:
                    Console.WriteLine("LbmLoader ERROR: ILBM format not supported.");
                    return false;

                default:
                    Console.WriteLine("LbmLoader ERROR: Unknown format identifier found.");
                    return false;
            }
        }

        private bool ReadHeader(int length)
        {
            m_width = ReadInt16();
            m_height = ReadInt16();
            m_offset += 4; //skip stuff
            int depth = ReadByte();
            m_offset++; //skip stuff
            m_compressed = ReadByte();
            m_offset += 9; //skip stuff
            if (depth == 0 || depth == 8) //only palette or indexed color supported
            {
                return true;
            }
            else
            {
                Console.WriteLine("LbmLoader ERROR: Unsupported color depth found.");
                return false;
            }
        }

        private bool ReadBody(int length)
        {
            m_tileset.InitMaster(m_width, m_height);
            switch (m_compressed)
            {
                case (int)LbmCompression.None:
                    return ReadUncompressed(length);

                case (int)LbmCompression.ByteRun1:
                    return ReadCompressed(length);

                default:
                    Console.WriteLine("LbmLoader ERROR: Unknown compression found.");
                    return false;
            }
        }

        private bool ReadUncompressed(int length)
        {
            //LBM applies padding to get lines with n*16 bit length
            //parser only supports depth of 8 bit (=1 byte), therefore just pad odd width with 1 byte
            int linePadding = m_width % 2;
//            if ((m_width % 16) != 0)
//                linePadding = 16 - (m_width % 16); //TODO this is wrong - BIT not byte, index buffer will always have 8 bit -> check even/odd width only
            int lineIdx;
            int tgtIdx = 0;

            for (int i = 0; i < length; i++)
            {
                if (tgtIdx < m_tileset.Master.Size - 1)
                {
                    lineIdx = i % (m_width + linePadding);
                    if (lineIdx < m_width)
                    {
                        m_tileset.Master[tgtIdx++] = ReadByte();
                    }
                    else
                        m_offset++; //ignore padding
                }
                else
                {
                    Console.WriteLine("LbmLoader ERROR: Corrupted body data found.");
                    return false;
                }
            }

            return true;
        }

        private bool ReadCompressed(int length)
        {
            byte selector;
            byte value;
            int count;
            int tgtIdx = 0;
            for (int i = 0; i < length; i++)
            {
                selector = ReadByte();
                if (selector < 128)
                {
                    count = selector + 1;
                    if ((tgtIdx < m_tileset.Master.Size - count) && (i + 1 < length - count))
                    {
                        for (int j = 0; j < count; j++)
                            m_tileset.Master[tgtIdx++] = ReadByte();
                        i += count; //update iterator
                    }
                    else
                    {
                        Console.WriteLine("LbmLoader ERROR: Corrupted body data found.");
                        return false;
                    }
                }
                else if (selector > 128)
                {
                    count = 257 - selector;
                    if (tgtIdx < m_tileset.Master.Size - 1)
                    {
                        value = ReadByte();
                        for (int j = 0; j < count; j++)
                            m_tileset.Master[tgtIdx++] = value;
                        i++; //update iterator
                    }
                    else
                    {
                        Console.WriteLine("LbmLoader ERROR: Corrupted body data found.");
                        return false;
                    }
                }
                else //ignore value 128
                {
                    m_offset++;
                }
            }
            return true;
        }

        private bool ReadColormap(int length)
        {
            if (length % 3 == 0)
            {
                int palIdx = 0;
                for (int i = length; i > 0; i -= 3)
                {
                    m_tileset.Palette[palIdx][0] = ReadByte(); //R
                    m_tileset.Palette[palIdx][1] = ReadByte(); //G
                    m_tileset.Palette[palIdx][2] = ReadByte(); //B
                    palIdx++;
                }
                return true;
            }
            else //truncated chunk
                return false;
        }

        private bool SkipChunk(int length)
        {
            m_offset += length; //just ignore contents
            return true;
        }

        private int ReadInt32()
        {
            int value = (m_bytes[m_offset] << 24) | (m_bytes[m_offset + 1] << 16) | (m_bytes[m_offset + 2] << 8) | m_bytes[m_offset + 3];
            m_offset += 4;
            return value;
        }

        private int ReadInt16()
        {
            int value = (m_bytes[m_offset] << 8) | m_bytes[m_offset + 1];
            m_offset += 2;
            return value;
        }

        private byte ReadByte()
        {
            byte value = m_bytes[m_offset];
            m_offset++;
            return value;
        }

        private enum LbmType : int
        {
            Ilbm = 0x494C424D,
            Pbm = 0x50424D20,
        }

        private enum LbmCompression : int
        {
            None = 0,
            ByteRun1 = 1,
        }

        private enum LbmChunk : int
        {
            Form = 0x464F524D,
            Bmhd = 0x424D4844,
            Body = 0x424F4459,
            Cmap = 0x434D4150,
        }
    }
}
