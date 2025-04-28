using Terraria;
using Terraria.ModLoader;
using static LuneWoL.LuneWoL;

namespace LuneWoL.Common.LWoLNpcs
{
    public class LWoL_NPC_StatChanges : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override void SetDefaults(NPC npc)
        {
            ApplyStatChanges(npc);
            ApplyBossStatChanges(npc);
        }

        public override void SetDefaultsFromNetId(NPC npc)
        {
            ApplyStatChanges(npc);
            ApplyBossStatChanges(npc);
        }

        private void ApplyStatChanges(NPC npc)
        {
            var npcConfig = LWoLServerStatConfig.NpcConfig;

            if (npcConfig.DisableNPCStatChanges) return;
            if (npc.CountsAsACritter) return;
            if (npc.friendly) return;
            if (npc.boss) return;

            npc.damage = npc.damage * (npcConfig.DamagePercent / 100);
            npc.defense = npc.defense * (npcConfig.DefensePercent / 100);
            npc.lifeMax = npc.lifeMax * (npcConfig.LifePercent / 100);
            npc.life = npc.lifeMax;
        }

        private void ApplyBossStatChanges(NPC npc)
        {
            var bossConfig = LWoLServerStatConfig.BossConfig;

            if (bossConfig.DisableBossStatChanges) return;
            if (!npc.boss) return;

            npc.damage = npc.damage * (bossConfig.DamagePercent / 100);
            npc.defense = npc.defense * (bossConfig.DefensePercent / 100);
            npc.lifeMax = npc.lifeMax * (bossConfig.LifePercent / 100);
            npc.life = npc.lifeMax;
        }
    }
}
