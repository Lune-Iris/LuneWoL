namespace LuneWoL.Common.Npcs;

public partial class WoLNpc : GlobalNPC
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

        if (Config.NeverGoldEnough) return;

        if (npc.value > 1600 && Config.NoMoneh != 1)
        {
            npc.value = 1600 * Config.NoMoneh;
        }
        else if (npc.value > 1600)
        {
            npc.value = 1600;
        }
    }
}
