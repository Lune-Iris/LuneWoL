namespace LuneWoL.Core.Config;

[BackgroundColor(5, 40, 40, 255)]
public class LWoLAdvancedClientSettings : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;

    [SeparatePage]
    public class ClientDepthPressureDented
    {
        [BackgroundColor(95, 155, 160, 255)]
        [Range(0, 500)]
        public int UpdateIntervalTicks { get; set; }

        [BackgroundColor(95, 155, 160, 255)]
        public bool ShowSurfaceDebug { get; set; }

        public ClientDepthPressureDented()
        {
            UpdateIntervalTicks = 30;
            ShowSurfaceDebug = false;
        }
    }

    [BackgroundColor(15, 60, 65, 255)]
    public ClientDepthPressureDented ClientDepthPressure = new();

    public override void OnLoaded() => LuneWoL.LWoLAdvancedClientSettings = this;
}