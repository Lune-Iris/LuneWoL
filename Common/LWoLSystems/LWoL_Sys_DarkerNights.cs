using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace LuneWoL.Common.LWoLSystems
{
    public partial class LWoLSystem : ModSystem
    {
        /*
            modified code from the darker surface mod to allow for moon phase changes
            if the author of the mod sees this feel free reach out if you want me to remove it
            or contact me if you see any issues with the calculations. i got confused halfway through and called it a day
         */

        private int _currentMoonPhase = -1;
        private bool asd = false, asdas = false;

        public override void OnWorldLoad()
        {
            base.OnWorldLoad();
            _currentMoonPhase = Main.moonPhase;
        }

        public void DarkerNightsSurfaceLight(ref Color tileColor, ref Color backgroundColor)
        {
            var c = LuneWoL.LWoLServerConfig.Main;
            float moonPhaseMultiplier = GetMoonPhaseMultiplier(_currentMoonPhase);

            float dayGrad = GradFloat((float)Main.time, 0f, 54000f);
            float nightGrad = GradFloat((float)Main.time, 0f, 32400f);

            dayGrad = MathHelper.Clamp(dayGrad, 0.1f, 1f);
            nightGrad = MathHelper.Clamp(nightGrad, 0.1f, 1f);

            if (Main.dayTime)
            {
                tileColor = ToColour(tileColor.ToVector3() * dayGrad);
                backgroundColor = ToColour(backgroundColor.ToVector3() * dayGrad);
            }
            else if (c.DarkerNightsMode == 1)
            {
                tileColor = ToColour(tileColor.ToVector3() * (nightGrad * moonPhaseMultiplier));
                backgroundColor = ToColour(backgroundColor.ToVector3() * (nightGrad * moonPhaseMultiplier));
            }
            else if (c.DarkerNightsMode == 2)
            {
                tileColor = ToColour(tileColor.ToVector3() * (nightGrad * 0.3f));
                backgroundColor = ToColour(backgroundColor.ToVector3() * (nightGrad * 0.3f));
            }

            if (Main.dayTime && !asd)
            {
                _currentMoonPhase = Main.moonPhase;
                asd = true;
                asdas = false;
            }
            else if (!Main.dayTime && !asdas)
            {
                asd = false;
                asdas = true;
            }
        }

        private static float GetMoonPhaseMultiplier(int moonPhase)
        {
            return moonPhase switch
            {
                0 => 1f,   // Full Moon
                1 => 0.8f, // Waning Gibbous
                2 => 0.6f, // Third Quarter
                3 => 0.4f, // Waning Crescent
                4 => 0.35f, // New Moon          lower values will cause lighting issues but you can bearly see a dif anyways
                5 => 0.4f, // Waxing Crescent
                6 => 0.6f, // First Quarter
                7 => 0.8f, // Waxing Gibbous
                _ => 1f,
            };
        }

        public static Color ToColour(Vector3 input)
        {
            return new Color((int)(input.X * 255f), (int)(input.Y * 255f), (int)(input.Z * 255f));
        }

        public static float GradFloat(float value, float min, float max)
        {
            float mid = (max + min) / 2f;
            if (value > mid)
            {
                float thing = 1f - (value - min) / (max - min) * 2f;
                return Utils.Clamp(1f + thing, 0f, 1f);
            }
            return Utils.Clamp((value - min) / (max - min) * 2f, 0f, 1f);
        }
    }
}
