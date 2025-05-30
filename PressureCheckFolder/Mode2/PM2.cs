namespace LuneWoL.PressureCheckFolder.Mode2
{
    public static class DepthPressureConfig
    {
        public static int InPoolRebuildTicks { get; set; } = 60 * 5;
        public static int NearPoolRebuildTicks { get; set; } = 60 * 15;
        public static int MaxFloodPointsPerTick { get; set; } = 10000;
        public static bool UseIncrementalUpdates { get; set; } = true;
        public static int ScanRadiusTiles { get; set; } = 50;
    }

    public class Pool
    {
        public int SurfaceY { get; private set; }
        private Dictionary<int, (int Left, int Right)> _bounds = new Dictionary<int, (int, int)>();

        public void AddPoints(IEnumerable<Point> points)
        {
            _bounds.Clear();
            foreach (var grp in points.GroupBy(p => p.Y))
                _bounds[grp.Key] = (grp.Min(p => p.X), grp.Max(p => p.X));
            Recalculate();
            Main.NewText($"[Debug] Pool rebuilt: Surface={SurfaceY}, Y[{_bounds.Keys.Min()}..{_bounds.Keys.Max()}]");
        }

        private void Recalculate()
        {
            SurfaceY = _bounds.Keys.Min();
            // _bounds holds rows; SurfaceY = highest water row
        }

        public bool IsIn(Vector2 pos)
        {
            int y = (int)(pos.Y / 16f);
            if (!_bounds.TryGetValue(y, out var r)) return false;
            int x = (int)(pos.X / 16f);
            return x >= r.Left && x <= r.Right;
        }

        public void Merge(Pool other)
        {
            foreach (var kv in other._bounds)
            {
                if (_bounds.TryGetValue(kv.Key, out var r))
                    _bounds[kv.Key] = (Math.Min(r.Left, kv.Value.Left), Math.Max(r.Right, kv.Value.Right));
                else
                    _bounds[kv.Key] = kv.Value;
            }
            Recalculate();
            Main.NewText($"[Debug] Pools merged, new Surface={SurfaceY}");
        }
    }

    public class DepthPressureCheck : ModPlayer
    {
        private int _inPoolTimer, _nearPoolTimer;

        public override bool IsLoadingEnabled(Mod mod) => LuneWoL.LWoLServerConfig.WaterRelated.DepthPressureMode == 2;

        public override void PostUpdate()
        {
            var center = Player.Center;
            bool drowning = Collision.DrownCollision(Player.position, Player.width, Player.height, Player.gravDir);
            var pool = Pools.Instance.FindPool(center);
            bool inPool = pool != null;
            bool nearWater = Pools.Instance.HasNearbyWater(center);

            // Incremental updates only
            if (DepthPressureConfig.UseIncrementalUpdates)
                Pools.Instance.ProcessQueue();

            // Targeted rebuild
            if (inPool)
            {
                if (++_inPoolTimer >= DepthPressureConfig.InPoolRebuildTicks)
                {
                    Pools.Instance.RebuildPlayerPool(center);
                    _inPoolTimer = 0;
                    Main.NewText("[Debug] Rebuilt player's pool");
                }
            }
            else if (nearWater)
            {
                if (++_nearPoolTimer >= DepthPressureConfig.NearPoolRebuildTicks)
                {
                    Pools.Instance.ProcessPlayerScan(center);
                    Pools.Instance.MergePools();
                    _nearPoolTimer = 0;
                    Main.NewText("[Debug] Scanned nearby water");
                }
            }

            if (!drowning) return;

            int ty = (int)(center.Y / 16f);
            int surface = pool?.SurfaceY ?? Pools.Instance.FindWaterSurface((int)(center.X / 16f), ty);
            Main.NewText($"[Debug] SurfaceY={surface}");
            float depth = ty - surface;
            Player.breath = Math.Max(0, Player.breath - (int)(depth / 1000f));
        }
    }

    public class Pools : ModSystem
    {
        public static Pools Instance { get; private set; }
        private List<Pool> _pools = new List<Pool>();
        private Queue<Point> _queue = new Queue<Point>();

        public override bool IsLoadingEnabled(Mod mod) => LuneWoL.LWoLServerConfig.WaterRelated.DepthPressureMode == 2;
        public override void OnWorldLoad() => Instance = this;

        public override void PostUpdateWorld()
        {
            if (DepthPressureConfig.UseIncrementalUpdates)
                ProcessQueue();
            ProcessPlayerScan(Main.LocalPlayer.Center);
            MergePools();
        }

        public void EnqueueUpdate(Point p) => _queue.Enqueue(p);

        public void ProcessQueue()
        {
            int limit = DepthPressureConfig.MaxFloodPointsPerTick;
            for (int i = 0; i < limit && _queue.Count > 0; i++)
            {
                var pt = _queue.Dequeue();
                var pos = pt.ToWorldCoordinates();
                var existing = FindPool(pos);
                var pts = Floodfill(new HashSet<Point>(), pt, limit);
                if (existing != null)
                    existing.AddPoints(pts);
                else
                {
                    var np = new Pool(); np.AddPoints(pts); _pools.Add(np);
                }
            }
        }

        public void RebuildPlayerPool(Vector2 pos)
        {
            var pt = new Point((int)(pos.X / 16f), (int)(pos.Y / 16f));
            var pts = Floodfill(new HashSet<Point>(), pt, DepthPressureConfig.MaxFloodPointsPerTick);
            var pool = FindPool(pos);
            if (pool != null) pool.AddPoints(pts);
        }

        public void ProcessPlayerScan(Vector2 center)
        {
            int cx = (int)(center.X / 16f), cy = (int)(center.Y / 16f), r = DepthPressureConfig.ScanRadiusTiles;
            int r2 = r * r;
            for (int dy = -r; dy <= r; dy++) for (int dx = -r; dx <= r; dx++)
                {
                    if (dx * dx + dy * dy > r2) continue;
                    int x = cx + dx, y = cy + dy;
                    if (x < 0 || y < 0 || x >= Main.tile.Width || y >= Main.tile.Height) continue;
                    var t = Main.tile[x, y];
                    if (t.LiquidAmount == 255 && t.LiquidType == LiquidID.Water && FindPool(new Vector2(x * 16, y * 16)) == null)
                    {
                        var pts = Floodfill(new HashSet<Point>(), new Point(x, y), DepthPressureConfig.MaxFloodPointsPerTick);
                        var np = new Pool(); np.AddPoints(pts); _pools.Add(np);
                        return;
                    }
                }
        }

        public void MergePools()
        {
            for (int i = 0; i < _pools.Count; i++)
                for (int j = i + 1; j < _pools.Count; j++)
                {
                    var a = _pools[i];
                    var b = _pools[j];
                    // adjacent or overlapping
                    if (a.SurfaceY <= b.SurfaceY + 1 && b.SurfaceY <= a.SurfaceY + 1)
                    {
                        a.Merge(b);
                        _pools.RemoveAt(j--);
                    }
                }
        }

        public Pool FindPool(Vector2 pos) => _pools.FirstOrDefault(p => p.IsIn(pos));

        public bool HasNearbyWater(Vector2 center)
        {
            int cx = (int)(center.X / 16f), cy = (int)(center.Y / 16f), r = DepthPressureConfig.ScanRadiusTiles;
            int r2 = r * r;
            for (int dy = -r; dy <= r; dy++) for (int dx = -r; dx <= r; dx++)
                    if (dx * dx + dy * dy <= r2)
                    {
                        int x = cx + dx, y = cy + dy;
                        var t = Main.tile[x, y];
                        if (t.LiquidAmount == 255 && t.LiquidType == LiquidID.Water) return true;
                    }
            return false;
        }

        public int FindWaterSurface(int x, int startY) { while (startY > 0) { var t = Main.tile[x, startY]; if (t.LiquidAmount < 255 || t.LiquidType != LiquidID.Water) return startY + 1; startY--; } return 0; }

        private static List<Point> Floodfill(HashSet<Point> vis, Point start, int max)
        {
            var outp = new List<Point>(max);
            var q = new Queue<Point>(); q.Enqueue(start);
            while (q.Count > 0 && outp.Count < max)
            {
                var p = q.Dequeue(); if (!vis.Add(p)) continue;
                var t = Main.tile[p.X, p.Y];
                if (t.LiquidAmount == 255 && t.LiquidType == LiquidID.Water)
                {
                    outp.Add(p);
                    EnqueueNeighbors(q, p, vis);
                }
            }
            return outp;
        }

        private static void EnqueueNeighbors(Queue<Point> q, Point p, HashSet<Point> vis)
        {
            int x = p.X, y = p.Y;
            if (x > 0) q.Enqueue(new Point(x - 1, y));
            if (x < Main.tile.Width - 1) q.Enqueue(new Point(x + 1, y));
            if (y > 0) q.Enqueue(new Point(x, y - 1));
            if (y < Main.tile.Height - 1) q.Enqueue(new Point(x, y + 1));
        }
    }
}
