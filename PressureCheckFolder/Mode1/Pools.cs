// Pools.cs
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LuneWoL.PressureCheckFolder.Mode1
{
    public class Pools : ModSystem
    {
        public static Pools Instance { get; private set; }

        private readonly List<Pool> _pools = new();
        private readonly Queue<Point> _initialBuildQueue = new();
        private readonly Queue<Point> _incrementalQueue = new();
        private bool _initializing;

        public override bool IsLoadingEnabled(Mod mod)
            => LuneWoL.LWoLServerConfig.WaterRelated.DepthPressureMode == 2;

        public override void OnWorldLoad()
        {
            Instance = this;
            Main.NewText("[Debug] Pools system loaded");

            // Seed initial build queue
            if (DepthPressureConfig.FastInitialBuildUnlimited)
            {
                _initializing = true;
                for (int x = 0; x < Main.tile.Width; x++)
                    for (int y = 0; y < Main.tile.Height; y++)
                    {
                        var t = Main.tile[x, y];
                        if (t != null && t.LiquidAmount == 255 && t.LiquidType == LiquidID.Water)
                            _initialBuildQueue.Enqueue(new Point(x, y));
                    }
                Main.NewText($"[Debug] Initial unlimited build queued: {_initialBuildQueue.Count} points");
            }
            else
            {
                RebuildAllPools(); // fallback small capped build
                MergePools();
            }
        }

        public override void PreUpdateWorld()
        {
            if (_initializing && DepthPressureConfig.FastInitialBuildUnlimited)
            {
                int batch = DepthPressureConfig.InitialBuildBatchPoints;
                while (batch-- > 0 && _initialBuildQueue.Count > 0)
                {
                    var start = _initialBuildQueue.Dequeue();
                    var pts = Floodfill(new HashSet<Point>(), start, DepthPressureConfig.MaxFloodPointsPerBuild);
                    var pool = new Pool(); pool.AddPoints(pts);
                    _pools.Add(pool);
                }
                MergePools();
                Main.NewText($"[Debug] Initial build remaining: {_initialBuildQueue.Count}");

                if (_initialBuildQueue.Count == 0)
                {
                    _initializing = false;
                    Main.NewText("[Debug] Initial unlimited build complete");
                }
            }
        }

        public override void PostUpdateWorld()
        {
            // Seed incremental queue by scan radius
            if (DepthPressureConfig.ScanRadiusTiles > 0)
                ProcessPlayerScan(Main.LocalPlayer.Center);

            // Process a fixed number of tiles each tick
            int processed = 0;
            while (processed < DepthPressureConfig.IncrementalTilesPerTick && _incrementalQueue.Count > 0)
            {
                var pt = _incrementalQueue.Dequeue();
                var pts = Floodfill(new HashSet<Point>(), pt, DepthPressureConfig.MaxFloodPointsPerTile);
                if (pts.Count > 0)
                {
                    var candidate = FindPool(pt.ToWorldCoordinates());
                    if (candidate != null)
                        candidate.AddPoints(pts);
                    else
                    {
                        var newPool = new Pool(); newPool.AddPoints(pts);
                        _pools.Add(newPool);
                    }
                }
                processed++;
            }

            if (processed > 0)
                MergePools();
        }

        public void EnqueueUpdate(Point p)
        {
            if (DepthPressureConfig.UseIncrementalUpdates)
                _incrementalQueue.Enqueue(p);
        }

        private void RebuildAllPools()
        {
            _pools.Clear();
            var visited = new HashSet<Point>();
            int cap = DepthPressureConfig.MaxFloodPointsPerBuild;
            for (int y = 0; y < Main.tile.Height; y++)
                for (int x = 0; x < Main.tile.Width; x++)
                {
                    var pt = new Point(x, y);
                    if (visited.Contains(pt)) continue;
                    var tile = Main.tile[x, y];
                    if (tile.LiquidAmount == 255 && tile.LiquidType == LiquidID.Water)
                    {
                        var pts = Floodfill(visited, pt, cap);
                        var pool = new Pool(); pool.AddPoints(pts);
                        _pools.Add(pool);
                    }
                }
            Main.NewText($"[Debug] Full rebuild: {_pools.Count} pools");
        }

        private void ProcessPlayerScan(Vector2 center)
        {
            int cx = (int)(center.X / 16f), cy = (int)(center.Y / 16f), r = DepthPressureConfig.ScanRadiusTiles;
            int r2 = r * r;
            for (int dy = -r; dy <= r; dy++)
                for (int dx = -r; dx <= r; dx++)
                {
                    if (dx * dx + dy * dy > r2) continue;
                    int x = cx + dx, y = cy + dy;
                    if (x < 0 || y < 0 || x >= Main.tile.Width || y >= Main.tile.Height) continue;

                    var tile = Main.tile[x, y];
                    if (tile.LiquidAmount == 255 && tile.LiquidType == LiquidID.Water)
                    {
                        if (FindPool(new Vector2((x + 0.5f) * 16f, (y + 0.5f) * 16f)) == null)
                        {
                            _incrementalQueue.Enqueue(new Point(x, y));
                            return;
                        }
                    }
                }
        }

        private void MergePools()
        {
            for (int i = 0; i < _pools.Count; i++)
            {
                for (int j = i + 1; j < _pools.Count; j++)
                {
                    var a = _pools[i]; var b = _pools[j];
                    if (a.MinX <= b.MaxX + 1 && a.MaxX >= b.MinX - 1 &&
                        a.MinY <= b.MaxY + 1 && a.MaxY >= b.MinY - 1)
                    {
                        a.Merge(b);
                        _pools.RemoveAt(j);
                        j--;
                    }
                }
            }
        }

        private static List<Point> Floodfill(HashSet<Point> visited, Point start, int max)
        {
            var res = new List<Point>(max);
            var queue = new Queue<Point>(); queue.Enqueue(start);
            while (queue.Count > 0 && res.Count < max)
            {
                var p = queue.Dequeue();
                if (!visited.Add(p)) continue;
                var t = Main.tile[p.X, p.Y];
                if (t.LiquidAmount == 255 && t.LiquidType == LiquidID.Water)
                {
                    res.Add(p);
                    EnqueueNeighbors(queue, p, visited);
                }
            }
            return res;
        }

        private static void EnqueueNeighbors(Queue<Point> q, Point p, HashSet<Point> v)
        {
            int x = p.X, y = p.Y;
            if (x > 0) q.Enqueue(new Point(x - 1, y));
            if (x < Main.tile.Width - 1) q.Enqueue(new Point(x + 1, y));
            if (y > 0) q.Enqueue(new Point(x, y - 1));
            if (y < Main.tile.Height - 1) q.Enqueue(new Point(x, y + 1));
        }

        public Pool FindPool(Vector2 pos)
            => _pools.FirstOrDefault(p => p.IsIn(pos));
    }
}
