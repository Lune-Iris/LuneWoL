using Terraria;
using Terraria.ModLoader;

namespace LuneWoL.Common.LWoLGlobalItems
{
    public partial class WoLGlobalItems : GlobalItem
    {
        public void NoReusing(Item item)
        {
            if (LuneWoL.LWoLServerConfig.Equipment.DisableAutoReuse)
                item.autoReuse = false;
        }
    }
}
