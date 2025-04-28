using Terraria;
using Terraria.ModLoader;
using static LuneLib.Utilities.LuneLibUtils;

namespace LuneWoL.Content.Buffs.DOT
{
    public class BoilFreezeDB : ModBuff
    {
        public override string Texture => "LuneWoL/Assets/Images/Buffs/DOT/speechless";
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.LibPlayer().BoilFreeze = true;
        }
    }
}
