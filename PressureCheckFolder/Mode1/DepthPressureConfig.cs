// DepthPressureConfig.cs
namespace LuneWoL.PressureCheckFolder.Mode1
{
    public static class DepthPressureConfig
    {
        // === Initial Build (chunked) ===
        public static bool FastInitialBuildUnlimited { get; set; } = false;
        public static int InitialBuildBatchPoints { get; set; } = 10000;  // tiles per tick

        // === Incremental Updates ===
        public static bool UseIncrementalUpdates { get; set; } = true;
        public static int IncrementalTilesPerTick { get; set; } = 500;
        public static int ScanRadiusTiles { get; set; } = 50;

        // === Flood Limits ===
        public static int MaxFloodPointsPerTile { get; set; } = 1;     // floodfill depth per point
        public static int MaxFloodPointsPerBuild { get; set; } = 10000; // cap per flood
    }
}