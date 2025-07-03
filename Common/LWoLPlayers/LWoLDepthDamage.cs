using Terraria.Chat;
using Terraria.Localization;

namespace LuneWoL.Common.LWoLPlayers;

internal class LWoLDepthDamage : ModPlayer
{
    public static bool NotEnabled => LuneWoL.LWoLServerConfig.Water.DepthPressureMode == 0;
    public static bool UsingModeOne => LuneWoL.LWoLServerConfig.Water.DepthPressureMode == 1;
    public static bool UsingModeTwo => LuneWoL.LWoLServerConfig.Water.DepthPressureMode == 2;

    public int breathCooldown, maxDepth, pressureDamageToApply, reducedDepthDiff;
    public float reducedDepth, lightDepthDiff, tileDiffCalced, tileDiff, entryY;

    public PressureModeOne ModeOnePlayer => Player.GetModPlayer<PressureModeOne>();
    public SurfacePressurePlayer ModeTwoPlayer => Player.GetModPlayer<SurfacePressurePlayer>();

    [JITWhenModsEnabled("LuneLibAssets")] //private mod with copyrighted content. you cant has this, sorry :c
    public void CopyrightSound() => SoundEngine.PlaySound(DrownSound, Player.Center);
    public void Sound() => SoundEngine.PlaySound(SoundID.Drown, Player.Center);

    private void DamageChecker()
    {
        if (NotEnabled) return;

        if (reducedDepthDiff >= maxDepth)
        {
            Player.LibPlayer().depthwaterPressure = true;
            Player.LibPlayer().currentDepthPressure = pressureDamageToApply;
        }
        else
        {
            Player.LibPlayer().depthwaterPressure = false;
            Player.LibPlayer().currentDepthPressure = 0;
        }
    }

    private void BreathChecker()
    {
        var cfg = LuneWoL.LWoLAdvancedServerSettings.ServerDepthPressure;
        var Bcfg = LuneWoL.LWoLAdvancedServerSettings.ServerDepthPressure.BreathValues;
        var Tcfg = LuneWoL.LWoLAdvancedServerSettings.ServerDepthPressure.TickValues;
        var Dcfg = LuneWoL.LWoLAdvancedServerSettings.ServerDepthPressure.DRValues;

        double depthRatio = tileDiff / (double)maxDepth;
        if (depthRatio < 0.0001)
            depthRatio = 0.0001;
        if (depthRatio > 1.0)
            depthRatio = 1.0;

        double rawBreathLoss = 50.0 * depthRatio;

        double breathLossMult = cfg.BaseBreathDrainRate
            - (Player.gills ? Bcfg.BREATHGillsAddition : 0.0)
            - (Player.accDivingHelm ? Bcfg.BREATHDivingHelmAddition : 0.0)
            - (Player.accDivingHelm && Player.accFlipper ? Bcfg.BREATHDivingGearAddition : 0.0)
            - (Player.arcticDivingGear ? Bcfg.BREATHArcticGearAddition : 0.0)
            - (Player.accMerman ? Bcfg.BREATHMermanAddition : 0.0);

        if (breathLossMult < 0.05)
            breathLossMult = 0.05;

        double breathLoss = rawBreathLoss * breathLossMult;

        double baseTick = cfg.BaseTickRate * (1.0 - depthRatio);
        if (baseTick < 1.0)
            baseTick = 1.0;

        double tickMultiplier = 1.0
            + (Player.gills ? Tcfg.TICKGillsAddition : 0.0)
            + (Player.accDivingHelm ? Tcfg.TICKDivingHelmAddition : 0.0)
            + (Player.accDivingHelm && Player.accFlipper ? Tcfg.TICKDivingGearAddition : 0.0)
            + (Player.arcticDivingGear ? Tcfg.TICKArcticGearAddition : 0.0)
            + (Player.accMerman ? Tcfg.TICKMermanAddition : 0.0);

        if (tickMultiplier > 50.0)
            tickMultiplier = 50.0;

        double tickRate = baseTick * tickMultiplier;
        if (tickRate < 1.0)
            tickRate = 1.0;

        breathCooldown++;
        if (breathCooldown >= (int)tickRate && tileDiff >= 2.0)
        {
            breathCooldown = 0;

            if (Player.breath > 0)
            {
                int breathToSubtract = (int)(breathLoss + 1.0);
                Player.breath -= breathToSubtract;
                if (Player.breath < 0)
                    Player.breath = 0;
            }
        }

        if (Player.breath > 0 && (Player.gills || Player.merman))
        {
            Player.breath -= 3;
            if (Player.breath < 0)
                Player.breath = 0;
        }

        int baseLifeLoss = (int)(cfg.BaseDRRate * depthRatio);
        if (baseLifeLoss < 0)
            baseLifeLoss = 0;

        int lifeLossResist = 0
            + (Player.gills ? Dcfg.DRGillsAddition : 0)
            + (Player.accDivingHelm ? Dcfg.DRDivingHelmAddition : 0)
            + (Player.accDivingHelm && Player.accFlipper ? Dcfg.DRArcticGearAddition : 0)
            + (Player.arcticDivingGear ? Dcfg.DRArcticGearAddition : 0)
            + (Player.accMerman ? Dcfg.DRMermanAddition : 0);

        int finalLifeLoss = baseLifeLoss - lifeLossResist;
        if (finalLifeLoss < 0)
            finalLifeLoss = 0;

        if (Player.breath <= 0)
        {
            Player.statLife -= finalLifeLoss;
            if (Player.statLife < 0)
                Player.statLife = 0;
        }

        if (Player.statLife <= 0)
            KillPlayer();
    }


    public void KillPlayer()
    {
        IEntitySource source_Death = Player.GetSource_Death();
        Player.lastDeathPostion = Player.Center;
        Player.lastDeathTime = DateTime.Now;
        Player.showLastDeath = true;
        bool overFlowing;
        int num = (int)Utils.CoinsCount(out overFlowing, Player.inventory);
        if (Main.myPlayer == Player.whoAmI)
        {
            Player.lostCoins = num;
            Player.lostCoinString = Main.ValueToCoins(Player.lostCoins);
        }
        if (Main.myPlayer == Player.whoAmI)
            Main.mapFullscreen = false;
        if (Main.myPlayer == Player.whoAmI)
        {
            Player.trashItem.SetDefaults(0, noMatCheck: false, null);
            if (Player.difficulty == 0 || Player.difficulty == 3)
                for (int i = 0; i < 59; i++)
                    if (Player.inventory[i].stack > 0 && ((Player.inventory[i].type >= ItemID.LargeAmethyst && Player.inventory[i].type <= ItemID.LargeDiamond) || Player.inventory[i].type == ItemID.LargeAmber))
                    {
                        int num2 = Item.NewItem(source_Death, (int)Player.position.X, (int)Player.position.Y, Player.width, Player.height, Player.inventory[i].type);
                        Main.item[num2].netDefaults(Player.inventory[i].netID);
                        _ = Main.item[num2].Prefix(Player.inventory[i].prefix);
                        Main.item[num2].stack = Player.inventory[i].stack;
                        Main.item[num2].velocity.Y = Main.rand.Next(-20, 1) * 0.2f;
                        Main.item[num2].velocity.X = Main.rand.Next(-20, 21) * 0.2f;
                        Main.item[num2].noGrabDelay = 100;
                        Main.item[num2].favorited = false;
                        Main.item[num2].newAndShiny = false;
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            NetMessage.SendData(MessageID.SyncItem, -1, -1, null, num2);
                        Player.inventory[i].SetDefaults(0, noMatCheck: false, null);
                    }
            else if (Player.difficulty == 1)
                Player.DropItems();
            else if (Player.difficulty == 2)
            {
                Player.DropItems();
                Player.KillMeForGood();
            }
        }
        _ = SoundEngine.PlaySound(in SoundID.PlayerKilled, Player.Center);
        Player.headVelocity.Y = Main.rand.Next(-40, -10) * 0.1f;
        Player.bodyVelocity.Y = Main.rand.Next(-40, -10) * 0.1f;
        Player.legVelocity.Y = Main.rand.Next(-40, -10) * 0.1f;
        Player.headVelocity.X = (Main.rand.Next(-20, 21) * 0.1f) + 0f;
        Player.bodyVelocity.X = (Main.rand.Next(-20, 21) * 0.1f) + 0f;
        Player.legVelocity.X = (Main.rand.Next(-20, 21) * 0.1f) + 0f;
        if (Player.stoned)
        {
            Player.headPosition = Vector2.Zero;
            Player.bodyPosition = Vector2.Zero;
            Player.legPosition = Vector2.Zero;
        }
        for (int j = 0; j < 100; j++)
            _ = Dust.NewDust(Player.position, Player.width, Player.height, DustID.LifeDrain, 0f, -2f);
        Player.mount.Dismount(Player);
        Player.dead = true;
        Player.respawnTimer = 600;
        if (Main.expertMode)
            Player.respawnTimer = (int)(Player.respawnTimer * 1.5);
        Player.immuneAlpha = 0;
        Player.palladiumRegen = false;
        Player.iceBarrier = false;
        Player.crystalLeaf = false;
        PlayerDeathReason playerDeathReason = PlayerDeathReason.ByOther(Player.Male ? 14 : 15);

        if (reducedDepthDiff > maxDepth + 50 && LE && Player.OceanMan() && Player.whoAmI == Main.myPlayer)
        {
            if (LuneLib.LuneLib.instance.LuneLibAssetsLoaded)
                CopyrightSound();
            else
                Sound();
            playerDeathReason = PlayerDeathReason.ByCustomReason(GetText("Status.Death.PressureDeathEdith").ToNetworkText(Player.name));
        }
        else if (reducedDepthDiff > maxDepth + 50 && Player.LibPlayer().depthwaterPressure && Player.OceanMan() && Player.whoAmI == Main.myPlayer)
        {
            if (LuneLib.LuneLib.instance.LuneLibAssetsLoaded)
                CopyrightSound();
            else
                Sound();
            playerDeathReason = PlayerDeathReason.ByCustomReason(GetText("Status.Death.PressureDeathTooDeep").ToNetworkText(Player.name));
        }
        else if (tileDiff >= 50 && Player.OceanMan() && Player.whoAmI == Main.myPlayer)
        {
            if (LuneLib.LuneLib.instance.LuneLibAssetsLoaded)
                CopyrightSound();
            else
                Sound();
            playerDeathReason = PlayerDeathReason.ByCustomReason(GetText("Status.Death.PressureDeath" + Main.rand.Next(1, 10 + 1)).ToNetworkText(Player.name));
        }
        NetworkText deathText = playerDeathReason.GetDeathText(Player.name);

        if (Main.netMode == NetmodeID.MultiplayerClient && Player.whoAmI == Main.myPlayer)
            NetMessage.SendPlayerDeath(Player.whoAmI, playerDeathReason, 1000, 0, pvp: false);
        if (Main.netMode == NetmodeID.Server)
            ChatHelper.BroadcastChatMessage(deathText, new Color(225, 25, 25));
        else if (Main.netMode == NetmodeID.SinglePlayer)
            Main.NewText(deathText.ToString(), 225, 25, 25);
        if (Player.whoAmI == Main.myPlayer && (Player.difficulty == 0 || Player.difficulty == 3))
            _ = Player.DropCoins();
        Player.DropTombstone(num, deathText, 0);
        if (Player.whoAmI == Main.myPlayer)
            try
            {
                WorldGen.saveToonWhilePlaying();
            }
            catch
            {
            }
    }

    public int CalcMaxDepth()
    {
        var cfg = LuneWoL.LWoLAdvancedServerSettings.ServerDepthPressure;
        var Dcfg = LuneWoL.LWoLAdvancedServerSettings.ServerDepthPressure.DepthValues;
        maxDepth = cfg.BaseMaxDepth;

        if (Player.arcticDivingGear)
            maxDepth = (int)(cfg.BaseMaxDepth * (1f + Dcfg.DEPTHArcticGearAddition));
        else if (Player.accDivingHelm && Player.accFlipper)
            maxDepth = (int)(cfg.BaseMaxDepth * (1f + Dcfg.DEPTHDivingGearAddition));
        else if (Player.accDivingHelm)
            maxDepth = (int)(cfg.BaseMaxDepth * (1f + Dcfg.DEPTHDivingHelmAddition));

        if (Player.gills)
            maxDepth = (int)(cfg.BaseMaxDepth * (1f + Dcfg.DEPTHGillsAddition));

        return maxDepth;
    }

    public float CalcReducedDepth()
    {
        var cfg = LuneWoL.LWoLAdvancedServerSettings.ServerDepthPressure.DepthValues;
        reducedDepth = 1f;

        if (Player.arcticDivingGear)
            reducedDepth -= cfg.DEPTHArcticGearAddition;
        if (Player.accDivingHelm && Player.accFlipper)
            reducedDepth -= cfg.DEPTHDivingGearAddition;
        else if (Player.accDivingHelm)
            reducedDepth -= cfg.DEPTHDivingHelmAddition;

        if (Player.gills)
            reducedDepth -= cfg.DEPTHGillsAddition;
        if (reducedDepth <= 0.25f) reducedDepth = 0.25f;

        return reducedDepth;
    }

    public float CalcTileDiff()
    {
        tileDiff = (Player.Center.Y - entryY) / 16f;
        return tileDiff;
    }

    public int CalcReducedTileDiff()
    {
        reducedDepthDiff = (int)(tileDiff * reducedDepth);
        if (reducedDepthDiff < 0) reducedDepthDiff = 0;
        return reducedDepthDiff;
    }

    public float CalcTileDiffClamped()
    {
        tileDiffCalced = Math.Clamp(reducedDepthDiff, 0, maxDepth);
        return tileDiffCalced;
    }

    public int CalcPressureDamage()
    {
        pressureDamageToApply = reducedDepthDiff - maxDepth;
        return pressureDamageToApply;
    }

    public float CalcLightDepthDiff()
    {
        lightDepthDiff = maxDepth == 0 ? 0f : tileDiffCalced / maxDepth;
        return lightDepthDiff;
    }

    private void UpdateWaterState()
    {
        if (UsingModeOne)
        {
            ModeOnePlayer.CheckWaterDepth();
            entryY = ModeOnePlayer.EntryPoint.Y;
        }
        else if (UsingModeTwo)
            entryY = ModeTwoPlayer.CachedTopY * 16f;
    }

    public override void PostUpdateMiscEffects()
    {
        if (NotEnabled) return;

        if (Player.whoAmI != Main.myPlayer)
            return;

        UpdateWaterState();
    }

    public override void PostUpdateEquips()
    {
        if (NotEnabled) return;

        if (Player.whoAmI != Main.myPlayer)
            return;
        if (entryY < 0 && UsingModeTwo)
            return;

        if (Player.OceanMan())
        {
            _ = CalcMaxDepth();
            _ = CalcReducedDepth();
            _ = CalcTileDiff();
            _ = CalcReducedTileDiff();
            _ = CalcTileDiffClamped();
            _ = CalcPressureDamage();
            _ = CalcLightDepthDiff();

            BreathChecker();
            DamageChecker();
        }
    }

    public override void PostUpdate()
    {
        if (NotEnabled) return;

        if (Player.whoAmI != Main.myPlayer)
            return;
        if (!Player.LibPlayer().LWaterEyes)
            return;
        if (entryY < 0 && UsingModeTwo)
            return;

        ScreenObstruction.screenObstruction = MathHelper.Lerp(ScreenObstruction.screenObstruction, 1f, lightDepthDiff);
        float reversed = 1f - lightDepthDiff;
        float clamped = MathHelper.Clamp(reversed, 0.5f, 1f);
        Lighting.GlobalBrightness *= clamped;
    }
}
