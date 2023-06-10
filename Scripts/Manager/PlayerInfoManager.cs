namespace MauiApp1;
public class PlayerInfoManager
{
    private static PlayerInfoManager instance;

    private static Object syncObj = new object();

    public string Name
    {
        get;
        set;
    }

    public static PlayerInfoManager Instance
    {
        get
        {
            if(instance == null)
            {
                lock(syncObj)
                {
                    if(instance == null)
                    {
                        instance = new PlayerInfoManager();
                    }
                    return instance;
                }
            }else
            {
                return instance;
            }
        }
    }
}
