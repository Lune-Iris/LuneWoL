namespace LuneWoL.Common.LWoLGlobalTiles;

public partial class LWoL_GT : GlobalTile
{
    private static readonly float chance = LuneWoL.LWoLServerConfig.Tiles.OreDestroyChance / 100f;

    public override bool CanDrop(int i, int j, int type) => chance == 0f
            ? base.CanDrop(i, j, type)
            : (Main.rand.NextFloat(0f, 1f) > chance || !HashSetContainsOreTile(type)) && base.CanDrop(i, j, type);
}