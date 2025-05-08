namespace LuneWoL.Common.LWoLPlayers;

public partial class LWoLPlayer : ModPlayer
{
    public int
        TundraBlizzardCounter,
        TundraChilledCounter,
        HeatStrokeCounter,
        LostHealth,
        LostMana,
        HealthCache,
        ManaCache;

    public static bool
        DeathFlag0 = false,
        DeathFlag1 = false;
}
