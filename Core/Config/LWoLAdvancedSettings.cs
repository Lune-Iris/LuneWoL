namespace LuneWoL.Core.Config;

[BackgroundColor(5, 40, 40, 255)]
public class LWoLAdvancedSettings : ModConfig
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

    [BackgroundColor(15, 60, 65, 255)]
    public DarkerNightsDented DarkerNights = new();

    public override void OnLoaded() => LuneWoL.LWoLAdvancedSettings = this;
}