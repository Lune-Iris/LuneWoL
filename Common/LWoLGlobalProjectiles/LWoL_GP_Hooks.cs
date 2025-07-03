namespace LuneWoL.Common.LWoLGlobalProjectiles;

public partial class LWoL_GP : GlobalProjectile
{
    public override void OnSpawn(Projectile Projectile, IEntitySource source)
    {
        var p = L.GetModPlayer<LWoL_Plr>();
        var Config = LuneWoL.LWoLServerConfig.LPlayer;

        if (p.DmgPlrBcCrit && Config.CritFailMode != 0 && Projectile.owner == Main.myPlayer)
        {
            Projectile.damage = 0;
            Projectile.penetrate = -1;
        }
        base.OnSpawn(Projectile, source);
    }
}
