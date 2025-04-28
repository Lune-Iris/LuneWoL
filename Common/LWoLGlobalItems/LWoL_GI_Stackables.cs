using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LuneWoL.Common.LWoLGlobalItems
{
    public partial class WoLGlobalItems : GlobalItem
    {
        public void Stackables(Item item)
        {
            if (item.type == ItemID.MusicBox)
            {
                item.maxStack = 9999;
            }
        }
    }
}
