namespace LuneWoL.PressureCheckFolder.Mode1;

public partial class PressureModeOne : ModPlayer
{
    public override bool IsLoadingEnabled(Mod mod) => !LuneLib.LuneLib.instance.CalamityModLoaded
            &&
            LuneWoL.LWoLServerConfig.WaterRelated.DepthPressureMode == 1;

    public override void PostUpdateMiscEffects()
    {
        if (Player.whoAmI != Main.myPlayer) return;
        CheckWaterDepth();
        if (LuneLib.LuneLib.clientConfig.DebugMessages)
        {
            Main.NewText($"W = {InWaterBody}, E = {EntryPoint.Y}, X = {ExitPoint.Y}");
        }
    }

    public override void PostUpdateEquips()
    {
        if (Player.whoAmI != Main.myPlayer) return;
        if (!LL && LP.OceanMan())
        {
            BreathChecker();
            DamageChecker();
        }
        MD();
        RD();
        RDD();
        TDC();
        TD();
        PDTA();
        LDD();
    }


    public override void PostUpdate()
    {
        if (Player.whoAmI != Main.myPlayer) return;
        if (!Player.LibPlayer().LWaterEyes) return;
        float value;
        float amount;
        value = 1f;
        amount = ModeOne.lDD;
        ScreenObstruction.screenObstruction = MathHelper.Lerp(ScreenObstruction.screenObstruction, value, amount);
        float reversedLDD = 1 - ModeOne.lDD;
        float clampedLDD = MathHelper.Clamp(reversedLDD, 0.5f, 1f);
        Lighting.GlobalBrightness *= clampedLDD;
        if (LuneLib.LuneLib.clientConfig.DebugMessages)
        {
            Main.NewText($"MD = {mD}, RD = {rD}, RDD = {rDD}, CDP = {Player.LibPlayer().currentDepthPressure}, LDD = {lDD}");
        }
    }
}
