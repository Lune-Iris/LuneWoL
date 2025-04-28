using System;
using Terraria.ModLoader.Config;

namespace LuneWoL.Core.Config
{
    [BackgroundColor(5, 30, 50, 255)]
    public class LWoLServerConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        public override bool NeedsReload(ModConfig pendingConfig)
        {
            if (pendingConfig is not LWoLServerConfig newConfig)
                return base.NeedsReload(pendingConfig);

            return 
                   !Equipment.Equals(newConfig.Equipment) ||
                   !Recipes.Equals(newConfig.Recipes) ||
                   !Tiles.Equals(newConfig.Tiles) ||
                   !NPCs.Equals(newConfig.NPCs) ||
                   !WaterRelated.Equals(newConfig.WaterRelated) ||
                   !Misc.Equals(newConfig.Misc);
        }

        [SeparatePage]
        public class MainDented
        {
            [BackgroundColor(155, 170, 205, 255)]
            [SliderColor(155, 170, 205, 255)]
            [Slider]
            [Range(0, 4)]
            public int CritFailMode { get; set; }

            [BackgroundColor(155, 170, 205, 255)]
            [Slider]
            [SliderColor(155, 170, 205, 255)]
            [Range(0, 2)]
            public int DarkerNightsMode { get; set; }

            [BackgroundColor(155, 170, 205, 255)]
            public bool WindArrows { get; set; }

            [BackgroundColor(155, 170, 205, 255)]
            public bool DemonMode { get; set; }

            public MainDented()
            {
                CritFailMode = 0;
                DarkerNightsMode = 0;
                WindArrows = false;
                DemonMode = false;
            }
        }

        [SeparatePage]
        public class BiomeSpecificDented
        {
            [BackgroundColor(120, 135, 180, 255)]
            public bool Chilly { get; set; }

            [BackgroundColor(120, 135, 180, 255)]
            public bool HeatStroke { get; set; }

            [BackgroundColor(120, 135, 180, 255)]
            public bool HellIsHot { get; set; }

            [BackgroundColor(120, 135, 180, 255)]
            public bool NoEvilDayTime { get; set; }

            [BackgroundColor(120, 135, 180, 255)]
            public bool SpacePain { get; set; }

            [BackgroundColor(120, 135, 180, 255)]
            public bool WeatherPain { get; set; }

            public BiomeSpecificDented()
            {
                Chilly = false;
                HeatStroke = false;
                HellIsHot = false;
                NoEvilDayTime = false;
                SpacePain = false;
                WeatherPain = false;
            }
        }

        [SeparatePage]
        public class BuffsAndDebuffsDented
        {
            [BackgroundColor(80, 100, 150, 255)]
            [Slider]
            [SliderColor(80, 100, 150, 255)]
            [Range(0, 100)]
            public int HealingPotionBadPercent { get; set; }

            public BuffsAndDebuffsDented()
            {
                HealingPotionBadPercent = 100;
            }
        }

        [SeparatePage]
        public class EquipmentDented
        {
            [BackgroundColor(40, 70, 125, 255)]
            public bool ArmourRework { get; set; }

            [BackgroundColor(40, 70, 125, 255)]
            [ReloadRequired]
            public bool DisableAutoReuse { get; set; }

            [BackgroundColor(40, 70, 125, 255)]
            [ReloadRequired]
            public bool NoAccessories { get; set; }

            [BackgroundColor(40, 70, 125, 255)]
            [ReloadRequired]
            public bool ReforgeNerf { get; set; }

            public EquipmentDented()
            {
                ArmourRework = false;
                DisableAutoReuse = false;
                NoAccessories = false;
                ReforgeNerf = false;
            }
            public override bool Equals(object obj)
            {
                return obj is EquipmentDented other &&
                       DisableAutoReuse == other.DisableAutoReuse &&
                       NoAccessories == other.NoAccessories &&
                       ReforgeNerf == other.ReforgeNerf;

            }

            public override int GetHashCode() =>
                HashCode.Combine(DisableAutoReuse, NoAccessories, ReforgeNerf);
        }

        [SeparatePage]
        public class RecipesDented
        {
            [BackgroundColor(20, 55, 110, 255)]
            [ReloadRequired]
            public bool IgnoreStacksOfOne;

            [BackgroundColor(20, 55, 110, 255)]
            [SliderColor(20, 55, 110, 255)]
            [Range(0f, 100f)]
            [Increment(1f)]
            [ReloadRequired]
            public float RecipePercent;

            public RecipesDented()
            {
                IgnoreStacksOfOne = true;
                RecipePercent = 0f;
            }

            public override bool Equals(object obj)
            {
                return obj is RecipesDented other &&
                       RecipePercent == other.RecipePercent &&
                       IgnoreStacksOfOne == other.IgnoreStacksOfOne;
            }

            public override int GetHashCode() =>
                HashCode.Combine(RecipePercent, IgnoreStacksOfOne);
        }

        [SeparatePage]
        public class TilesDented
        {
            [BackgroundColor(5, 40, 95, 255)]
            [SliderColor(5, 40, 95, 255)]
            [Range(0, 100)]
            [Increment(1)]
            [ReloadRequired]
            public int OreDestroyChance;

            public TilesDented()
            {
                OreDestroyChance = 0;
            }

            public override bool Equals(object obj)
            {
                return obj is TilesDented other &&
                       OreDestroyChance == other.OreDestroyChance;

            }

            public override int GetHashCode() =>
                HashCode.Combine(OreDestroyChance);
        }

        [SeparatePage]
        public class NPCsDented
        {
            [BackgroundColor(0, 25, 80, 255)]
            [SliderColor(0, 25, 80, 255)]
            [Range(1f, 25f)]
            [Increment(0.1f)]
            [ReloadRequired]
            public float BuyMult { get; set; }

            [BackgroundColor(0, 25, 80, 255)]
            [SliderColor(0, 25, 80, 255)]
            [Range(0f, 1f)]
            [Increment(0.1f)]
            [ReloadRequired]
            public float SellMult { get; set; }

            [BackgroundColor(0, 25, 80, 255)]
            [SliderColor(0, 25, 80, 255)]
            [Slider]
            [Range(-1, 50)]
            public int InvasionMultiplier { get; set; }

            [BackgroundColor(0, 25, 80, 255)]
            [ReloadRequired]
            public bool NeverGoldEnough { get; set; }

            [BackgroundColor(0, 25, 80, 255)]
            [SliderColor(0, 25, 80, 255)]
            [Range(0f, 1f)]
            [Increment(0.05f)]
            [ReloadRequired]
            public float NoMoneh { get; set; }

            public NPCsDented()
            {
                BuyMult = 1f;
                SellMult = 1f;
                InvasionMultiplier = -1;
                NeverGoldEnough = false;
                NoMoneh = 1f;
            }
            public override bool Equals(object obj)
            {
                return obj is NPCsDented other &&
                       BuyMult == other.BuyMult &&
                       SellMult == other.SellMult &&
                       NeverGoldEnough == other.NeverGoldEnough &&
                       NoMoneh == other.NoMoneh;
            }

            public override int GetHashCode() =>
                HashCode.Combine(BuyMult, SellMult, NeverGoldEnough, NoMoneh);
        }

        [SeparatePage]
        public class WaterRelatedDented
        {
            [BackgroundColor(0, 15, 70, 255)]
            public bool DarkWaters { get; set; }

            [BackgroundColor(0, 15, 70, 255)]
            [SliderColor(0, 15, 70, 255)]
            [Slider]
            [Range(0, 1)]
            [ReloadRequired]
            public int DepthPressureMode { get; set; }

            [BackgroundColor(0, 15, 70, 255)]
            public bool SlowWater { get; set; }

            [BackgroundColor(0, 15, 70, 255)]
            public bool WaterPoison { get; set; }

            public WaterRelatedDented()
            {
                DarkWaters = false;
                DepthPressureMode = 0;
                SlowWater = false;
                WaterPoison = false;
            }
            public override bool Equals(object obj)
            {
                return obj is WaterRelatedDented other &&
                       DepthPressureMode == other.DepthPressureMode;
            }

            public override int GetHashCode() =>
                HashCode.Combine(DepthPressureMode);
        }

        [SeparatePage]
        public class MiscDented
        {
            [BackgroundColor(0, 15, 60, 255)]
            [SliderColor(0, 15, 60, 255)]
            [Slider]
            [Range(0, 3)]
            [ReloadRequired]
            public int DeathPenaltyMode { get; set; }

            [BackgroundColor(0, 15, 60, 255)]
            [Range(-1, int.MaxValue)]
            [ReloadRequired]
            public int DespawnItemsTimer { get; set; }

            [BackgroundColor(0, 15, 60, 255)]
            [ReloadRequired]
            public bool DisableWoLItems { get; set; }

            [BackgroundColor(0, 15, 60, 255)]
            [ReloadRequired]
            public bool SkillIssueMode { get; set; }

            public MiscDented()
            {
                DeathPenaltyMode = 0;
                DisableWoLItems = false;
                SkillIssueMode = false;
                DespawnItemsTimer = -1;
            }
            public override bool Equals(object obj)
            {
                return obj is MiscDented other &&
                       DeathPenaltyMode == other.DeathPenaltyMode &&
                       DespawnItemsTimer == other.DespawnItemsTimer &&
                       DisableWoLItems == other.DisableWoLItems &&
                       SkillIssueMode == other.SkillIssueMode;
            }

            public override int GetHashCode() =>
                HashCode.Combine(DeathPenaltyMode, DespawnItemsTimer, DisableWoLItems, SkillIssueMode);
        }

        [SeparatePage]
        public class CalamityDented
        {
            [BackgroundColor(0, 20, 40, 255)]
            public bool DifficultyRebuff { get; set; }

            public CalamityDented()
            {
                DifficultyRebuff = false;
            }
        }

        #region new()

        [BackgroundColor(155, 170, 205, 200)]
        public MainDented Main = new();

        [BackgroundColor(120, 135, 180, 200)]
        public BiomeSpecificDented BiomeSpecific = new();

        [BackgroundColor(80, 100, 150, 200)]
        public BuffsAndDebuffsDented BuffsAndDebuffs = new();

        [BackgroundColor(40, 70, 125, 200)]
        public EquipmentDented Equipment = new();

        [BackgroundColor(20, 55, 110, 200)]
        public RecipesDented Recipes = new();
        
        [BackgroundColor(5, 40, 95, 200)]
        public TilesDented Tiles = new();

        [BackgroundColor(0, 25, 80, 200)]
        public NPCsDented NPCs = new();

        [BackgroundColor(0, 15, 70, 200)]
        public WaterRelatedDented WaterRelated = new();

        [BackgroundColor(0, 15, 60, 200)]
        public MiscDented Misc = new();

        [BackgroundColor(0, 20, 40, 200)]
        public CalamityDented CalamityMod = new();

        //spiritmod 15 15 15
        //[BackgroundColor(15, 15, 15, 255)]
        //public SpiritDented Spirit = new();

        #endregion

        public override void OnLoaded()
        {
            LuneWoL.LWoLServerConfig = this;
        }
    }
}
