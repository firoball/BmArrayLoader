using System.Collections.Generic;

namespace BmArrayLoader;

public class LoaderSelector
{
    private List<Loader> m_loaders = new List<Loader>();

    public void RegisterLoader(Loader loader)
    {
        if (!m_loaders.Contains(loader))
            m_loaders.Add(loader);
    }
    
    public Loader GetLoader(string fileName)
    {
        foreach (Loader loader in m_loaders)
        {
            if (loader.Accepts(fileName))
                return loader;
        }
        return null;
    }
}