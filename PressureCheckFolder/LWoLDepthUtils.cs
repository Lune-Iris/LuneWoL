using LuneWoL.PressureCheckFolder.Mode2;
using Terraria.ModLoader;
using static LuneLib.Utilities.LuneLibUtils;

namespace LuneWoL.PressureCheckFolder
{
    public class LWoLDepthUtils : ModPlayer
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return LuneWoL.LWoLServerConfig.WaterRelated.DepthPressureMode == 1;
        }
        public static PressureModeTwo ModeTwo = LP.GetModPlayer<PressureModeTwo>();
    }
}
