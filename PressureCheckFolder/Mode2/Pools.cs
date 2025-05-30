//using Microsoft.Xna.Framework;
//using System.Collections.Generic;
//using System.Linq;

//namespace LuneWoL.PressureCheckFolder.Mode2
//{
//    public class Pools : ModSystem
//    {
//        public static Pools Instance { get; private set; }
//        private readonly List<Pool> _pools = new();
//        private readonly Queue<Point> _incrementalQueue = new();

//        public override bool IsLoadingEnabled(Mod mod)
//            => LuneWoL.LWoLServerConfig.WaterRelated.DepthPressureMode == 2;
//        public void ForceUpdatePool(Pool pool)
//        {
//            var centerPoint = pool.GetCentralPoint();
//            var updatedPoints = Floodfill(new HashSet<Point>(), centerPoint, DepthPressureConfig.PriorityUpdateFloodCap);
//            if (updatedPoints.Count > 0)
//                pool.AddPoints(updatedPoints);
//        }

//        public override void OnWorldLoad()
//        {
//            Instance = this;
//            RebuildAllPools();
//            MergePools();
//        }

//        public override void PostUpdateWorld()
//        {
//            if (DepthPressureConfig.ScanRadiusTiles > 0)
//                ProcessPlayerScan(Main.LocalPlayer.Center);

//            int processed = 0;
//            while (processed < DepthPressureConfig.IncrementalTilesPerTick && _incrementalQueue.Count > 0)
//            {
//                Point pt = _incrementalQueue.Dequeue();
//                var pts = Floodfill(new HashSet<Point>(), pt, DepthPressureConfig.MaxFloodPointsPerTile);
//                if (pts.Count > 0)
//                {
//                    Pool pool = FindPool(pt.ToWorldCoordinates());
//                    if (pool != null)
//                        pool.AddPoints(pts);
//                    else
//                    {
//                        var newPool = new Pool();
//                        newPool.AddPoints(pts);
//                        _pools.Add(newPool);
//                    }
//                }
//                processed++;
//            }

//            if (processed > 0)
//                MergePools();
//        }

//        public void EnqueueUpdate(Point p)
//        {
//            if (DepthPressureConfig.UseIncrementalUpdates)
//                _incrementalQueue.Enqueue(p);
//        }

//        private void RebuildAllPools()
//        {
//            _pools.Clear();
//            var visited = new HashSet<Point>();
//            int cap = DepthPressureConfig.MaxFloodPointsPerBuild;

//            for (int y = 0; y < Main.tile.Height; y++)
//                for (int x = 0; x < Main.tile.Width; x++)
//                {
//                    var pt = new Point(x, y);
//                    if (visited.Contains(pt)) continue;
//                    var tile = Main.tile[x, y];
//                    if (tile.LiquidAmount == 255 && tile.LiquidType == LiquidID.Water)
//                    {
//                        var pts = Floodfill(visited, pt, cap);
//                        var pool = new Pool();
//                        pool.AddPoints(pts);
//                        _pools.Add(pool);
//                    }
//                }
//        }

//        private void ProcessPlayerScan(Vector2 center)
//        {
//            int cx = (int)(center.X / 16f);
//            int cy = (int)(center.Y / 16f);
//            int r = DepthPressureConfig.ScanRadiusTiles;
//            int r2 = r * r;

//            for (int dy = -r; dy <= r; dy++)
//                for (int dx = -r; dx <= r; dx++)
//                {
//                    if (dx * dx + dy * dy > r2) continue;
//                    int x = cx + dx;
//                    int y = cy + dy;
//                    if (x < 0 || y < 0 || x >= Main.tile.Width || y >= Main.tile.Height) continue;

//                    var tile = Main.tile[x, y];
//                    if (tile.LiquidAmount == 255 && tile.LiquidType == LiquidID.Water)
//                    {
//                        Vector2 worldPos = new Vector2((x + 0.5f) * 16f, (y + 0.5f) * 16f);
//                        if (FindPool(worldPos) == null)
//                        {
//                            _incrementalQueue.Enqueue(new Point(x, y));
//                            return;
//                        }
//                    }
//                }
//            }

//        private void MergePools()
//        {
//            for (int i = 0; i < _pools.Count; i++)
//            {
//                for (int j = i + 1; j < _pools.Count; j++)
//                {
//                    var a = _pools[i];
//                    var b = _pools[j];
//                    if (a.MinX <= b.MaxX + 1 && a.MaxX >= b.MinX - 1 &&
//                        a.MinY <= b.MaxY + 1 && a.MaxY >= b.MinY - 1)
//                    {
//                        a.Merge(b);
//                        _pools.RemoveAt(j);
//                        j--;
//                    }
//                }
//            }
//        }

//        private static List<Point> Floodfill(HashSet<Point> visited, Point start, int max)
//        {
//            var res = new List<Point>(max);
//            var queue = new Queue<Point>();
//            queue.Enqueue(start);

//            while (queue.Count > 0 && res.Count < max)
//            {
//                var p = queue.Dequeue();
//                if (!visited.Add(p)) continue;
//                var t = Main.tile[p.X, p.Y];
//                if (t.LiquidAmount == 255 && t.LiquidType == LiquidID.Water)
//                {
//                    res.Add(p);
//                    EnqueueNeighbors(queue, p);
//                }
//            }

//            return res;
//        }

//        private static void EnqueueNeighbors(Queue<Point> q, Point p)
//        {
//            int x = p.X;
//            int y = p.Y;
//            if (x > 0) q.Enqueue(new Point(x - 1, y));
//            if (x < Main.tile.Width - 1) q.Enqueue(new Point(x + 1, y));
//            if (y > 0) q.Enqueue(new Point(x, y - 1));
//            if (y < Main.tile.Height - 1) q.Enqueue(new Point(x, y + 1));
//        }

//        public Pool FindPool(Vector2 pos)
//            => _pools.FirstOrDefault(p => p.IsIn(pos));
//    }
//}
