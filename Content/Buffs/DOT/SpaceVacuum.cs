namespace LuneWoL.Content.Buffs.DOT;

public class SpaceVacuum : ModBuff
{
    public override string Texture => "LuneWoL/Assets/Images/Buffs/DOT/SpaceVacuum";
    public override void SetStaticDefaults()
    {
        Main.debuff[Type] = true;
        Main.buffNoSave[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex) => player.LibPlayer().BoilFreeze = true;
}
