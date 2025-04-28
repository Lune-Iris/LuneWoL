//using HarmonyLib;
//using Microsoft.Xna.Framework;
//using System.Collections.Generic;
//using System.Formats.Asn1;
//using Terraria;
//using Terraria.ModLoader;

//namespace LuneWoL.PressureCheckFolder.Mode1
//{
//    public class PoolSys : ModSystem
//    {
//        public override bool IsLoadingEnabled(Mod mod)
//        {
//            return LuneWoL.LWoLServerConfig.DepthPressure && LuneWoL.LWoLServerConfig.DepthPressureMode == 1;
//        }

//        static List<Point> PointsToCheck = new List<Point>();

//        private void PreAddBuffer(int x, int y)
//        {
//            PointsToCheck.Add(new Point(x, y));
//        }

//        private void PreDelBuffer(int x, int y)
//        {
//            PointsToCheck.Add(new Point(x, y));
//        }

//        public override void OnModLoad()
//        {
//            var harmony = new Harmony("wol");
//            var originalAddBufferMethod = typeof(LiquidBuffer).GetMethod("AddBuffer", System.Reflection.BindingFlags.Static);
//            var originalDelBufferMethod = typeof(LiquidBuffer).GetMethod("DelBuffer", System.Reflection.BindingFlags.Static);

//            harmony.Patch(originalAddBufferMethod, prefix: new HarmonyMethod(PreAddBuffer));
//            harmony.Patch(originalDelBufferMethod, postfix: new HarmonyMethod(PreDelBuffer));

//            WorldGen.Hooks.OnWorldLoad += () =>
//            {
//                DepthPressureCheck.Pools = Pools.CreatePools();
//            };
//        }

//        public override void PreUpdateWorld()
//        {
//            PointsToCheck.Clear();
//            // Här finns Main.liquidBuffer med positioner där liquid kommer ändras.
//            // Spara ner vilka poola som är träffade av positionerna.
//        }

//        public override void PostUpdateWorld()
//        {
//            Main.NewText($"PointsToCheck size: {PointsToCheck.Count}");

//            // Liquid.Update är nu körd, och vattnet är förändrat.
//            // Bygg om pools som är träffade. (Floodfill på nytt)
//        }
//    }
//}
