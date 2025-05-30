//using Microsoft.Xna.Framework;
//using System;

//namespace LuneWoL.PressureCheckFolder.Mode2
//{
//    public class DepthPressureCheck : ModPlayer
//    {
//        public override bool IsLoadingEnabled(Mod mod)
//            => LuneWoL.LWoLServerConfig.WaterRelated.DepthPressureMode == 2;

//        public override void PostUpdate()
//        {
//            bool drowning = Collision.DrownCollision(Player.position, Player.width, Player.height, Player.gravDir);
//            if (!drowning) return;

//            UpdateCurrentPool();
//            var pool = Pools.Instance?.FindPool(Player.Center);
//            if (pool == null)
//                return;

//            // Prioritize updating the player's pool to ensure up-to-date bounds
//            Pools.Instance.ForceUpdatePool(pool);

//            int dynamicSurface = pool.SurfaceY;
//            Main.NewText($"surface {dynamicSurface}");

//            int tileY = (int)(Player.Center.Y / 16f);
//            float depth = tileY - dynamicSurface;
//            int loss = (int)(depth * DepthPressureConfig.BreathLossPerTile);
//            Player.breath = Math.Max(0, Player.breath - loss);
//        }

//        private Pool _currentPool;
//        private readonly List<Pool> _pools = new List<Pool>();
//        private void UpdateCurrentPool()
//        {
//            _currentPool = Pools.Instance.FindPool(Player.Center);
//            Main.NewText(_currentPool != null ? "[Debug] Found existing pool" : "[Debug] No pool found");
//            if (_currentPool != null)
//                Main.NewText($"count {_pools.Count}, SurfaceY {_currentPool.SurfaceY}, MaxY {_currentPool.MaxY}, MaxX {_currentPool.MaxX}, MinY {_currentPool.MinY}, MinX {_currentPool.MinX}");
//        }
//    }

//}