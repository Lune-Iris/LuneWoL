using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System.Reflection;
using Terraria.ModLoader;

namespace LuneWoL.Common.LWoLSystems
{
    public partial class LWoLSystem : ModSystem
    {
        public override void PreUpdateInvasions()
        {
            LongerInvasions();
        }

        public override void AddRecipes()
        {
            AddMusicBox();
            AddCrystalRecipe();
        }

        public override void PostAddRecipes()
        {
            RecipeMulti();
        }

        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
            var c = LuneWoL.LWoLServerConfig.Main;
            if (c.DarkerNightsMode != 0)
                DarkerNightsSurfaceLight(ref tileColor, ref backgroundColor);
        }
    }
}
