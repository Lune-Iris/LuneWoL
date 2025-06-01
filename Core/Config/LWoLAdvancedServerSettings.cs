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
        [Range(0f, 1f)]
        [Increment(0.01f)]
        public float GillsDepthReduction { get; set; }

        [BackgroundColor(95, 155, 160, 255)]
        [Range(0f, 1f)]
        [Increment(0.01f)]
        public float DivingHelmDepthReduction { get; set; }

        [BackgroundColor(95, 155, 160, 255)]
        [Range(0f, 1f)]
        [Increment(0.01f)]
        public float FlipperDepthReduction { get; set; }

        [BackgroundColor(95, 155, 160, 255)]
        [Range(0f, 1f)]
        [Increment(0.01f)]
        public float ArcticGearDepthReduction { get; set; }

        [BackgroundColor(95, 155, 160, 255)]
        [Range(0f, 1f)]
        [Increment(0.01f)]
        public float MermanDepthReduction { get; set; }

        [BackgroundColor(95, 155, 160, 255)]
        [Range(0f, 1f)]
        [Increment(0.01f)]
        public float BreathGillsMultiplier { get; set; }

        [BackgroundColor(95, 155, 160, 255)]
        [Range(0f, 1f)]
        [Increment(0.01f)]
        public float BreathDivingHelmMultiplier { get; set; }

        [BackgroundColor(95, 155, 160, 255)]
        [Range(0f, 1f)]
        [Increment(0.01f)]
        public float BreathArcticGearMultiplier { get; set; }

        [BackgroundColor(95, 155, 160, 255)]
        [Range(0f, 8f)]
        [Increment(0.1f)]
        public float BreathBaseDrainRate { get; set; }

        public ServerDepthPressureDented()
        {
            BaseMaxDepth = 200;
            GillsDepthReduction = 0.05f;
            DivingHelmDepthReduction = 0.1f;
            FlipperDepthReduction = 0.0f;
            ArcticGearDepthReduction = 0.15f;
            MermanDepthReduction = 0.15f;

            BreathGillsMultiplier = 0.3f;
            BreathDivingHelmMultiplier = 0.5f;
            BreathArcticGearMultiplier = 0.5f;
            BreathBaseDrainRate = 2f;
        }
    }

    [BackgroundColor(15, 60, 65, 255)]
    public DarkerNightsDented DarkerNights = new();

    [BackgroundColor(15, 60, 65, 255)]
    public ServerDepthPressureDented ServerDepthPressure = new();

    public override void OnLoaded() => LuneWoL.LWoLAdvancedServerSettings = this;
}