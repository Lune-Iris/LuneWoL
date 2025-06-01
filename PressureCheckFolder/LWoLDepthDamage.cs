using CalamityMod;
using CalamityMod.World;
using Terraria.Chat;
using Terraria.Localization;
using Terraria.ID;

namespace LuneWoL.PressureCheckFolder;

internal class LWoLDepthDamage : ModPlayer
{
    public static bool UsingModeOne => LuneWoL.LWoLServerConfig.WaterRelated.DepthPressureMode == 1;
    public static bool UsingModeTwo => LuneWoL.LWoLServerConfig.WaterRelated.DepthPressureMode == 2;

    public int breathCooldown, maxDepth, pressureDamageToApply, reducedDepthDiff;
    public float reducedDepth, lightDepthDiff, tileDiffCalced, tileDiff, entryY;
    public PlayerDeathReason dmgsrc;

    public Mode1.PressureModeOne modeOnePlayer => Player.GetModPlayer<Mode1.PressureModeOne>();
    public Mode2.SurfacePressurePlayer modeTwoPlayer => Player.GetModPlayer<Mode2.SurfacePressurePlayer>();

    [JITWhenModsEnabled("LuneLibAssets")] //private mod with copyrighted content. you cant has this, sorry :c
    public void CopyrightSound() => SoundEngine.PlaySound(DrownSound, Player.Center);
    public void Sound() => SoundEngine.PlaySound(SoundID.Drown, Player.Center);

    private void DamageChecker()
    {
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
        double depthRatio = (tileDiff / maxDepth) * 2.0;
        if (depthRatio < 0.0001)
            depthRatio = 0.0001;

        double baseDrain = 12.0 * (1.0 - depthRatio) * cfg.BreathBaseDrainRate;
        if (baseDrain < 1.0)
            baseDrain = 1.0;

        double tickMultiplier = 1.0;
        if (Player.gills)
            tickMultiplier -= cfg.BreathGillsMultiplier;
        if (Player.accDivingHelm)
            tickMultiplier -= cfg.BreathDivingHelmMultiplier;
        if (Player.arcticDivingGear)
            tickMultiplier -= cfg.BreathArcticGearMultiplier;
        if (Player.accMerman)
            tickMultiplier -= cfg.MermanDepthReduction;
        if (Player.ignoreWater)
            tickMultiplier -= 0.5;

        tickMultiplier = MathHelper.Clamp((float)tickMultiplier, 0.1f, 50f);

        double tickRate = baseDrain / (depthRatio * tickMultiplier);
        tickRate = Math.Max(tickRate, 1.0);

        breathCooldown++;
        if (breathCooldown >= (int)tickRate && tileDiff >= 2.0)
        {
            breathCooldown = 0;
            if (Player.breath > 0)
                Player.breath -= 1;
        }

        if (Player.breath > 0 && (Player.gills || Player.merman))
        {
            Player.breath -= 3;
        }

        int zeroBreathLifeLoss = (int)(6.0 * depthRatio);
        if (zeroBreathLifeLoss < 0)
            zeroBreathLifeLoss = 0;

        if (Player.breath <= 0)
        {
            Player.statLife -= zeroBreathLifeLoss;
        }
        if (Player.statLife <= 0)
        {
            KillPlayer();
        }
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
        {
            Main.mapFullscreen = false;
        }
        if (Main.myPlayer == Player.whoAmI)
        {
            Player.trashItem.SetDefaults(0, noMatCheck: false, null);
            if (Player.difficulty == 0 || Player.difficulty == 3)
            {
                for (int i = 0; i < 59; i++)
                {
                    if (Player.inventory[i].stack > 0 && ((Player.inventory[i].type >= ItemID.LargeAmethyst && Player.inventory[i].type <= ItemID.LargeDiamond) || Player.inventory[i].type == ItemID.LargeAmber))
                    {
                        int num2 = Item.NewItem(source_Death, (int)Player.position.X, (int)Player.position.Y, Player.width, Player.height, Player.inventory[i].type);
                        Main.item[num2].netDefaults(Player.inventory[i].netID);
                        Main.item[num2].Prefix(Player.inventory[i].prefix);
                        Main.item[num2].stack = Player.inventory[i].stack;
                        Main.item[num2].velocity.Y = Main.rand.Next(-20, 1) * 0.2f;
                        Main.item[num2].velocity.X = Main.rand.Next(-20, 21) * 0.2f;
                        Main.item[num2].noGrabDelay = 100;
                        Main.item[num2].favorited = false;
                        Main.item[num2].newAndShiny = false;
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                        {
                            NetMessage.SendData(MessageID.SyncItem, -1, -1, null, num2);
                        }
                        Player.inventory[i].SetDefaults(0, noMatCheck: false, null);
                    }
                }
            }
            else if (Player.difficulty == 1)
            {
                Player.DropItems();
            }
            else if (Player.difficulty == 2)
            {
                Player.DropItems();
                Player.KillMeForGood();
            }
        }
        SoundEngine.PlaySound(in SoundID.PlayerKilled, Player.Center);
        Player.headVelocity.Y = Main.rand.Next(-40, -10) * 0.1f;
        Player.bodyVelocity.Y = Main.rand.Next(-40, -10) * 0.1f;
        Player.legVelocity.Y = Main.rand.Next(-40, -10) * 0.1f;
        Player.headVelocity.X = Main.rand.Next(-20, 21) * 0.1f + 0f;
        Player.bodyVelocity.X = Main.rand.Next(-20, 21) * 0.1f + 0f;
        Player.legVelocity.X = Main.rand.Next(-20, 21) * 0.1f + 0f;
        if (Player.stoned)
        {
            Player.headPosition = Vector2.Zero;
            Player.bodyPosition = Vector2.Zero;
            Player.legPosition = Vector2.Zero;
        }
        for (int j = 0; j < 100; j++)
        {
            Dust.NewDust(Player.position, Player.width, Player.height, DustID.LifeDrain, 0f, -2f);
        }
        Player.mount.Dismount(base.Player);
        Player.dead = true;
        Player.respawnTimer = 600;
        if (Main.expertMode)
        {
            Player.respawnTimer = (int)(Player.respawnTimer * 1.5);
        }
        Player.immuneAlpha = 0;
        Player.palladiumRegen = false;
        Player.iceBarrier = false;
        Player.crystalLeaf = false;
        PlayerDeathReason playerDeathReason = PlayerDeathReason.ByOther(Player.Male ? 14 : 15);

        if (reducedDepthDiff > (maxDepth + 50) && LE && Player.OceanMan() && Player.whoAmI == Main.myPlayer)
        {
            if (LuneLib.LuneLib.instance.LuneLibAssetsLoaded)
                CopyrightSound();
            else
                Sound();
            playerDeathReason = PlayerDeathReason.ByCustomReason(GetText("Status.Death.PressureDeathEdith").ToNetworkText(Player.name));
        }
        else if (reducedDepthDiff > (maxDepth + 50) && Player.LibPlayer().depthwaterPressure && Player.OceanMan() && Player.whoAmI == Main.myPlayer)
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
            playerDeathReason = PlayerDeathReason.ByCustomReason(GetText("Status.Death.PressureDeath" + Main.rand.Next(1, 9 + 1)).ToNetworkText(Player.name));
        }
        NetworkText deathText = playerDeathReason.GetDeathText(Player.name);

        if (Main.netMode == NetmodeID.MultiplayerClient && Player.whoAmI == Main.myPlayer)
        {
            NetMessage.SendPlayerDeath(Player.whoAmI, playerDeathReason, 1000, 0, pvp: false);
        }
        if (Main.netMode == NetmodeID.Server)
        {
            ChatHelper.BroadcastChatMessage(deathText, new Color(225, 25, 25));
        }
        else if (Main.netMode == NetmodeID.SinglePlayer)
        {
            Main.NewText(deathText.ToString(), 225, 25, 25);
        }
        if (Player.whoAmI == Main.myPlayer && (Player.difficulty == 0 || Player.difficulty == 3))
        {
            Player.DropCoins();
        }
        Player.DropTombstone(num, deathText, 0);
        if (Player.whoAmI == Main.myPlayer)
        {
            try
            {
                WorldGen.saveToonWhilePlaying();
            }
            catch
            {
            }
        }
    }

    public int CalcMaxDepth()
    {
        var cfg = LuneWoL.LWoLAdvancedServerSettings.ServerDepthPressure;
        maxDepth = cfg.BaseMaxDepth;

        if (Player.arcticDivingGear)
            maxDepth = (int)(cfg.BaseMaxDepth / (1f - cfg.ArcticGearDepthReduction));
        else if (Player.accDivingHelm && Player.accFlipper)
            maxDepth = (int)(cfg.BaseMaxDepth / (1f - cfg.DivingHelmDepthReduction - cfg.FlipperDepthReduction));
        else if (Player.accDivingHelm)
            maxDepth = (int)(cfg.BaseMaxDepth / (1f - cfg.DivingHelmDepthReduction));
        else if (Player.gills)
            maxDepth = (int)(cfg.BaseMaxDepth / (1f - cfg.GillsDepthReduction));

        return maxDepth;
    }

    public float CalcReducedDepth()
    {
        var cfg = LuneWoL.LWoLAdvancedServerSettings.ServerDepthPressure;
        reducedDepth = 1f;

        if (Player.arcticDivingGear)
            reducedDepth -= cfg.ArcticGearDepthReduction;
        if (Player.accDivingHelm && Player.accFlipper)
            reducedDepth -= (cfg.DivingHelmDepthReduction + cfg.FlipperDepthReduction);
        else if (Player.accDivingHelm)
            reducedDepth -= cfg.DivingHelmDepthReduction;
        if (Player.gills)
            reducedDepth -= cfg.GillsDepthReduction;
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
            modeOnePlayer.CheckWaterDepth();
            entryY = modeOnePlayer.EntryPoint.Y;
        }
        else if (UsingModeTwo)
        {
            entryY = modeTwoPlayer.CachedTopY * 16f;
            //if (Player.whoAmI == Main.myPlayer)
            //    Main.NewText($"entryY={entryY}, CachedTopY={modeTwoPlayer.CachedTopY}, EntryPointY={modeOnePlayer.EntryPoint.Y}");
        }
    }

    public override void PostUpdateMiscEffects()
    {
        if (Player.whoAmI != Main.myPlayer)
            return;

        UpdateWaterState();
    }

    public override void PostUpdateEquips()
    {
        if (Player.whoAmI != Main.myPlayer)
            return;
        if (entryY < 0 && UsingModeTwo)
            return;

        if (Player.OceanMan())
        {
            CalcMaxDepth();
            CalcReducedDepth();
            CalcTileDiff();
            CalcReducedTileDiff();
            CalcTileDiffClamped();
            CalcPressureDamage();
            CalcLightDepthDiff();

            BreathChecker();
            DamageChecker();
        }
    }

    public override void PostUpdate()
    {
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
        //if (Player.whoAmI == Main.myPlayer)
        //    Main.NewText($"[DepthDamage] mD={maxDepth}, rD={reducedDepth:F2}, rDD={reducedDepthDiff}, CDP={Player.LibPlayer().currentDepthPressure}, lDD={lightDepthDiff:F2}");
    }
}
