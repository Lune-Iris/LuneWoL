using LuneLib.Utilities;
using LuneWoL.Content.Buffs.Debuffs;
using LuneWoL.Content.Buffs.DOT;
using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static LuneLib.Common.Players.LuneLibPlayer.LibPlayer;
using static LuneLib.Utilities.LuneLibUtils;
using static LuneWoL.LuneWoL;

namespace LuneWoL.Common.LWoLPlayers
{
    public partial class LWoLPlayer : ModPlayer
    {

        public int tundraBlizzardCounter, tundraChilledCounter, HeatStrokeCounter;

        #region methods for update update pre skeletron of eye chlututl

        #region space pain

        public void prebuffBurnFreeze()
        {
            var Config = LWoLServerConfig.BiomeSpecific;
            if (!Config.SpacePain) return;
            if (Player.whoAmI != Main.myPlayer) return;
            if (!Player.ZoneSkyHeight) return;
            if (Player.behindBackWall) return;


            Main.buffNoTimeDisplay[ModContent.BuffType<BoilFreezeDB>()] = true;
            Player.AddBuff(ModContent.BuffType<BoilFreezeDB>(), 15, true, false);
        }

        #endregion

        #region hell hot

        public void HellIsQuiteHot()
        {
            var Config = LWoLServerConfig.BiomeSpecific;

            if (!Config.HellIsHot) return;
            if (!Player.ZoneUnderworldHeight) return;
            if (Player.buffImmune[BuffID.Burning] || Player.fireWalk || Player.buffImmune[BuffID.OnFire] || Player.lavaImmune || Player.wet || Player.honeyWet && !Player.lavaWet)
            return;
            Main.buffNoTimeDisplay[BuffID.OnFire] = true;
            Player.AddBuff(BuffID.OnFire, 120, false, false);
        }

        #endregion

        #region slow water

        public void SlowWater()
        {
            var Config = LWoLServerConfig.WaterRelated;

            if (Player.OceanMan() && Config.SlowWater && !LL)
            {
                if (Player.velocity.Length() > 8f)
                {
                    Player.velocity = Vector2.Normalize(Player.velocity) * 8f;
                }
            }
        }

        #endregion

        #region Armour Rework

        public bool ArmourReworked()
        {
            var Config = LWoLServerConfig.Equipment;

            if (!Config.ArmourRework) return false;

            LeadRework();

            return true;

        }

        public bool ArmourReworkedMove()
        {
            var Config = LWoLServerConfig.Equipment;

            if (!Config.ArmourRework) return false;

            TungstenReworkMove();

            return true;

        }

        public void LeadRework()
        {
            if (WearingFullLead || WearingTwoLeadPieces || WearingOneLeadPiece)
            {
                Player.LibPlayer().LeadPoison = true;

                Main.buffNoTimeDisplay[BuffID.Poisoned] = true;
                Player.AddBuff(BuffID.Poisoned, 5);
            }
        }

        public void TungstenReworkMove()
        {
            if (WearingFullTungsten)
            {
                Player.maxRunSpeed *= 0.6f;
                Player.accRunSpeed *= 0.6f;
            }
            else if (WearingTwoTungstenPieces)
            {
                Player.maxRunSpeed *= 0.7f;
                Player.accRunSpeed *= 0.7f;
            }
            else if (WearingOneTungstenPiece)
            {
                Player.maxRunSpeed *= 0.8f;
                Player.accRunSpeed *= 0.8f;
            }
        }

        #endregion

        #region water poison

        public void WaterPoison()
        {
            var Config = LWoLServerConfig.WaterRelated;

            if (!Config.WaterPoison) return;
            if (!Player.wet || Player.lavaWet || Player.honeyWet) return;
            if (LL) return;

            if (Player.ZoneCrimson)
            {
                Main.buffNoTimeDisplay[BuffID.Ichor] = true;
                Player.AddBuff(BuffID.Ichor, 180, true, false);
                if (Player.buffTime[BuffID.Ichor] > 180)
                {
                    Player.buffTime[BuffID.Ichor] = 180;
                }
            }
            else if (Player.ZoneCorrupt)
            {
                Main.buffNoTimeDisplay[BuffID.CursedInferno] = true;
                Player.AddBuff(BuffID.CursedInferno, 180, true, false);
                if (Player.buffTime[BuffID.CursedInferno] > 180)
                {
                    Player.buffTime[BuffID.CursedInferno] = 180;
                }
            }
            else if (Player.ZoneJungle)
            {
                Main.buffNoTimeDisplay[BuffID.Poisoned] = true;
                Player.AddBuff(BuffID.Poisoned, 180, true, false);
                if (Player.buffTime[BuffID.Poisoned] > 180)
                {
                    Player.buffTime[BuffID.Poisoned] = 180;
                }
            }
            else if (Player.ZoneHallow)
            {
                Main.buffNoTimeDisplay[BuffID.Confused] = true;
                Player.AddBuff(BuffID.Confused, 180, true, false);
                if (Player.buffTime[BuffID.Confused] > 180)
                {
                    Player.buffTime[BuffID.Confused] = 180;
                }
            }
        }

        #endregion

        #region WeatherPain

        public void WeatherPain()
        {
            var Config = LWoLServerConfig.BiomeSpecific;

            if (!Config.WeatherPain) return;

            if (Main.raining && Player.ZoneCrimson || Player.ZoneCorrupt && !Player.behindBackWall)
            {
                Main.buffNoTimeDisplay[BuffID.Bleeding] = true;
                Player.AddBuff(BuffID.Bleeding, 60, true, false);
            }

            Player.LibPlayer().LStormEyeCovered = Sandstorm.Happening && Player.ZoneDesert && !Player.behindBackWall ? true : false;
            Player.blackout = Player.LibPlayer().LStormEyeCovered;

            // Check for blizzard and tundra conditions
            if (!WearingFullEskimo && Main.raining && Player.ZoneSnow && !Player.behindBackWall && !Player.HasBuff(BuffID.Campfire))
            {
                if (tundraBlizzardCounter < 0)
                    tundraBlizzardCounter = 0;

                tundraBlizzardCounter += 1;

                if (tundraBlizzardCounter >= 180)
                    tundraBlizzardCounter = 180;

                if (tundraBlizzardCounter >= 180)
                {
                    Player.LibPlayer().BlizzardFrozen = true;
                    Main.buffNoTimeDisplay[BuffID.Frozen] = true;
                    Player.AddBuff(BuffID.Frozen, 60, true, false);
                }
            }
            else
            {
                tundraBlizzardCounter = 0;
                Player.LibPlayer().BlizzardFrozen = false;
            }
        }

        #endregion

        #region Bad Potions
        // chance of potions being "bad" giving you a debuff instead?

        //public void BadPotions()
        //{
        //}

        #endregion

        #region Cold Make Cold Brrrrr

        // cold env give chilled debuff?
        public void ColdMakeColdBrrrrr()
        {
            var Config = LWoLServerConfig.BiomeSpecific;

            if (!Config.Chilly) return;

            if (WearingFullEskimo) return;

            if (Player.ZoneSnow && !Player.HasBuff(BuffID.Campfire) && !Player.behindBackWall && !Player.HasBuff(BuffID.OnFire) && !Player.HasBuff(BuffID.Burning))
            {
                if (tundraChilledCounter < 0)
                    tundraChilledCounter = 0;

                tundraChilledCounter += 1;

                if (tundraChilledCounter >= 180)
                    tundraChilledCounter = 180;

                if (tundraChilledCounter >= 180)
                {
                    L.LibPlayer().Chilly = true;
                    Main.buffNoTimeDisplay[BuffID.Chilled] = true;
                    Player.AddBuff(BuffID.Chilled, 180, true, false);
                }
            }
            else
            {
                tundraChilledCounter = 0;
                L.LibPlayer().Chilly = false;
            }
        }

        #endregion

        #region EVIL NIGHT TIME BABY

        // evil biomes are day time only?
        public void Thisissoevillmfao()
        {
            var Config = LWoLServerConfig.BiomeSpecific;

            if (Main.dayTime) return;

            if (!Config.NoEvilDayTime) return;

            if (Player.ZoneCorrupt || Player.ZoneCrimson)
            {
                L.LibPlayer().CrimtuptionzoneNight = true;
            }
            else
            {
                L.LibPlayer().CrimtuptionzoneNight = false;
            }
        }

        #endregion

        #region wind fucks w arrows
        public class Windproj : GlobalProjectile
        {
            public override void PostAI(Projectile Projectile)
            {
                var Config = LWoLServerConfig.Main;

                if (Config.WindArrows && Projectile.arrow &&
                    Projectile.Center.Y < Main.worldSurface * 16.0
                    && Main.tile[(int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16] != null
                    && Main.tile[(int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16].WallType == 0
                    && ((Projectile.velocity.X > 0f
                    && Main.windSpeedCurrent < 0f)
                    || (Projectile.velocity.X < 0f
                    && Main.windSpeedCurrent > 0f)
                    || Math.Abs(Projectile.velocity.X) < Math.Abs(Main.windSpeedCurrent * Main.windPhysicsStrength) * 180f)
                    && Math.Abs(Projectile.velocity.X) < 16f)
                {
                    Projectile.velocity.X += Main.windSpeedCurrent * Main.windPhysicsStrength;
                    MathHelper.Clamp(Projectile.velocity.X, -16f, 16f); ;
                }
                base.PostAI(Projectile);
            }
        }

        #endregion

        #region Heat Exhaustion

        // new "Heat Exhaustion" debuff increases mana costs and decreases max summons and move speed and attack speed
        public void HeatExhaustion()
        {
            var Config = LWoLServerConfig.BiomeSpecific;

            if (!Config.HeatStroke) return;

            if (Main.dayTime && WearingAnyArmour && !Player.behindBackWall && !(Player.wet || Player.HasBuff(BuffID.Wet)) && Player.ZoneDesert)
            {
                if (HeatStrokeCounter < 0)
                    HeatStrokeCounter = 0;

                HeatStrokeCounter += 1;

                if (HeatStrokeCounter >= 180)
                    HeatStrokeCounter = 180;

                if (HeatStrokeCounter >= 180)
                {
                    Main.buffNoTimeDisplay[ModContent.BuffType<HeatStroke>()] = true;
                    Player.AddBuff(ModContent.BuffType<HeatStroke>(), 60);
                    Player.statDefense *= 0.8f;
                    Player.statLifeMax2 /= 4;
                    Player.statManaMax2 /= 4;
                    Player.GetAttackSpeed(DamageClass.Generic) *= 0.8f;
                }
            }
            else
            {
                HeatStrokeCounter -= 1;
                if (HeatStrokeCounter < 0)
                    HeatStrokeCounter = 0;
            }
        }
        public void HeatExhaustionUpdEquips()
        {
            if (HeatStrokeCounter >= 180)
            {
                Player.maxMinions /= 4;
            }
        }

        public void HeatExhaustionUpdRunSpeed()
        {
            if (HeatStrokeCounter >= 180)
            {
                Player.accRunSpeed *= 0.8f;
                Player.maxRunSpeed *= 0.8f;
            }
        }

        #endregion

        #region darker waters

        public void DarkWaters()
        {
            if (!Wait(5000)) return;
            if (LL) return;
            if (Player.whoAmI != Main.myPlayer) return;

            var Config = LWoLServerConfig.WaterRelated;

            if (Player.OceanMan() && Config.DarkWaters && Config.DepthPressureMode > 0)
            {
                Player.LibPlayer().LWaterEyes = true;
            }
            else if (Player.OceanMan() && Config.DarkWaters && Config.DepthPressureMode != 0)
            {
                Player.LibPlayer().LWaterEyes = true;
                Lighting.GlobalBrightness *= 0.8f;
            }
            else
            {
                Player.LibPlayer().LWaterEyes = false;
            }
        }

        #endregion

        #region asdasd

        public int hplosthuhhhhh;
        public int bmananalosthuhhhhh;

        public int JesusStopDyingMoron;
        public int JesusStopDyingMoronPART2EVILDARKANDTWISTED;

        public static bool AUURHGHRUGH = false;
        public static bool AUURHGHRUGHpart2ofc = false;

        public void okback2()
        {
            var Config = LWoLServerConfig.Misc;

            if (Config.DeathPenaltyMode != 2) return;

            if (Player.statLifeMax2 <= 400 && Player.statLifeMax2 > 100)
            {
                Player.ConsumedLifeCrystals--;
            }
            else if (Player.statLifeMax2 > 400 && Player.statLifeMax2 <= 500)
            {
                Player.ConsumedLifeFruit--;
            }

            if (Player.statManaMax2 <= 200 && Player.statManaMax2 >= 5)
            {
                Player.ConsumedManaCrystals--;
            }
        }

        public void THEBLACKONE()
        {
            var Config = LuneWoL.LWoLServerConfig.Misc;

            if (Config.DeathPenaltyMode != 3) return;

            if (Player.statLifeMax2 <= 400 && Player.statLifeMax2 > 100)
            {
                Player.ConsumedLifeCrystals = 0;
            }
            else if (Player.statLifeMax2 > 400 && Player.statLifeMax2 <= 500)
            {
                Player.ConsumedLifeFruit = 0;
            }

            if (Player.statManaMax2 <= 200 && Player.statManaMax2 >= 5)
            {
                Player.ConsumedManaCrystals = 0;
            }
        }

        public void ActualBrainrotCodeOnRespawn()
        {
            var Config = LuneWoL.LWoLServerConfig.Misc;

            if (Config.DeathPenaltyMode != 1) return;

            hplosthuhhhhh += 5;
            bmananalosthuhhhhh += 5;

            if (Player.statLifeMax2 <= 500 && Player.statLifeMax2 > 20)
            {
                JesusStopDyingMoron++;
            }
            if (Player.statManaMax2 <= 200 && Player.statManaMax2 > 20)
            {
                JesusStopDyingMoronPART2EVILDARKANDTWISTED++;
            }

            if (hplosthuhhhhh >= 20 && Player.statLifeMax2 <= 400 && Player.statLifeMax2 > 100)
            {
                hplosthuhhhhh = 0;
                JesusStopDyingMoron = 0;
                Player.ConsumedLifeCrystals--;
            }
            else if (hplosthuhhhhh >= 5 && Player.statLifeMax2 > 400 && Player.statLifeMax2 <= 500)
            {
                hplosthuhhhhh = 0;
                JesusStopDyingMoron = 0;
                Player.ConsumedLifeFruit--;
            }

            if (bmananalosthuhhhhh >= 20 && Player.statManaMax2 <= 200 && Player.statManaMax2 > 20)
            {
                bmananalosthuhhhhh = 0;
                JesusStopDyingMoronPART2EVILDARKANDTWISTED = 0;
                Player.ConsumedManaCrystals--;
            }
        }

        public void uhghhhghhg()
        {
            var Config = LuneWoL.LWoLServerConfig.Misc;

            if (Config.DeathPenaltyMode != 1) return;

            if (AUURHGHRUGH)
            {
                JesusStopDyingMoron = 0;
                AUURHGHRUGH = false;
            }
            if (AUURHGHRUGHpart2ofc)
            {
                JesusStopDyingMoronPART2EVILDARKANDTWISTED = 0;
                AUURHGHRUGHpart2ofc = false;
            }
        }

        public void ActualBrainrotCodeFuckThisImTired(out StatModifier AUGH, out StatModifier ARGHHH)
        {
            AUGH = StatModifier.Default;
            ARGHHH = StatModifier.Default;

            var Config = LuneWoL.LWoLServerConfig.Misc;

            if (Config.DeathPenaltyMode != 1) return;

            AUGH.Base -= JesusStopDyingMoron * 5;
            ARGHHH.Base -= JesusStopDyingMoronPART2EVILDARKANDTWISTED * 5;
        }
        public void MistaWhiteImInFortnite(BinaryReader gaygaygaygay)
        {
            var Config = LuneWoL.LWoLServerConfig.Misc;

            if (Config.DeathPenaltyMode != 1) return;

            JesusStopDyingMoron = gaygaygaygay.ReadByte();
            JesusStopDyingMoronPART2EVILDARKANDTWISTED = gaygaygaygay.ReadByte();
        }
        public void syncthething(int toWho, int fromWho, bool newPlayer)
        {
            var Config = LuneWoL.LWoLServerConfig.Misc;

            if (Config.DeathPenaltyMode != 1) return;

            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)MessageType.dedsec);
            packet.Write((byte)Player.whoAmI);
            packet.Write((byte)JesusStopDyingMoron);
            packet.Write((byte)JesusStopDyingMoronPART2EVILDARKANDTWISTED);
            packet.Send(toWho, fromWho);
        }

        public void copytheclientthing(ModPlayer targetCopy)
        {
            var Config = LuneWoL.LWoLServerConfig.Misc;

            if (Config.DeathPenaltyMode != 1) return;

            LWoLPlayer clone = (LWoLPlayer)targetCopy;
            clone.JesusStopDyingMoron = JesusStopDyingMoron;
            clone.JesusStopDyingMoronPART2EVILDARKANDTWISTED = JesusStopDyingMoronPART2EVILDARKANDTWISTED;
        }

        public void sendtheclientthing(ModPlayer clientPlayer)
        {
            var Config = LuneWoL.LWoLServerConfig.Misc;

            if (Config.DeathPenaltyMode != 1) return;

            LWoLPlayer clone = (LWoLPlayer)clientPlayer;

            if (JesusStopDyingMoron != clone.JesusStopDyingMoron || JesusStopDyingMoronPART2EVILDARKANDTWISTED != clone.JesusStopDyingMoronPART2EVILDARKANDTWISTED)
                SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
        }
        public void ActualBrainrotCodeTHISAGUST12TH2025(TagCompound gayshitismyshit)
        {
            var Config = LuneWoL.LWoLServerConfig.Misc;

            if (Config.DeathPenaltyMode != 1) return;

            gayshitismyshit["hplosthuhhhhh"] = hplosthuhhhhh;
            gayshitismyshit["bmananalosthuhhhhh"] = bmananalosthuhhhhh;
            gayshitismyshit["JesusStopDyingMoron"] = JesusStopDyingMoron;
            gayshitismyshit["JesusStopDyingMoronPART2EVILDARKANDTWISTED"] = JesusStopDyingMoronPART2EVILDARKANDTWISTED;
        }
        public void ActualBrainrotCodebutcooler(TagCompound incestiswincest) // cool bug where if you just rejoin it resets the thingy thing im not fixing it because if you go through the pain of rejoining for that extra hp you deserve it
        {
            var Config = LuneWoL.LWoLServerConfig.Misc;

            if (Config.DeathPenaltyMode != 1) return;

            hplosthuhhhhh = incestiswincest.GetInt("hplosthuhhhhh");
            bmananalosthuhhhhh = incestiswincest.GetInt("bmananalosthuhhhhh");
            JesusStopDyingMoron = incestiswincest.GetInt("JesusStopDyingMoron");
            JesusStopDyingMoronPART2EVILDARKANDTWISTED = incestiswincest.GetInt("JesusStopDyingMoronPART2EVILDARKANDTWISTED");
        }

        #endregion

        #endregion
    }
}
