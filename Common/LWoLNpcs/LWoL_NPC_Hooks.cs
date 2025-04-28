using Terraria;
using Terraria.ModLoader;

namespace LuneWoL.Common.Npcs
{
    public partial class WoLNpc : GlobalNPC
    {
        public override void SetDefaults(NPC npc)
        {
            var M = LuneWoL.LWoLServerConfig.Main;

            if (M.DemonMode)
            {
                JungleBatFuckery(npc);
                GiantTortoiseFuckery(npc);
                golemjackingoffFuckery(npc);
                SpikedJungleSlimeFuckery(npc);
            }
            LessMoneyDrops(npc);
            NeverGoldEnough(npc);
        }
    }
}