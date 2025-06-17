namespace LuneWoL.Common.LWoLSystems;

public partial class LWoLSystem : ModSystem
{
    private int _currentMoonPhase;
    private bool _wasDaytime = true;

    public override void PostUpdateWorld()
    {
        if (Main.dayTime && !_wasDaytime)
        {
            _currentMoonPhase = (_currentMoonPhase + 1) % 8;
            Main.moonPhase = _currentMoonPhase;
        }
        _wasDaytime = Main.dayTime;
    }

    public void DarkerNightsSurfaceLight(ref Color tileColor, ref Color backgroundColor)
    {
        if (Main.dayTime) return;

        var cfg = LuneWoL.LWoLServerConfig.Environment;
        var Acfg = LuneWoL.LWoLAdvancedServerSettings.DarkerNights;
        float moonMultiplier = GetMoonPhaseMultiplier(_currentMoonPhase);

        const float nightLength = 32400f;
        float fadeTicks = MathHelper.Clamp(Acfg.NightFadeDuration * 60f, 0f, nightLength / 2f);
        float t = (float)Main.time;

        float minB = cfg.DarkerNightsMode == 2 ? Acfg.MinBrightness : 1f;
        if (cfg.DarkerNightsMode == 1) minB *= moonMultiplier;

        float brightness = (t <= fadeTicks)
            ? MathHelper.Lerp(1f, minB, t / fadeTicks)
            : (t >= nightLength - fadeTicks)
                ? MathHelper.Lerp(minB, 1f, (t - (nightLength - fadeTicks)) / fadeTicks)
                : minB;

        tileColor = ToColour(tileColor.ToVector3() * brightness);
        backgroundColor = ToColour(backgroundColor.ToVector3() * brightness);
    }

    private static float GetMoonPhaseMultiplier(int phase)
    {
        var Acfg = LuneWoL.LWoLAdvancedServerSettings.DarkerNights;
        return phase switch
        {
            0 => Acfg.MoonPhases.FullMoonMult,
            1 => Acfg.MoonPhases.WaningGibbousMult,
            2 => Acfg.MoonPhases.ThirdQuarterMult,
            3 => Acfg.MoonPhases.WaningCrescentMult,
            4 => Acfg.MoonPhases.NewMoonMult,
            5 => Acfg.MoonPhases.WaxingCrescentMult,
            6 => Acfg.MoonPhases.FirstQuarterMult,
            7 => Acfg.MoonPhases.WaxingGibbousMult,
            _ => 1f,
        };
    }

    public static Color ToColour(Vector3 v) =>
        new((int)(v.X * 255), (int)(v.Y * 255), (int)(v.Z * 255));
}
