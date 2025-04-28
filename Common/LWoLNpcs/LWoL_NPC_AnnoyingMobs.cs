using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LuneWoL.Common.Npcs
{
    public partial class WoLNpc : GlobalNPC
    {
        public void JungleBatFuckery(NPC npc)
        {
            if (npc.type == NPCID.JungleBat)
            {
                npc.lifeMax *= 4;
                npc.damage *= 4;
            }
        }

        public void GiantTortoiseFuckery(NPC npc)
        {
            if (npc.type == NPCID.GiantTortoise)
            {
                npc.lifeMax *= 4;
                npc.damage *= 4;
            }
        }
        
        public void golemjackingoffFuckery(NPC npc)
        {
            if (npc.type == NPCID.GolemFistRight)
            {
                npc.lifeMax *= 4;
                npc.damage *= 4;
            }
        }

        public void SpikedJungleSlimeFuckery(NPC npc)
        {
            if (npc.type == NPCID.SpikedJungleSlime)
            {
                npc.lifeMax *= 4;
                npc.damage *= 4;
            }
        }
    }
}