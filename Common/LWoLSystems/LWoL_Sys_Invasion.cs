namespace LuneWoL.Common.LWoLSystems;

public partial class LWoLSystem : ModSystem
{
    public bool b = true;

    public void LongerInvasions()
    {
        var Config = LuneWoL.LWoLServerConfig.NPCs;

        if (Config.InvasionMultiplier! <= 0 && Main.invasionType != 0 && b)
        {
            Main.invasionSizeStart *= Config.InvasionMultiplier;
            Main.invasionSize *= Config.InvasionMultiplier;
            Main.invasionProgressMax *= Config.InvasionMultiplier;
            NPC.waveNumber *= Config.InvasionMultiplier;
            b = false;
        }
        else if (Config.InvasionMultiplier! <= 0 && Main.invasionType == 0 && !b)
        {
            Main.invasionSizeStart /= Config.InvasionMultiplier;
            Main.invasionSize /= Config.InvasionMultiplier;
            Main.invasionProgressMax /= Config.InvasionMultiplier;
            NPC.waveNumber /= Config.InvasionMultiplier;
            b = true;
        }
    }
}
