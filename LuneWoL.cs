namespace LuneWoL;

public partial class LuneWoL : Mod
{
    internal static Mod Instance;
    internal static LuneWoL instance;
    internal static LWoLServerConfig LWoLServerConfig;
    internal static LWoLClientConfig LWoLClientConfig;
    internal static LWoLServerStatConfig LWoLServerStatConfig;
    internal static LWoLAdvancedServerSettings LWoLAdvancedServerSettings;
    internal static LWoLAdvancedClientSettings LWoLAdvancedClientSettings;

    public override void Load()
    {
        instance = this;
        Instance = this;

        // compat issues im guessing (havent tried)
        if (LuneLib.LuneLib.instance.StrongerReforgesLoaded && LWoLServerConfig.Equipment.ReforgeNerf)
        {
            throw new Exception($"Disable `Reforge Nerf` in the config if you wanna use the `Stronger Reforges` mod." + new string('\n', 20));
        }

        // same as reforge thing
        if (LuneLib.LuneLib.instance.DarkSurfaceLoaded && LWoLServerConfig.Environment.DarkerNightsMode != 0)
        {
            throw new Exception("$Disable `Darker Nights` in the config if you wanna use the `Dark Surface` mod." + new string('\n', 20));
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
