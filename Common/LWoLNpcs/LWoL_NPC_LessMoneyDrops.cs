namespace LuneWoL.Common.Npcs;

public partial class LWoL_NPC : GlobalNPC
{
    private static void LessMoneyDrops(NPC npc)
    {
        var Config = LuneWoL.LWoLServerConfig.NPCs;

        if (Config.NoMoneh == 1) return;

        npc.value *= Config.NoMoneh;
    }

    private static void NeverGoldEnough(NPC npc)
    {
        var Config = LuneWoL.LWoLServerConfig.NPCs;

        if (!Config.NeverGoldEnough) return;

        float cappedVal = Math.Clamp(npc.value, 0, 1600);

        npc.value = Config.NoMoneh != 1 ? cappedVal * Config.NoMoneh : cappedVal;
    }
}
