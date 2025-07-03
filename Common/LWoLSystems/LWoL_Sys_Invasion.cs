namespace LuneWoL.Common.LWoLSystems;

public partial class LWoL_Sys : ModSystem
{
    private bool flag = true;

    public void LongerInvasions()
    {
        var Config = LuneWoL.LWoLServerConfig.NPCs;

        if (Config.InvasionMultiplier! <= 0 && Main.invasionType != 0 && flag)
        {
            Main.invasionSizeStart *= Config.InvasionMultiplier;
            Main.invasionSize *= Config.InvasionMultiplier;
            Main.invasionProgressMax *= Config.InvasionMultiplier;
            NPC.waveNumber *= Config.InvasionMultiplier;
            flag = false;
        }
        else if (Config.InvasionMultiplier! <= 0 && Main.invasionType == 0 && !flag)
        {
            Main.invasionSizeStart /= Config.InvasionMultiplier;
            Main.invasionSize /= Config.InvasionMultiplier;
            Main.invasionProgressMax /= Config.InvasionMultiplier;
            NPC.waveNumber /= Config.InvasionMultiplier;
            flag = true;
        }
    }
}