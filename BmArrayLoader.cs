namespace BmArrayLoader;

public class BmArrayLoader
{
    private static LoaderSelector s_loaderSelector = new LoaderSelector();

    static BmArrayLoader()
    {
        //register new loaders here
        s_loaderSelector.RegisterLoader(new PcxLoader());
        s_loaderSelector.RegisterLoader(new LbmLoader());
    }

    public static Loader GetLoader(string file)
    {
        return s_loaderSelector.GetLoader(file);
    }


}