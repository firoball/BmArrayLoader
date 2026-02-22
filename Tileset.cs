using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BmArrayLoader;

public class Tileset
{
    private bool m_initialized;
    private List<byte[]> m_palette;
    private Indexmap m_master;
    private List<Indexmap> m_tiles;

    private ReadOnlyCollection<Indexmap> m_tilesReadOnly;
    private ReadOnlyCollection<byte[]> m_paletteReadOnly;

    public Indexmap Master { get => m_master; }
    public ReadOnlyCollection<Indexmap> Tiles { get => m_tilesReadOnly; }
    public ReadOnlyCollection<byte[]> Palette { get => m_paletteReadOnly; }

    public Tileset()
    {
        m_initialized = false;
        m_palette = new List<byte[]>(256);
        for (int i = 0; i < 256; i++)
            m_palette.Add(new byte[3]);
        m_tiles = new List<Indexmap>();
        
        m_tilesReadOnly = m_tiles.AsReadOnly();
        m_paletteReadOnly = m_palette.AsReadOnly();
    }

    public Tileset(int width, int height) : this()
    {
        InitMaster(width, height);
    }

    public void InitMaster(int width, int height)
    {
        m_master = new Indexmap(width, height);
        m_tiles.Add(m_master);
        m_initialized = true;
    }

    public Indexmap GetTile(int offsetX, int offsetY, int width, int height)
    {
        int idx = GetTileId(offsetX, offsetY, width, height);
        return idx != -1 ? m_tiles[idx] : null;
    }
    
    public int GetTileId(int offsetX, int offsetY, int width, int height)
    {
        if (!m_initialized)
            return -1;
        
        //Indexmap already in list?
        int idx = m_tiles.FindIndex(x => x.Equals(offsetX, offsetY, width, height));
        if (idx == -1)
        {
            //derive new index map from master
            int sizeX = offsetX + width;
            int sizeY = offsetY + height;

            if (sizeX > 0 && sizeY > 0 &&
                width > 0 && height > 0 &&
                sizeX <= m_master.Width && sizeY <= m_master.Height)
            {
                Indexmap indexmap = new Indexmap(offsetX, offsetY, width, height);
                int srcIdx = 0;
                int tgtIdx = 0;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        srcIdx = m_master.Width * (offsetY + y) + offsetX + x;
                        indexmap[tgtIdx] = m_master[srcIdx];
                        tgtIdx++;
                    }
                }
                m_tiles.Add(indexmap);
                idx =  m_tiles.Count - 1;
            }
            else
            {
                Console.WriteLine("TILESET error: Indexmap not created - invalid dimensions");
            }
        }
        return idx;
    }

    public bool AddTile(int offsetX, int offsetY, int width, int height)
    {
        int tiles = m_tiles.Count;
        int idx = GetTileId(offsetX, offsetY, width, height);
        return (idx == tiles); //new tile was added
    }
    
}