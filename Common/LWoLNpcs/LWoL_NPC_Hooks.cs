namespace LuneWoL.Common.Npcs;

public partial class LWoL_NPC : GlobalNPC
{
    public override void SetDefaults(NPC npc)
    {
        var M = LuneWoL.LWoLServerConfig.NPCs;
        
        if (M.DemonMode)
        {
            if (npc.type == NPCID.JungleBat)
            {
                npc.lifeMax *= 4;
                npc.damage *= 4;
            }
            if (npc.type == NPCID.GiantTortoise)
            {
                npc.lifeMax *= 4;
                npc.damage *= 4;
            }
            if (npc.type == NPCID.GolemFistRight)
            {
                npc.lifeMax *= 4;
                npc.damage *= 4;
            }
            if (npc.type == NPCID.SpikedJungleSlime)
            {
                npc.lifeMax *= 4;
                npc.damage *= 4;
            }
        }
        LessMoneyDrops(npc);
        NeverGoldEnough(npc);
    }
}