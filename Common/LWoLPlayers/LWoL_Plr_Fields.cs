namespace LuneWoL.Common.WoL_Plrs;

public partial class LWoL_Plr : ModPlayer
{
    public int
        TundraBlizzardCounter,
        TundraChilledCounter,
        HeatStrokeCounter,
        LostHealth,
        LostMana,
        HealthCache,
        ManaCache;

    internal static bool
        DeathFlag0 = false,
        DeathFlag1 = false;
}
