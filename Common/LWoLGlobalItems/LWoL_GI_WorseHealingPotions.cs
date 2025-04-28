using Terraria;
using Terraria.ModLoader;

namespace LuneWoL.Common.LWoLGlobalItems
{
    public partial class WoLGlobalItems : GlobalItem
    {
        public override void GetHealLife(Item item, Player player, bool quickHeal, ref int healValue)
        {
            var buffs = LuneWoL.LWoLServerConfig.BuffsAndDebuffs;

            if (buffs.HealingPotionBadPercent > 1 && buffs.HealingPotionBadPercent < 100)
            {
                healValue = healValue * buffs.HealingPotionBadPercent / 100;

                base.GetHealLife(item, player, quickHeal, ref healValue);
            }
            else if (buffs.HealingPotionBadPercent <= 1)
            {
                healValue = 0;
            }
        }
    }
}
