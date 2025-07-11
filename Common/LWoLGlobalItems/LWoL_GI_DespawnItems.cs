﻿namespace LuneWoL.Common.LWoLGlobalItems;

public partial class LWoL_Items : GlobalItem
{

    private int Tajmer = LuneWoL.LWoLServerConfig.Items.DespawnItemsTimer * 60;
    private bool a = true;
    private void DespawnItemsAfterTime(Item item)
    {
        var Config = LuneWoL.LWoLServerConfig.Items;

        if (Config.DespawnItemsTimer > -1)
        {
            if (Tajmer > 0)
            {
                Tajmer--;
            }
            else if (Tajmer <= 0)
            {
                Tajmer = Config.DespawnItemsTimer * 60;
            }


            if (a && Tajmer <= 0)
            {
                item.TurnToAir(true);
                a = false;
            }
            if (Tajmer <= 0)
            {
                DustyDespawn(item);
            }
        }
    }

    private void DustyDespawn(Item item)
    {
        var Config = LuneWoL.LWoLServerConfig.Items;

        if (Config.DespawnItemsTimer > -1)
        {
            for (int i = 0; i < 30; i++)
            {
                Dust.NewDust(item.position, 1, 1, DustID.Smoke, 0f, 0f, 100, default, 1f);
            }
        }
    }
}
