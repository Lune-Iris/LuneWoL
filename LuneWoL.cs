using LuneWoL.Core.Config;
using LuneWoL.LWoL_IL_Edits;
using Terraria.ModLoader;

namespace LuneWoL
{
    public partial class LuneWoL : Mod
    {
        public static Mod Instance;
        public static LuneWoL instance;
        public static LWoLServerConfig LWoLServerConfig;
        public static LWoLClientConfig LWoLClientConfig;
        public static LWoLServerStatConfig LWoLServerStatConfig;

        private Mod clam;

        public void InitializeGlizzy()
        {
            if (ModLoader.TryGetMod("CalamityMod", out Mod result))
            {
                clam = result;
            }
            else
            {
                clam = null;
            }
        }

        public override void Load()
        {
            instance = this;
            Instance = this;

            // compat issues im guessing (havent tried)
            if (LuneLib.LuneLib.instance.StrongerReforgesLoaded && LWoLServerConfig.Equipment.ReforgeNerf)
            {
                throw new System.Exception("Disable `Reforge Nerf` in the config if you wanna use the `Stronger Reforges` mod.\n" + new string('\n', 20));
            }

            // same as reforge thing
            if (LuneLib.LuneLib.instance.DarkSurfaceLoaded && LWoLServerConfig.Main.DarkerNightsMode != 0)
            {
                throw new System.Exception("Disable `Darker Nights` in the config if you wanna use the `Dark Surface` mod.\n" + new string('\n', 20));
            }

            LWoLILEdits.LoadIL();
        }

        public override void Unload()
        {
            Instance = null;
            instance = null;
            LWoLServerConfig = null;
            LWoLClientConfig = null;
            LWoLServerStatConfig = null;
        }
    }
}
