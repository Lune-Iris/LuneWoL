﻿namespace LuneWoL.Common.WoL_Plrs;

public partial class LWoL_Plr : ModPlayer
{
    public bool IsCritFail;

    public bool DmgPlrBcCrit;

    public int AplyDmgAmt;

    public void CritFail(Player player, NPC npc)
    {
        var Config = LuneWoL.LWoLServerConfig.LPlayer;

        if (Config.CritFailMode == 1 || Config.CritFailMode == 2)
        {
            int normalCritChance = (int)player.GetTotalCritChance(DamageClass.Generic);

            int chanceoffail = Main.rand.Next(normalCritChance + 1);

            if (chanceoffail == 1)
            {
                IsCritFail = true;
            }
        }
        else if (Config.CritFailMode == 3 || Config.CritFailMode == 4)
        {
            int chanceoffail = Main.rand.Next(100 + 1);

            if (chanceoffail == 1)
            {
                IsCritFail = true;
            }
        }
    }

    public void CritFailDamage(Player player)
    {
        var Config = LuneWoL.LWoLServerConfig.LPlayer;

        if (Config.CritFailMode == 0) return;

        if (player.whoAmI == Main.myPlayer && IsCritFail && (Config.CritFailMode == 1 || Config.CritFailMode == 3))
        {
            player.Hurt(PlayerDeathReason.ByCustomReason(GetText("Status.Death.FailedCrit" + Main.rand.Next(1, 5 + 1)).ToNetworkText(Player.name)), AplyDmgAmt, 0);
        }
        if (player.whoAmI == Main.myPlayer && IsCritFail && Config.CritFailMode > 0)
        {
            WaitUntilZero();
        }
    }

    public async void WaitUntilZero()
    {
        await Task.Delay(32);

        DmgPlrBcCrit = false;
        IsCritFail = false;
    }
}
