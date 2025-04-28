using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace LuneWoL.Core.Config
{
    [BackgroundColor(10, 75, 105, 255)]
    public class LWoLClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [BackgroundColor(35, 115, 145, 255)]
        [DefaultValue(false)]
        public bool STFUCHAT { get; set; }

        public override void OnLoaded()
        {
            LuneWoL.LWoLClientConfig = this;
        }
    }
}
