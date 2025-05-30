namespace LuneWoL.PressureCheckFolder;

public class LWoLDepthUtils : ModPlayer
{
    public override bool IsLoadingEnabled(Mod mod) => LuneWoL.LWoLServerConfig.WaterRelated.DepthPressureMode == 1;
    public static PressureModeOne ModeOne = LP.GetModPlayer<PressureModeOne>();
}