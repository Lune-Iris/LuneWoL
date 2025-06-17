namespace LuneWoL.Common.LWoLPlayers;

public partial class LWoLPlayer : ModPlayer
{
    public void ApplySpaceVacuum()
    {
        var Config = LuneWoL.LWoLServerConfig.BiomeSpecific;
        if (!Config.SpacePain) return;
        if (Player.whoAmI != Main.myPlayer) return;
        if (!Player.ZoneSkyHeight) return;
        if (Player.behindBackWall) return;

        Main.buffNoTimeDisplay[ModContent.BuffType<SpaceVacuum>()] = true;
        Player.AddBuff(ModContent.BuffType<SpaceVacuum>(), 15, true, false);
    }

    public void HellIsHot()
    {
        var Config = LuneWoL.LWoLServerConfig.BiomeSpecific;

        if (!Config.HellIsHot) return;
        if (!Player.ZoneUnderworldHeight) return;
        if (Player.buffImmune[BuffID.Burning] || Player.fireWalk || Player.buffImmune[BuffID.OnFire] || Player.lavaImmune || Player.wet || (Player.honeyWet && !Player.lavaWet))
            return;
        Main.buffNoTimeDisplay[BuffID.OnFire] = true;
        Player.AddBuff(BuffID.OnFire, 120, false, false);
    }

    public void ViscousWater()
    {
        var Config = LuneWoL.LWoLServerConfig.WaterRelated;

        if (Player.OceanMan() && Config.SlowWater && !LL)
        {
            if (Player.velocity.Length() > 5f)
            {
                Player.velocity = Vector2.Normalize(Player.velocity) * 5f;
            }
        }
    }

    public bool ArmourRework()
    {
        var Config = LuneWoL.LWoLServerConfig.Equipment;

        if (!Config.ArmourRework) return false;
        LeadRework();

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

    public void PoisonedWater()
    {
        var Config = LuneWoL.LWoLServerConfig.WaterRelated;

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

    public void WeatherChanges()
    {
        var Config = LuneWoL.LWoLServerConfig.BiomeSpecific;

        if (!Config.WeatherPain) return;

        if ((Main.raining && Player.ZoneCrimson) || (Player.ZoneCorrupt && !Player.behindBackWall))
        {
            Main.buffNoTimeDisplay[BuffID.Bleeding] = true;
            Player.AddBuff(BuffID.Bleeding, 60, true, false);
        }

        Player.LibPlayer().LStormEyeCovered = Sandstorm.Happening && Player.ZoneDesert && !Player.behindBackWall;
        Player.blackout = Player.LibPlayer().LStormEyeCovered;

        if (!WearingFullEskimo && Main.raining && Player.ZoneSnow && !Player.behindBackWall && !Player.HasBuff(BuffID.Campfire))
        {
            if (TundraBlizzardCounter < 0)
                TundraBlizzardCounter = 0;

            TundraBlizzardCounter += 1;

            if (TundraBlizzardCounter >= 180)
                TundraBlizzardCounter = 180;

            if (TundraBlizzardCounter >= 180)
            {
                Player.LibPlayer().BlizzardFrozen = true;
                Main.buffNoTimeDisplay[BuffID.Frozen] = true;
                Player.AddBuff(BuffID.Frozen, 60, true, false);
            }
        }
        else
        {
            TundraBlizzardCounter = 0;
            Player.LibPlayer().BlizzardFrozen = false;
        }
    }

    public void FreezingTundra()
    {
        var Config = LuneWoL.LWoLServerConfig.BiomeSpecific;

        if (!Config.Chilly) return;

        if (WearingFullEskimo) return;

        if (Player.ZoneSnow
            && !Player.HasBuff(BuffID.Campfire)
            && !Player.behindBackWall
            && !Player.HasBuff(BuffID.OnFire)
            && !Player.HasBuff(BuffID.Burning)
            && !Player.HasBuff(BuffID.Warmth))
        {
            if (TundraChilledCounter < 0)
                TundraChilledCounter = 0;

            TundraChilledCounter += 1;

            if (TundraChilledCounter >= 180)
                TundraChilledCounter = 180;

            if (TundraChilledCounter >= 180)
            {
                L.LibPlayer().Chilly = true;
                Main.buffNoTimeDisplay[BuffID.Chilled] = true;
                Player.AddBuff(BuffID.Chilled, 180, true, false);
            }
        }
        else
        {
            TundraChilledCounter = 0;
            L.LibPlayer().Chilly = false;
        }
    }

    public void OnlyEnterEvilAtDay()
    {
        var Config = LuneWoL.LWoLServerConfig.BiomeSpecific;

        if (Main.dayTime) return;

        if (!Config.NoEvilDayTime) return;

        L.LibPlayer().CrimtuptionzoneNight = Player.ZoneCorrupt || Player.ZoneCrimson;
    }

    public class Windproj : GlobalProjectile
    {
        public override void PostAI(Projectile Projectile)
        {
            var Config = LuneWoL.LWoLServerConfig.Main;

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

    public void MurkyWater()
    {
        if (LL) return;
        if (Player.whoAmI != Main.myPlayer) return;

        var Config = LuneWoL.LWoLServerConfig.WaterRelated;

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



    public void DeathPenaltyConsumedCrystals()
    {
        var Config = LuneWoL.LWoLServerConfig.Misc;

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

    public void DeathPenaltyConsumedFloor()
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

    public void DeathPenaltyAppliedOnRespawn()
    {
        var Config = LuneWoL.LWoLServerConfig.Misc;

        if (Config.DeathPenaltyMode != 1) return;

        LostHealth += 5;
        LostMana += 5;

        if (Player.statLifeMax2 <= 500 && Player.statLifeMax2 > 20)
        {
            HealthCache++;
        }
        if (Player.statManaMax2 <= 200 && Player.statManaMax2 > 20)
        {
            ManaCache++;
        }

        if (LostHealth >= 20 && Player.statLifeMax2 <= 400 && Player.statLifeMax2 > 100)
        {
            LostHealth = 0;
            HealthCache = 0;
            Player.ConsumedLifeCrystals--;
        }
        else if (LostHealth >= 5 && Player.statLifeMax2 > 400 && Player.statLifeMax2 <= 500)
        {
            LostHealth = 0;
            HealthCache = 0;
            Player.ConsumedLifeFruit--;
        }

        if (LostMana >= 20 && Player.statManaMax2 <= 200 && Player.statManaMax2 > 20)
        {
            LostMana = 0;
            ManaCache = 0;
            Player.ConsumedManaCrystals--;
        }
    }

    public void ResetDeathPenalty()
    {
        var Config = LuneWoL.LWoLServerConfig.Misc;

        if (Config.DeathPenaltyMode != 1) return;

        if (DeathFlag0)
        {
            HealthCache = 0;
            DeathFlag0 = false;
        }
        if (DeathFlag1)
        {
            ManaCache = 0;
            DeathFlag1 = false;
        }
    }

    public void DeathPenaltyStatmod(out StatModifier health, out StatModifier mana)
    {
        health = StatModifier.Default;
        mana = StatModifier.Default;

        var Config = LuneWoL.LWoLServerConfig.Misc;

        if (Config.DeathPenaltyMode != 1) return;

        health.Base -= HealthCache * 5;
        mana.Base -= ManaCache * 5;
    }

    public void ReciveDeathPenalty(BinaryReader rd)
    {
        var Config = LuneWoL.LWoLServerConfig.Misc;

        if (Config.DeathPenaltyMode != 1) return;

        HealthCache = rd.ReadByte();
        ManaCache = rd.ReadByte();
    }

    public void SyncDeathPenalty(int toWho, int fromWho, bool newPlayer)
    {
        var Config = LuneWoL.LWoLServerConfig.Misc;

        if (Config.DeathPenaltyMode != 1) return;

        ModPacket packet = Mod.GetPacket();
        packet.Write((byte)MessageType.dedsec);
        packet.Write((byte)Player.whoAmI);
        packet.Write((byte)HealthCache);
        packet.Write((byte)ManaCache);
        packet.Send(toWho, fromWho);
    }

    public void CloneClientsDeathPenalty(ModPlayer targetCopy)
    {
        var Config = LuneWoL.LWoLServerConfig.Misc;

        if (Config.DeathPenaltyMode != 1) return;

        LWoLPlayer clone = (LWoLPlayer)targetCopy;
        clone.HealthCache = HealthCache;
        clone.ManaCache = ManaCache;
    }

    public void SendDeathPenalty(ModPlayer clientPlayer)
    {
        var Config = LuneWoL.LWoLServerConfig.Misc;

        if (Config.DeathPenaltyMode != 1) return;

        LWoLPlayer clone = (LWoLPlayer)clientPlayer;

        if (HealthCache != clone.HealthCache || ManaCache != clone.ManaCache)
            SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
    }

    public void SaveDeathPenaltyTag(TagCompound tag)
    {
        var Config = LuneWoL.LWoLServerConfig.Misc;

        if (Config.DeathPenaltyMode != 1) return;

        tag["LostHealth"] = LostHealth;
        tag["LostMana"] = LostMana;
        tag["HealthCache"] = HealthCache;
        tag["ManaCache"] = ManaCache;
    }

    public void LoadDeathPenaltyTag(TagCompound tag)
    {
        var Config = LuneWoL.LWoLServerConfig.Misc;

        if (Config.DeathPenaltyMode != 1) return;

        LostHealth = tag.GetInt("LostHealth");
        LostMana = tag.GetInt("LostMana");
        HealthCache = tag.GetInt("HealthCache");
        ManaCache = tag.GetInt("ManaCache");
    }
}
