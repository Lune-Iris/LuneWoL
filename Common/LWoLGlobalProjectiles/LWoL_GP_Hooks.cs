using static LuneLib.Utilities.LuneLibUtils;
using LuneWoL.Common.LWoLPlayers;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace LuneWoL.Common.LWoLGlobalProjectiles
{
    public partial class LWoL_GP_Hooks : GlobalProjectile
    {
        public override void OnSpawn(Projectile Projectile, IEntitySource source)
        {
            var p = L.GetModPlayer<LWoLPlayer>();
            var Config = LuneWoL.LWoLServerConfig.Main;

            if (p.DmgPlrBcCrit && Config.CritFailMode != 0 && Projectile.owner == Main.myPlayer)
            {
                Projectile.damage = 0;
                Projectile.penetrate = -1;
            }
            base.OnSpawn(Projectile, source);
        }
    }
}
