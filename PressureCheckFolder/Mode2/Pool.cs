//using Microsoft.Xna.Framework;
//using System.Collections.Generic;
//using System.Linq;

//namespace LuneWoL.PressureCheckFolder.Mode2
//{
//    public class Pool
//    {
//        public int SurfaceY { get; private set; }
//        public int MinX { get; private set; }
//        public int MaxX { get; private set; }
//        public int MinY { get; private set; }
//        public int MaxY { get; private set; }
//        private readonly Dictionary<int, (int leftX, int rightX)> _bounds = new();

//        public void AddPoints(IEnumerable<Point> points)
//        {
//            _bounds.Clear();
//            MinY = int.MaxValue; MaxY = int.MinValue;
//            MinX = int.MaxValue; MaxX = int.MinValue;

//            foreach (var group in points.GroupBy(pt => pt.Y))
//            {
//                int y = group.Key;
//                int left = group.Min(pt => pt.X);
//                int right = group.Max(pt => pt.X);
//                _bounds[y] = (left, right);
//                MinY = Math.Min(MinY, y);
//                MaxY = Math.Max(MaxY, y);
//                MinX = Math.Min(MinX, left);
//                MaxX = Math.Max(MaxX, right);
//            }

//            SurfaceY = MinY;
//        }

//        public bool IsIn(Vector2 worldPos)
//        {
//            int ty = (int)(worldPos.Y / 16f);
//            if (ty < MinY || ty > MaxY) return false;
//            if (!_bounds.TryGetValue(ty, out var bounds)) return false;
//            int tx = (int)(worldPos.X / 16f);
//            return tx >= bounds.leftX && tx <= bounds.rightX;
//        }
//        public Point GetCentralPoint()
//        {
//            int midX = (MinX + MaxX) / 2;
//            int midY = (MinY + MaxY) / 2;
//            return new Point(midX, midY);
//        }

//        public void Merge(Pool other)
//        {
//            foreach (var kv in other._bounds)
//            {
//                if (_bounds.TryGetValue(kv.Key, out var b))
//                    _bounds[kv.Key] = (System.Math.Min(b.leftX, kv.Value.leftX), System.Math.Max(b.rightX, kv.Value.rightX));
//                else
//                    _bounds[kv.Key] = kv.Value;
//            }

//            MinY = System.Math.Min(MinY, other.MinY);
//            MaxY = System.Math.Max(MaxY, other.MaxY);
//            MinX = System.Math.Min(MinX, other.MinX);
//            MaxX = System.Math.Max(MaxX, other.MaxX);
//            SurfaceY = MinY;
//        }
//    }
//}