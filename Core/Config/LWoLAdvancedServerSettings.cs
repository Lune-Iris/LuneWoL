namespace LuneWoL.Core.Config;

[BackgroundColor(5, 40, 40, 255)]
public class LWoLAdvancedServerSettings : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ServerSide;

    [SeparatePage]
    public class DarkerNightsDented
    {
        [BackgroundColor(95, 155, 160, 255)]
        [Range(1, int.MaxValue)]
        public int NightFadeDuration { get; set; }

        [BackgroundColor(95, 155, 160, 255)]
        [Range(0f, 1f)]
        [Increment(0.05f)]
        public float MinBrightness { get; set; }

        [BackgroundColor(15, 60, 65, 255)]
        public MoonPhasesDented MoonPhases { get; set; } = new();
        public class MoonPhasesDented
        {
            [BackgroundColor(95, 155, 160, 255)]
            [Range(0f, 1f)]
            [Increment(0.05f)]
            public float FullMoonMult { get; set; }

            [BackgroundColor(95, 155, 160, 255)]
            [Range(0f, 1f)]
            [Increment(0.05f)]
            public float WaningGibbousMult { get; set; }

            [BackgroundColor(95, 155, 160, 255)]
            [Range(0f, 1f)]
            [Increment(0.05f)]
            public float ThirdQuarterMult { get; set; }

            [BackgroundColor(95, 155, 160, 255)]
            [Range(0f, 1f)]
            [Increment(0.05f)]
            public float WaningCrescentMult { get; set; }

            [BackgroundColor(95, 155, 160, 255)]
            [Range(0f, 1f)]
            [Increment(0.05f)]
            public float NewMoonMult { get; set; }

            [BackgroundColor(95, 155, 160, 255)]
            [Range(0f, 1f)]
            [Increment(0.05f)]
            public float WaxingCrescentMult { get; set; }

            [BackgroundColor(95, 155, 160, 255)]
            [Range(0f, 1f)]
            [Increment(0.05f)]
            public float FirstQuarterMult { get; set; }

            [BackgroundColor(95, 155, 160, 255)]
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
        [BackgroundColor(95, 155, 160, 255)]
        [Range(0, 2000)]
        public int BaseMaxDepth { get; set; }

        [BackgroundColor(95, 155, 160, 255)]
        [Range(0f, 8f)]
        [Increment(0.1f)]
        public float BaseBreathDrainRate { get; set; }
        
        [BackgroundColor(95, 155, 160, 255)]
        [Range(0f, 256f)]
        [Increment(1f)]
        public float BaseTickRate { get; set; }
        
        [BackgroundColor(95, 155, 160, 255)]
        [Range(0f, 256f)]
        [Increment(1f)]
        public float BaseDRRate { get; set; }


        [BackgroundColor(15, 60, 65, 255)]
        public BreathValuesDented BreathValues { get; set; } = new();
        public class BreathValuesDented
        {
            [BackgroundColor(95, 155, 160, 255)]
            [Range(0f, 1f)]
            [Increment(0.01f)]
            public float BREATHMermanAddition { get; set; }

            [BackgroundColor(95, 155, 160, 255)]
            [Range(0f, 1f)]
            [Increment(0.01f)]
            public float BREATHGillsAddition { get; set; }

            [BackgroundColor(95, 155, 160, 255)]
            [Range(0f, 1f)]
            [Increment(0.01f)]
            public float BREATHDivingHelmAddition { get; set; }

            [BackgroundColor(95, 155, 160, 255)]
            [Range(0f, 1f)]
            [Increment(0.01f)]
            public float BREATHDivingGearAddition { get; set; }

            [BackgroundColor(95, 155, 160, 255)]
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
        [BackgroundColor(15, 60, 65, 255)]
        public DepthValuesDented DepthValues { get; set; } = new();
        public class DepthValuesDented
        {
            [BackgroundColor(95, 155, 160, 255)]
            [Range(0f, 1f)]
            [Increment(0.01f)]
            public float DEPTHGillsAddition { get; set; }

            [BackgroundColor(95, 155, 160, 255)]
            [Range(0f, 1f)]
            [Increment(0.01f)]
            public float DEPTHDivingHelmAddition { get; set; }

            [BackgroundColor(95, 155, 160, 255)]
            [Range(0f, 1f)]
            [Increment(0.01f)]
            public float DEPTHDivingGearAddition { get; set; }

            [BackgroundColor(95, 155, 160, 255)]
            [Range(0f, 1f)]
            [Increment(0.01f)]
            public float DEPTHArcticGearAddition { get; set; }

            [BackgroundColor(95, 155, 160, 255)]
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
        [BackgroundColor(15, 60, 65, 255)]
        public TICKValuesDented TíckValues { get; set; } = new();
        public class TICKValuesDented
        {
            [BackgroundColor(95, 155, 160, 255)]
            [Range(0f, 15f)]
            [Increment(0.01f)]
            public float TICKGillsAddition { get; set; }

            [BackgroundColor(95, 155, 160, 255)]
            [Range(0f, 15f)]
            [Increment(0.01f)]
            public float TICKDivingHelmAddition { get; set; }

            [BackgroundColor(95, 155, 160, 255)]
            [Range(0f, 15f)]
            [Increment(0.01f)]
            public float TICKDivingGearAddition { get; set; }

            [BackgroundColor(95, 155, 160, 255)]
            [Range(0f, 15f)]
            [Increment(0.01f)]
            public float TICKArcticGearAddition { get; set; }

            [BackgroundColor(95, 155, 160, 255)]
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
        [BackgroundColor(15, 60, 65, 255)]
        public DRValuesDented DRValues { get; set; } = new();
        public class DRValuesDented
        {
            [BackgroundColor(95, 155, 160, 255)]
            [Range(0, 10)]
            public int DRGillsAddition { get; set; }

            [BackgroundColor(95, 155, 160, 255)]
            [Range(0, 10)]
            public int DRDivingHelmAddition { get; set; }

            [BackgroundColor(95, 155, 160, 255)]
            [Range(0, 10)]
            public int DRDivingGearAddition { get; set; }

            [BackgroundColor(95, 155, 160, 255)]
            [Range(0, 10)]
            public int DRArcticGearAddition { get; set; }

            [BackgroundColor(95, 155, 160, 255)]
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
            BaseMaxDepth = 200;
            BaseBreathDrainRate = 0.5f;
            BaseTickRate = 32f;
            BaseDRRate = 12f;
        }
    }

    [BackgroundColor(15, 60, 65, 255)]
    public DarkerNightsDented DarkerNights = new();

    [BackgroundColor(15, 60, 65, 255)]
    public ServerDepthPressureDented ServerDepthPressure = new();

    public override void OnLoaded() => LuneWoL.LWoLAdvancedServerSettings = this;
}