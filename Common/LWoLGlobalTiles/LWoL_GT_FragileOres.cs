using Terraria;
using Terraria.ModLoader;
using LuneLib.Utilities;
using static LuneLib.Utilities.Hashsets.HashSets;

namespace LuneWoL.Common.LWoLGlobalTiles
{
    public partial class LWoL_GT : GlobalTile
    {
        private static readonly float chance = LuneLibUtils.ToPercentage(LuneWoL.LWoLServerConfig.Tiles.OreDestroyChance);

        public override bool CanDrop(int i, int j, int type)
        {
            if (chance == 0f) return base.CanDrop(i, j, type);
            if (Main.rand.NextFloat(0f, 1f) <= chance && HashSetContainsOreTile(type)) return false;
            else return base.CanDrop(i, j, type);
        }
    }
}