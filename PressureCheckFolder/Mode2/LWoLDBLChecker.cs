using LuneLib.Utilities;
using Terraria;
using Terraria.ModLoader;
using static LuneLib.Utilities.LuneLibUtils;
using static LuneWoL.PressureCheckFolder.LWoLDepthUtils;

namespace LuneWoL.PressureCheckFolder.Mode2
{
    public partial class PressureModeTwo : ModPlayer
    {
        public int abyssBreathCD;

        public void DamageChecker()
        {
            if (ModeTwo.rDD >= ModeTwo.mD)
            {
                Player.LibPlayer().depthwaterPressure = true;
                Player.LibPlayer().currentDepthPressure = ModeTwo.pDTA;
            }
        }

        #region Breath

        public void BreathChecker()
        { 
            // stolen from clamtitty mod cause they sorta already had a system for it not really though
            double dR = ModeTwo.tD / ModeTwo.mD;

            dR *= 2D;

            double tick = 12D * (1D - dR);

            if (tick < 1D)
                tick = 1D;

            double tickMult = 1D +
                (Player.gills ? 4D : 0D) +
                (Player.ignoreWater ? 5D : 0D) +
                (Player.accDivingHelm ? 10D : 0D) +
                (Player.arcticDivingGear ? 10D : 0D) +
                (Player.accMerman ? 15D : 0D);

            if (tickMult > 50D)
                tickMult = 50D;

            tick *= tickMult / dR;

            abyssBreathCD++;
            if (abyssBreathCD >= (int)tick && ModeTwo.tD >= 2)
            {
                abyssBreathCD = 0;

                if (Player.breath > 0)
                    Player.breath -= 1;
            }
            if (Player.breath > 0)
            {
                if (Player.gills || Player.merman)
                    Player.breath -= 3;
            }

            int lifeLossAtZeroBreath = (int)(6D * ModeTwo.rD);

            if (lifeLossAtZeroBreath < 0)
                lifeLossAtZeroBreath = 0;

            if (LuneLib.LuneLib.clientConfig.DebugMessages && Player.whoAmI == Main.myPlayer)
            {
                Main.NewText($"dR = {dR}, BL = X, TM = {tickMult}, T = {tick}, LLAZB = {lifeLossAtZeroBreath}");
            }

            if (Player.breath <= 0)
            {
                Player.statLife -= lifeLossAtZeroBreath;
            }
        }

        #endregion
    }
}
