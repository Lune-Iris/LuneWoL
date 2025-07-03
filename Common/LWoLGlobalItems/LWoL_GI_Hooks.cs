namespace LuneWoL.Common.LWoLGlobalItems;

public partial class LWoL_Items : GlobalItem
{
    public override bool InstancePerEntity => true;

    public override bool? UseItem(Item item, Player player)
    {
        var p = player.GetModPlayer<LWoL_Plr>();
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
                LWoL_Plr.DeathFlag0 = true;
            }
        }

        if (item.type == ItemID.LifeFruit && plr.DeathPenaltyMode == 1)
        {
            if (player.ConsumedLifeFruit >= Player.LifeFruitMax)
            {
                LWoL_Plr.DeathFlag0 = true;
            }
        }

        if (item.type == ItemID.ManaCrystal)
        {
            if (player.ConsumedManaCrystals >= Player.ManaCrystalMax && plr.DeathPenaltyMode == 1)
            {
                LWoL_Plr.DeathFlag1 = true;
            }
        }

        return base.UseItem(item, player);
    }

    public override void SetDefaults(Item item)
    {
        var equipment = LuneWoL.LWoLServerConfig.Equipment;

        if (!equipment.NoAccessories) return;

        if (!item.vanity)
        {
            item.accessory = false;
            ItemID.Sets.CanGetPrefixes[item.type] = false;
        }
        if (item.wingSlot > -1)
        {
            ArmorIDs.Wing.Sets.Stats[item.wingSlot] = new WingStats(0, 0, 0, false, 0);
        }

        if (LuneWoL.LWoLServerConfig.Equipment.DisableAutoReuse)
            item.autoReuse = false;

        if (item.type == ItemID.MusicBox)
            item.maxStack = 9999;
    }
    public override void GetHealLife(Item item, Player player, bool quickHeal, ref int healValue)
    {
        var buffs = LuneWoL.LWoLServerConfig.Buffs;

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

    public override void PostUpdate(Item item)
    {
        var itm = LuneWoL.LWoLServerConfig.Items;

        if (itm.DespawnItemsTimer >= -1)
        {
            DespawnItemsAfterTime(item);
        }
    }
}
