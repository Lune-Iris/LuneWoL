//using Terraria.ModLoader;
//using Terraria;

//namespace LuneWoL.PressureCheckFolder.Mode1
//{
//    public class DepthPressureCheck : ModPlayer
//    {
//        public override bool IsLoadingEnabled(Mod mod)
//        {
//            return LuneWoL.LWoLServerConfig.DepthPressure && LuneWoL.LWoLServerConfig.DepthPressureMode == 1;
//        }

//        public static Pools Pools = new Pools();

//        public Pool CurrentlyInThisPool;
//        public bool WasDrowningLastFrame { get; set; }


//        public override void PostUpdate()
//        {
//            CheckWaterDepth();
//            if (CurrentlyInThisPool == null) return;

//            float depthDifference = Player.position.Y - CurrentlyInThisPool.SurfaceY;

//            float additionalBreathLoss = depthDifference / 1000f;

//            Player.breath -= (int)additionalBreathLoss;

//            if (Player.breath < 0)
//            {
//                Player.breath = 0;
//            }
//        }

//        private void CheckWaterDepth()
//        {
//            bool currentlyDrowning = Collision.DrownCollision(Player.position, Player.width, Player.height, Player.gravDir);

//            if (currentlyDrowning)
//            {
//                if (!WasDrowningLastFrame)
//                {
//                    CurrentlyInThisPool = Pools.FindPool(Player.Center);

//                    //Main.NewText($"{CurrentlyInThisPool} {CurrentlyInThisPool.SurfaceY}. player center enter = {Player.Center}");
//                }

//                if (CurrentlyInThisPool != null)
//                {
//                    var poolSurfaceY = CurrentlyInThisPool.SurfaceY;
//                }
//            }

//            if (!currentlyDrowning && WasDrowningLastFrame) CurrentlyInThisPool = null;

//            WasDrowningLastFrame = currentlyDrowning;
//        }
//    }
//}
