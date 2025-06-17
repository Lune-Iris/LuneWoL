namespace LuneWoL.Common.LWoLGlobalItems;

public partial class WoLGlobalItems : GlobalItem
{
    public override bool InstancePerEntity => true;

    public override bool? UseItem(Item item, Player player)
    {
        var p = player.GetModPlayer<LWoLPlayer>();
        var plr = LuneWoL.LWoLServerConfig.LPlayer;

        if (player.whoAmI == Main.myPlayer && plr.CritFailMode != 0 && p.IsCritFail)
        {
            if (plr.CritFailMode == 1)
            {
                p.AplyDmgAmt = player.GetWeaponDamage(item);
            }

            p.DmgPlrBcCrit = true;
        }

        if (item.type == ItemID.LifeCrystal && plr.DeathPenaltyMode == 1)
        {
            if (player.ConsumedLifeCrystals >= Player.LifeCrystalMax)
            {
                LWoLPlayer.DeathFlag0 = true;
            }
        }

        if (item.type == ItemID.LifeFruit && plr.DeathPenaltyMode == 1)
        {
            if (player.ConsumedLifeFruit >= Player.LifeFruitMax)
            {
                LWoLPlayer.DeathFlag0 = true;
            }
        }

        if (item.type == ItemID.ManaCrystal)
        {
            if (player.ConsumedManaCrystals >= Player.ManaCrystalMax && plr.DeathPenaltyMode == 1)
            {
                LWoLPlayer.DeathFlag1 = true;
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
        var itm = LuneWoL.LWoLServerConfig.Items;

        if (itm.DespawnItemsTimer >= -1)
        {
            DespawnItemsAfterTime(item);
        }
    }
}
