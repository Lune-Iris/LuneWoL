using LuneWoL.Common.LWoLPlayers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LuneWoL.Common.LWoLGlobalItems
{
    public partial class WoLGlobalItems : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool? UseItem(Item item, Player player)
        {
            var p = player.GetModPlayer<LWoLPlayer>();
            var main = LuneWoL.LWoLServerConfig.Main;
            var misc = LuneWoL.LWoLServerConfig.Misc;

            if (player.whoAmI == Main.myPlayer && main.CritFailMode != 0 && p.IsCritFail)
            {
                if (main.CritFailMode == 1)
                {
                    p.AplyDmgAmt = player.GetWeaponDamage(item);
                }

                p.DmgPlrBcCrit = true;
            }

            if (item.type == ItemID.LifeCrystal && misc.DeathPenaltyMode == 1)
            {
                if (player.ConsumedLifeCrystals >= Player.LifeCrystalMax)
                {
                    LWoLPlayer.AUURHGHRUGH = true;
                }
            }

            if (item.type == ItemID.LifeFruit && misc.DeathPenaltyMode == 1)
            {
                if (player.ConsumedLifeFruit >= Player.LifeFruitMax)
                {
                    LWoLPlayer.AUURHGHRUGH = true;
                }
            }

            if (item.type == ItemID.ManaCrystal)
            {
                if (player.ConsumedManaCrystals >= Player.ManaCrystalMax && misc.DeathPenaltyMode == 1)
                {
                    LWoLPlayer.AUURHGHRUGHpart2ofc = true;
                }
            }

            return base.UseItem(item, player);
        }

        public override void SetDefaults(Item item)
        {
            NoAccessories(item);

            NoReusing(item);

            Stackables(item);
        }

        public override void PostUpdate(Item item)
        {
            var misc = LuneWoL.LWoLServerConfig.Misc;

            if (misc.DespawnItemsTimer >= -1)
            {
                DespawnItemsAfterTime(item);
            }
        }
    }
}
