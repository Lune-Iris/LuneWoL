using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static LuneLib.Utilities.LuneLibUtils;

namespace LuneWoL.PressureCheckFolder.Mode2
{
    public partial class PressureModeTwo : ModPlayer
    {
        public bool WasDrowningLastFrame { get; set; }

        public Vector2 EntryPoint { get; set; }
        public Vector2 ExitPoint { get; set; }

        public bool InWaterBody { get; set; }
        public bool IsDrowning { get; set; }

        private const float ReEntryRadius = 240f;

        public void CheckWaterDepth()
        {
            bool currentlyDrowning = Collision.DrownCollision(LP.position, LP.width, LP.height, LP.gravDir);

            if (currentlyDrowning && !WasDrowningLastFrame && !InWaterBody)
            {
                InWaterBody = true;
                EntryPoint = LP.position;
            }
            else if (!currentlyDrowning && WasDrowningLastFrame)
            {
                ExitPoint = LP.position;

                if (ExitPoint.Y < EntryPoint.Y)
                {
                    EntryPoint = ExitPoint;
                }

                InWaterBody = true;
            }

            if (!currentlyDrowning && InWaterBody && Vector2.Distance(LP.position, ExitPoint) <= ReEntryRadius)
            {
                InWaterBody = true;
            }

            if (!currentlyDrowning && InWaterBody && Vector2.Distance(LP.position, ExitPoint) >= ReEntryRadius)
            {
                InWaterBody = false;
            }

            if (!InWaterBody && Vector2.Distance(LP.position, ExitPoint) > ReEntryRadius)
            {
                EntryPoint = LP.position;
                ExitPoint = LP.position;
            }

            WasDrowningLastFrame = currentlyDrowning;
        }
    }
}
