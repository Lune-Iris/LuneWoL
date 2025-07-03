namespace LuneWoL.Common.LWoLGlobalTiles;

public partial class LWoL_GT : GlobalTile
{
    public override bool CanDrop(int i, int j, int type) => LuneWoL.LWoLServerConfig.Tiles.OreDestroyChance / 100f == 0f
        ? base.CanDrop(i, j, type)
        : (Main.rand.NextFloat(0f, 1f) >= LuneWoL.LWoLServerConfig.Tiles.OreDestroyChance / 100f || !HashSetContainsOreTile(type)) && base.CanDrop(i, j, type);
}