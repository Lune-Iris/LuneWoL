namespace LuneWoL.Common.WoL_Plrs;

public partial class LWoL_Plr : ModPlayer
{
    public static int spacedout = 50;
    public override void UpdateBadLifeRegen()
    {
        float totalNegativeLifeRegen = 0;

        void ApplyDoTDebuff(bool hasDebuff, int negativeLifeRegenToApply, bool immuneCondition = false)
        {
            if (!hasDebuff || immuneCondition)
                return;

            if (Player.lifeRegen > 0)
                Player.lifeRegen = 0;

            Player.lifeRegenTime = 0;
            totalNegativeLifeRegen += negativeLifeRegenToApply;
        }

        spacedout = 50 -
            (WearingAstroHelm ? 10 : 0) -
            (WearingAstraliteVisor ? 15 : 0) -
            (IsWearingFishBowl ? 10 : 0);

        ApplyDoTDebuff(Player.LibPlayer().BoilFreeze, spacedout, WearingFullAstralite || WearingFullAstro);

        ApplyDoTDebuff(Player.LibPlayer().depthwaterPressure, Player.LibPlayer().currentDepthPressure, MPLL);

        ApplyDoTDebuff(Player.LibPlayer().BlizzardFrozen, 50, Player.buffImmune[BuffID.Frozen]);

        ApplyDoTDebuff(Player.LibPlayer().Chilly, 2, Player.buffImmune[BuffID.Chilled]);

        ApplyDoTDebuff(Player.LibPlayer().CrimtuptionzoneNight, 100, false);

        ApplyDoTDebuff(WearingFullLead && Player.LibPlayer().LeadPoison, 8, Player.buffImmune[BuffID.Poisoned]);

        ApplyDoTDebuff(WearingTwoLeadPieces && Player.LibPlayer().LeadPoison, 4, Player.buffImmune[BuffID.Poisoned]);

        ApplyDoTDebuff(WearingOneLeadPiece && Player.LibPlayer().LeadPoison, 2, Player.buffImmune[BuffID.Poisoned]);

        Player.lifeRegen -= (int)totalNegativeLifeRegen;
    }
}
