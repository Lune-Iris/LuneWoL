namespace LuneWoL.Core.Config;

[BackgroundColor(10, 75, 105, 255)]
public class LWoLAdvancedClientSettings : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;

    [SeparatePage]
    public class ClientDepthPressureDented
    {
        [BackgroundColor(35, 115, 145, 255)]
        [Range(0, 500)]
        public int UpdateIntervalTicks { get; set; }

        [BackgroundColor(35, 115, 145, 255)]
        public bool ShowSurfaceDebug { get; set; }

        public ClientDepthPressureDented()
        {
            UpdateIntervalTicks = 30;
            ShowSurfaceDebug = false;
        }
    }

    [BackgroundColor(10, 75, 105, 200)]
    public ClientDepthPressureDented ClientDepthPressure = new();

    public override void OnLoaded() => LuneWoL.LWoLAdvancedClientSettings = this;
}