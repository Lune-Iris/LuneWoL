using Terraria.ID;
using Terraria.ModLoader;
using static LuneLib.Common.Players.LuneLibPlayer.LibPlayer;
using static LuneLib.Utilities.LuneLibUtils;

namespace LuneWoL.Common.LWoLPlayers
{
    public partial class LWoLPlayer : ModPlayer
    {
        public static int spacedout = 50;
        public override void UpdateBadLifeRegen()
        { // more calamity theft
            float totalNegativeLifeRegen = 0;

            void ApplyDoTDebuff(bool hasDebuff, int negativeLifeRegenToApply, bool immuneCondition = false)
            {
                if (!hasDebuff || immuneCondition)
                    return;

                if (L.lifeRegen > 0)
                    L.lifeRegen = 0;

                L.lifeRegenTime = 0;
                totalNegativeLifeRegen += negativeLifeRegenToApply;
            }

            spacedout = 50 -
                (WearingAstroHelm ? 10 : 0) -
                (WearingAstraliteVisor ? 15 : 0) -
                (IsWearingFishBowl ? 10 : 0);

            ApplyDoTDebuff(L.LibPlayer().BoilFreeze, spacedout, WearingFullAstralite || WearingFullAstro);

            ApplyDoTDebuff(L.LibPlayer().depthwaterPressure, L.LibPlayer().currentDepthPressure, LL);

            ApplyDoTDebuff(L.LibPlayer().BlizzardFrozen, 50, L.buffImmune[BuffID.Frozen]);

            ApplyDoTDebuff(L.LibPlayer().Chilly, 2, L.buffImmune[BuffID.Chilled]);

            ApplyDoTDebuff(L.LibPlayer().CrimtuptionzoneNight, 100, false);

            ApplyDoTDebuff(WearingFullLead && L.LibPlayer().LeadPoison, 8, L.buffImmune[BuffID.Poisoned]);

            ApplyDoTDebuff(WearingTwoLeadPieces && L.LibPlayer().LeadPoison, 4, L.buffImmune[BuffID.Poisoned]);

            ApplyDoTDebuff(WearingOneLeadPiece && L.LibPlayer().LeadPoison, 2, L.buffImmune[BuffID.Poisoned]);

            L.lifeRegen -= (int)totalNegativeLifeRegen;
        }
    }
}
