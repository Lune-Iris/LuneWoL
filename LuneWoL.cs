namespace LuneWoL;

public partial class LuneWoL : Mod
{
    public static Mod Instance;
    public static LuneWoL instance;
    public static LWoLServerConfig LWoLServerConfig;
    public static LWoLClientConfig LWoLClientConfig;
    public static LWoLServerStatConfig LWoLServerStatConfig;
    public static LWoLAdvancedServerSettings LWoLAdvancedServerSettings;
    public static LWoLAdvancedClientSettings LWoLAdvancedClientSettings;

    public override void Load()
    {
        instance = this;
        Instance = this;

        // compat issues im guessing (havent tried)
        if (LuneLib.LuneLib.instance.StrongerReforgesLoaded && LWoLServerConfig.Equipment.ReforgeNerf)
        {
            throw new Exception("Disable `Reforge Nerf` in the config if you wanna use the `Stronger Reforges` mod.\n" + new string('\n', 20));
        }

        // same as reforge thing
        if (LuneLib.LuneLib.instance.DarkSurfaceLoaded && LWoLServerConfig.Main.DarkerNightsMode != 0)
        {
            throw new Exception("Disable `Darker Nights` in the config if you wanna use the `Dark Surface` mod.\n" + new string('\n', 20));
        }

        LWoLILEdits.LoadIL();
    }

    public override void Unload()
    {
        Instance = null;
        instance = null;
        LWoLServerConfig = null;
        LWoLClientConfig = null;
        LWoLServerStatConfig = null;
    }
}
