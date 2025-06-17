namespace LuneWoL.Core.Config;

[BackgroundColor(5, 40, 40, 255)]
public class LWoLAdvancedServerSettings : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ServerSide;
    public override bool NeedsReload(ModConfig pendingConfig) => pendingConfig is not LWoLAdvancedServerSettings newConfig
        ? base.NeedsReload(pendingConfig)
        : !OreDensityCfg.Equals(newConfig.OreDensityCfg);

    [SeparatePage]
    public class DarkerNightsDented
    {
        [BackgroundColor(155, 170, 205, 255)]
        [Range(1, int.MaxValue)]
        public int NightFadeDuration { get; set; }

        [BackgroundColor(155, 170, 205, 255)]
        [Range(0f, 1f)]
        [Increment(0.05f)]
        public float MinBrightness { get; set; }

        [BackgroundColor(155, 170, 205, 255)]
        public MoonPhasesDented MoonPhases { get; set; } = new();
        public class MoonPhasesDented
        {
            [BackgroundColor(155, 170, 205, 255)]
            [Range(0f, 1f)]
            [Increment(0.05f)]
            public float FullMoonMult { get; set; }

            [BackgroundColor(155, 170, 205, 255)]
            [Range(0f, 1f)]
            [Increment(0.05f)]
            public float WaningGibbousMult { get; set; }

            [BackgroundColor(155, 170, 205, 255)]
            [Range(0f, 1f)]
            [Increment(0.05f)]
            public float ThirdQuarterMult { get; set; }

            [BackgroundColor(155, 170, 205, 255)]
            [Range(0f, 1f)]
            [Increment(0.05f)]
            public float WaningCrescentMult { get; set; }

            [BackgroundColor(155, 170, 205, 255)]
            [Range(0f, 1f)]
            [Increment(0.05f)]
            public float NewMoonMult { get; set; }

            [BackgroundColor(155, 170, 205, 255)]
            [Range(0f, 1f)]
            [Increment(0.05f)]
            public float WaxingCrescentMult { get; set; }

            [BackgroundColor(155, 170, 205, 255)]
            [Range(0f, 1f)]
            [Increment(0.05f)]
            public float FirstQuarterMult { get; set; }

            [BackgroundColor(155, 170, 205, 255)]
            [Range(0f, 1f)]
            [Increment(0.05f)]
            public float WaxingGibbousMult { get; set; }

            public MoonPhasesDented()
            {
                FullMoonMult = 1f;
                WaningGibbousMult = 0.8f;
                ThirdQuarterMult = 0.6f;
                WaningCrescentMult = 0.4f;
                NewMoonMult = 0.35f;
                WaxingCrescentMult = 0.4f;
                FirstQuarterMult = 0.6f;
                WaxingGibbousMult = 0.8f;
            }
        }
        public DarkerNightsDented()
        {
            NightFadeDuration = 60;
            MinBrightness = 0.35f;
        }
    }

    [SeparatePage]
    public class ServerDepthPressureDented
    {
        [BackgroundColor(120, 135, 180, 255)]
        [Range(0, 2000)]
        public int BaseMaxDepth { get; set; }

        [BackgroundColor(120, 135, 180, 255)]
        [Range(0f, 8f)]
        [Increment(0.1f)]
        public float BaseBreathDrainRate { get; set; }

        [BackgroundColor(120, 135, 180, 255)]
        [Range(0f, 256f)]
        [Increment(1f)]
        public float BaseTickRate { get; set; }

        [BackgroundColor(120, 135, 180, 255)]
        [Range(0f, 256f)]
        [Increment(1f)]
        public float BaseDRRate { get; set; }


        [BackgroundColor(120, 135, 180, 255)]
        public BreathValuesDented BreathValues { get; set; } = new();
        public class BreathValuesDented
        {
            [BackgroundColor(120, 135, 180, 255)]
            [Range(0f, 1f)]
            [Increment(0.01f)]
            public float BREATHMermanAddition { get; set; }

            [BackgroundColor(120, 135, 180, 255)]
            [Range(0f, 1f)]
            [Increment(0.01f)]
            public float BREATHGillsAddition { get; set; }

            [BackgroundColor(120, 135, 180, 255)]
            [Range(0f, 1f)]
            [Increment(0.01f)]
            public float BREATHDivingHelmAddition { get; set; }

            [BackgroundColor(120, 135, 180, 255)]
            [Range(0f, 1f)]
            [Increment(0.01f)]
            public float BREATHDivingGearAddition { get; set; }

            [BackgroundColor(120, 135, 180, 255)]
            [Range(0f, 1f)]
            [Increment(0.01f)]
            public float BREATHArcticGearAddition { get; set; }
            public BreathValuesDented()
            {
                BREATHGillsAddition = 0.20f;
                BREATHDivingHelmAddition = 0.10f;
                BREATHDivingGearAddition = 0.15f;
                BREATHArcticGearAddition = 0.25f;
                BREATHMermanAddition = 0.30f;
            }
        }
        [BackgroundColor(120, 135, 180, 255)]
        public DepthValuesDented DepthValues { get; set; } = new();
        public class DepthValuesDented
        {
            [BackgroundColor(120, 135, 180, 255)]
            [Range(0f, 1f)]
            [Increment(0.01f)]
            public float DEPTHGillsAddition { get; set; }

            [BackgroundColor(120, 135, 180, 255)]
            [Range(0f, 1f)]
            [Increment(0.01f)]
            public float DEPTHDivingHelmAddition { get; set; }

            [BackgroundColor(120, 135, 180, 255)]
            [Range(0f, 1f)]
            [Increment(0.01f)]
            public float DEPTHDivingGearAddition { get; set; }

            [BackgroundColor(120, 135, 180, 255)]
            [Range(0f, 1f)]
            [Increment(0.01f)]
            public float DEPTHArcticGearAddition { get; set; }

            [BackgroundColor(120, 135, 180, 255)]
            [Range(0f, 1f)]
            [Increment(0.01f)]
            public float DEPTHMermanAddition { get; set; }
            public DepthValuesDented()
            {
                DEPTHGillsAddition = 0.05f;
                DEPTHDivingHelmAddition = 0.1f;
                DEPTHDivingGearAddition = 0.15f;
                DEPTHArcticGearAddition = 0.2f;
                DEPTHMermanAddition = 0.15f;
            }
        }
        [BackgroundColor(120, 135, 180, 255)]
        public TICKValuesDented TíckValues { get; set; } = new();
        public class TICKValuesDented
        {
            [BackgroundColor(120, 135, 180, 255)]
            [Range(0f, 15f)]
            [Increment(0.01f)]
            public float TICKGillsAddition { get; set; }

            [BackgroundColor(120, 135, 180, 255)]
            [Range(0f, 15f)]
            [Increment(0.01f)]
            public float TICKDivingHelmAddition { get; set; }

            [BackgroundColor(120, 135, 180, 255)]
            [Range(0f, 15f)]
            [Increment(0.01f)]
            public float TICKDivingGearAddition { get; set; }

            [BackgroundColor(120, 135, 180, 255)]
            [Range(0f, 15f)]
            [Increment(0.01f)]
            public float TICKArcticGearAddition { get; set; }

            [BackgroundColor(120, 135, 180, 255)]
            [Range(0f, 15f)]
            [Increment(0.01f)]
            public float TICKMermanAddition { get; set; }
            public TICKValuesDented()
            {
                TICKGillsAddition = 4f;
                TICKDivingHelmAddition = 5f;
                TICKDivingGearAddition = 7.5f;
                TICKArcticGearAddition = 10f;
                TICKMermanAddition = 15f;
            }
        }
        [BackgroundColor(120, 135, 180, 255)]
        public DRValuesDented DRValues { get; set; } = new();
        public class DRValuesDented
        {
            [BackgroundColor(120, 135, 180, 255)]
            [Range(0, 10)]
            public int DRGillsAddition { get; set; }

            [BackgroundColor(120, 135, 180, 255)]
            [Range(0, 10)]
            public int DRDivingHelmAddition { get; set; }

            [BackgroundColor(120, 135, 180, 255)]
            [Range(0, 10)]
            public int DRDivingGearAddition { get; set; }

            [BackgroundColor(120, 135, 180, 255)]
            [Range(0, 10)]
            public int DRArcticGearAddition { get; set; }

            [BackgroundColor(120, 135, 180, 255)]
            [Range(0, 10)]
            public int DRMermanAddition { get; set; }
            public DRValuesDented()
            {
                DRGillsAddition = 1;
                DRDivingHelmAddition = 2;
                DRDivingGearAddition = 3;
                DRArcticGearAddition = 4;
                DRMermanAddition = 5;
            }
        }
        public ServerDepthPressureDented()
        {
            BaseMaxDepth = 250;
            BaseBreathDrainRate = 0.5f;
            BaseTickRate = 32f;
            BaseDRRate = 12f;
        }
    }

    [SeparatePage]
    public class OreDensityDented
    {
        [BackgroundColor(80, 100, 150, 255)]
        public bool DynamiteVein;

        [BackgroundColor(80, 100, 150, 255)]
        [Range(0, 100)]
        [Slider]
        public int PrehardmodeOreDensityPercent;

        [BackgroundColor(80, 100, 150, 255)]
        [Range(0, 100)]
        [Slider]
        public int PrehardmodeOreAmountPercent;

        [BackgroundColor(80, 100, 150, 255)]
        [Range(0, 100)]
        [Slider]
        [ReloadRequired]
        public int HardmodeOreDensityPercent;

        [BackgroundColor(80, 100, 150, 255)]
        [Range(0, 100)]
        [Slider]
        [ReloadRequired]
        public int HardmodeOreAmountPercent;

        [BackgroundColor(80, 100, 150, 255)]
        [Range(0, 100)]
        [Slider]
        public int GemStoneDensityPercent;

        [BackgroundColor(80, 100, 150, 255)]
        [Range(0, 100)]
        [Slider]
        public int GemStoneAmountPercent;

        [BackgroundColor(80, 100, 150, 255)]
        [Range(0, 100)]
        [Slider]
        public int SiltDensityPercent;

        [BackgroundColor(80, 100, 150, 255)]
        [Range(0, 100)]
        [Slider]
        public int SiltAmountPercent;

        [BackgroundColor(80, 100, 150, 255)]
        [Range(0, 100)]
        [Slider]
        public int SlushDensityPercent;

        [BackgroundColor(80, 100, 150, 255)]
        [Range(0, 100)]
        [Slider]
        public int SlushAmountPercent;

        public OreDensityDented()
        {
            DynamiteVein = true;
            PrehardmodeOreDensityPercent = 50;
            PrehardmodeOreAmountPercent = 50;
            HardmodeOreDensityPercent = 50;
            HardmodeOreAmountPercent = 50;
            GemStoneDensityPercent = 50;
            GemStoneAmountPercent = 50;
            SiltDensityPercent = 50;
            SiltAmountPercent = 50;
            SlushDensityPercent = 50;
            SlushAmountPercent = 50;
        }

        public override bool Equals(object obj) => obj is OreDensityDented other &&
               HardmodeOreDensityPercent == other.HardmodeOreDensityPercent &&
               HardmodeOreAmountPercent == other.HardmodeOreAmountPercent;

        public override int GetHashCode() =>
            HashCode.Combine(HardmodeOreDensityPercent, HardmodeOreAmountPercent);
    }


    [BackgroundColor(155, 170, 205, 200)]
    public DarkerNightsDented DarkerNights = new();

    [BackgroundColor(120, 135, 180, 200)]
    public ServerDepthPressureDented ServerDepthPressure = new();

    [BackgroundColor(80, 100, 150, 200)]
    public OreDensityDented OreDensityCfg = new();

    public override void OnLoaded() => LuneWoL.LWoLAdvancedServerSettings = this;
}