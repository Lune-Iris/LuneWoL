using System.Reflection;
using MonoMod.Cil;
using Terraria.ModLoader;

namespace LuneWoL.CalamityModChanges.Common.Npcs;

public class CalamityModPatch : ILoadable
{
    private Mod CalamityMod => ModLoader.TryGetMod("CalamityMod", out var clam) ? clam : null;

    public bool IsLoadingEnabled(Mod mod)
    {
        return CalamityMod != null && LuneWoL.LWoLServerConfig.CalamityMod.DifficultyRebuff;
    }

    public void Load(Mod mod)
    {
        MonoModHooks.Modify(CalamityMod.Code.GetType("CalamityMod.NPCs.CalamityGlobalNPC").GetMethod("AdjustMasterModeStatScaling", BindingFlags.Public | BindingFlags.Static), Callback);
    }

    public void Unload()
    {
    }

    private void Callback(ILContext IL)
    {
        ILCursor c = new(IL);
        c.TryGotoNext(MoveType.Before, (i) => i.MatchLdcR8(0.75));
        c.RemoveRange(1);
        c.EmitLdcR4(1);
        c.TryGotoNext(MoveType.Before, (i) => i.MatchLdcR8(0.9));
        c.RemoveRange(1);
        c.EmitLdcR4(1);
    }
}
