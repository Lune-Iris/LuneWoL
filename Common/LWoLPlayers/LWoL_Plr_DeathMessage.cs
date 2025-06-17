namespace LuneWoL.Common.LWoLPlayers;

public class LWoLPlayerDeath : ModPlayer
{
    public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
    {
        if (Player.LibPlayer().CrimtuptionzoneNight && Player.whoAmI == Main.myPlayer)
        {
            damageSource = PlayerDeathReason.ByCustomReason(GetText("Status.Death.CrimtuptionzoneDeath").ToNetworkText(Player.name));
        }

        return true;
    }
}