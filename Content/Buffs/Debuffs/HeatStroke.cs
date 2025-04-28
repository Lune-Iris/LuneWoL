using Terraria;
using Terraria.ModLoader;
using static LuneLib.Utilities.LuneLibUtils;

namespace LuneWoL.Content.Buffs.Debuffs
{
    public class HeatStroke : ModBuff
    {
        public override string Texture => "LuneWoL/Assets/Images/Buffs/Debuffs/HeatStroke";
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.LibPlayer().HeatStroke = true;
        }
    }
}
