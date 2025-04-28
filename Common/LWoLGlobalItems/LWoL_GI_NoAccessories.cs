using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LuneWoL.Common.LWoLGlobalItems
{
    public partial class WoLGlobalItems : GlobalItem
    {
        public void NoAccessories(Item item)
        {
            var equipment = LuneWoL.LWoLServerConfig.Equipment;

            if (!equipment.NoAccessories) return;

            if (!item.vanity)
            {
                item.accessory = false;
                ItemID.Sets.CanGetPrefixes[item.type] = false;
            }
            if (item.wingSlot > -1)
            {
                ArmorIDs.Wing.Sets.Stats[item.wingSlot] = new WingStats(0, 0, 0, false, 0);
            }
        }
    }
}
