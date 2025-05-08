namespace LuneWoL.Common.LWoLPlayers;

public class LWoLPlayerDeath : ModPlayer
{

    public override bool IsLoadingEnabled(Mod mod) => LuneWoL.LWoLServerConfig.WaterRelated.DepthPressureMode == 1 && !LuneLib.LuneLib.instance.CalamityModLoaded;


    [JITWhenModsEnabled("LuneLibAssets")]
    public void sound() => SoundEngine.PlaySound(DrownSound, LP.Center);

    public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
    {
        if (ModeTwo.rDD > (ModeTwo.mD + 50) && LE && Player.OceanMan() && Player.whoAmI == Main.myPlayer)
        {
            if (LuneLib.LuneLib.instance.LuneLibAssetsLoaded)
            {
                sound();
            }
            damageSource = PlayerDeathReason.ByCustomReason(GetText("Status.Death.PressureDeathEdith").ToNetworkText(Player.name));
        }
        else if (ModeTwo.rDD > (ModeTwo.mD + 50) && Player.LibPlayer().depthwaterPressure && Player.OceanMan() && Player.whoAmI == Main.myPlayer)
        {
            if (LuneLib.LuneLib.instance.LuneLibAssetsLoaded)
            {
                sound();
            }
            damageSource = PlayerDeathReason.ByCustomReason(GetText("Status.Death.PressureDeathTooDeep").ToNetworkText(Player.name));
        }
        else if (ModeTwo.tD >= 50 && Player.OceanMan() && Player.whoAmI == Main.myPlayer)
        {
            if (LuneLib.LuneLib.instance.LuneLibAssetsLoaded)
            {
                sound();
            }
            damageSource = PlayerDeathReason.ByCustomReason(GetText("Status.Death.PressureDeath" + Main.rand.Next(1, 9 + 1)).ToNetworkText(Player.name));
        }
        else if (Player.LibPlayer().CrimtuptionzoneNight && Player.whoAmI == Main.myPlayer)
        {
            damageSource = PlayerDeathReason.ByCustomReason(GetText("Status.Death.CrimtuptionzoneDeath").ToNetworkText(Player.name));
        }
        return true;
    }
}