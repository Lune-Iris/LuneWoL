using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;

using static LuneLib.Utilities.LuneLibUtils;

namespace LuneWoL.LWoL_IL_Edits
{
    public partial class LWoLILEdits
    {
        public static void LoadIL()
        {
            var npc = LuneWoL.LWoLServerConfig.NPCs;
            var equip = LuneWoL.LWoLServerConfig.Equipment;

            if (npc.SellMult != 1 || npc.BuyMult != 1)
            {
                IL_Player.GetItemExpectedPrice += BuyPriceandSellPrice;
            }

            if (equip.ReforgeNerf)
            {
                IL_Player.GrantPrefixBenefits += NerfAccessoryReforges;
                IL_Main.MouseText_DrawItemTooltip_GetLinesInfo += AccessoryTooltipFix;
                IL_Item.TryGetPrefixStatMultipliersForItem += NerfWeaponReforges;
            }
        }

        #region buy and sell consume capitalism stuff
        private static void BuyPriceandSellPrice(ILContext il)
        {
            var npc = LuneWoL.LWoLServerConfig.NPCs;

            var cursor = new ILCursor(il);

            if (cursor.TryGotoNext(MoveType.Before, i => i.MatchLdarg(0), i => i.MatchLdflda<Player>("currentShoppingSettings")))
            {
                cursor.RemoveRange(4);
                cursor.Emit(OpCodes.Ldc_R4, npc.BuyMult);
                cursor.Emit(OpCodes.Conv_R8);
                cursor.Emit(OpCodes.Mul);
            }
            if (cursor.TryGotoNext(MoveType.Before, i => i.MatchLdarg(0), i => i.MatchLdflda<Player>("currentShoppingSettings")))
            {
                cursor.RemoveRange(4);
                cursor.Emit(OpCodes.Ldc_R4, npc.SellMult);
                cursor.Emit(OpCodes.Conv_R8);
                cursor.Emit(OpCodes.Mul);
            }
            if (cursor.TryGotoNext(MoveType.Before, i => i.MatchLdarg(0), i => i.MatchLdflda<Player>("currentShoppingSettings")))
            {
                cursor.RemoveRange(4);
                cursor.Emit(OpCodes.Ldc_R4, npc.BuyMult);
                cursor.Emit(OpCodes.Conv_R8);
                cursor.Emit(OpCodes.Mul);
            }
            if (cursor.TryGotoNext(MoveType.Before, i => i.MatchLdarg(0), i => i.MatchLdflda<Player>("currentShoppingSettings")))
            {
                cursor.RemoveRange(4);
                cursor.Emit(OpCodes.Ldc_R4, npc.SellMult);
                cursor.Emit(OpCodes.Conv_R8);
                cursor.Emit(OpCodes.Mul);
            }
        }
        #endregion

        #region MAN FUCK YO MF REFORGES
        private static void NerfAccessoryReforges(ILContext iL)
        {
            var c = new ILCursor(iL);


            #region defense
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcI4(2)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_I4, 1);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcI4(3)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_I4, 2);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcI4(4)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_I4, 2);
            }
            #endregion

            #region mana
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcI4(20)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_I4, 10);
            }
            #endregion

            #region crit
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(2)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(4)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 2f);
            }
            #endregion

            #region damage
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.01f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.01f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.02f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.01f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.03f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.02f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.04f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.02f);
            }
            #endregion

            #region move speed
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.01f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.01f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.02f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.01f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.03f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.02f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.04f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.02f);
            }
            #endregion

            #region melee speed
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.01f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.01f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.02f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.01f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.03f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.02f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.04f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.02f);
            }
            #endregion
        }

        private static void AccessoryTooltipFix(ILContext iL)
        {
            var c = new ILCursor(iL);

            #region defense
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdstr("+1")))
            {
                c.Remove();
                c.Emit(OpCodes.Ldstr, "+1");
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdstr("+2")))
            {
                c.Remove();
                c.Emit(OpCodes.Ldstr, "+1");
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdstr("+3")))
            {
                c.Remove();
                c.Emit(OpCodes.Ldstr, "+2");
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdstr("+4")))
            {
                c.Remove();
                c.Emit(OpCodes.Ldstr, "+2");
            }
            #endregion

            #region mana
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdstr("+20 ")))
            {
                c.Remove();
                c.Emit(OpCodes.Ldstr, "+10 ");
            }
            #endregion

            #region crit
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdstr("+2")))
            {
                c.Remove();
                c.Emit(OpCodes.Ldstr, "1");
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdstr("+4")))
            {
                c.Remove();
                c.Emit(OpCodes.Ldstr, "2");
            }
            #endregion

            #region damage
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdstr("+1")))
            {
                c.Remove();
                c.Emit(OpCodes.Ldstr, "+1");
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdstr("+2")))
            {
                c.Remove();
                c.Emit(OpCodes.Ldstr, "+1");
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdstr("+3")))
            {
                c.Remove();
                c.Emit(OpCodes.Ldstr, "+2");
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdstr("+4")))
            {
                c.Remove();
                c.Emit(OpCodes.Ldstr, "+2");
            }
            #endregion

            #region move speed
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdstr("+1")))
            {
                c.Remove();
                c.Emit(OpCodes.Ldstr, "+1");
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdstr("+2")))
            {
                c.Remove();
                c.Emit(OpCodes.Ldstr, "+1");
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdstr("+3")))
            {
                c.Remove();
                c.Emit(OpCodes.Ldstr, "+2");
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdstr("+4")))
            {
                c.Remove();
                c.Emit(OpCodes.Ldstr, "+2");
            }
            #endregion

            #region melee speed
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdstr("+1")))
            {
                c.Remove();
                c.Emit(OpCodes.Ldstr, "+1");
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdstr("+2")))
            {
                c.Remove();
                c.Emit(OpCodes.Ldstr, "+1");
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdstr("+3")))
            {
                c.Remove();
                c.Emit(OpCodes.Ldstr, "+2");
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdstr("+4")))
            {
                c.Remove();
                c.Emit(OpCodes.Ldstr, "+2");
            }
            #endregion
        }

        private static void NerfWeaponReforges(ILContext iL)
        {
            var c = new ILCursor(iL);

            #region Large
            //  Large BBWs
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.12f))) //size
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.07f);
            }
            #endregion

            #region Massive
            //  Massive BBWs
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.18f))) //size
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.13f);
            }
            #endregion

            #region Dangerous
            //im not commenting all of these fuck that
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.05f))) //dmg
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.03f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcI4(2))) //crit
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_I4, 1);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.05f))) //size
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.00f);
            }
            #endregion

            #region Savage
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.05f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.05f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.05f);
            }
            #endregion

            #region Sharp
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.15f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.1f);
            }
            #endregion

            #region Pointy
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.05f);
            }
            #endregion

            #region Legendary 
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.15f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.08f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.15f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.08f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcI4(5)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_I4, 2);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.95f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.05f);
            }
            #endregion

            #region Tiny
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.82f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.77f);
            }
            #endregion

            #region Terrible 
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.85f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.8f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.85f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.8f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.87f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.82f);
            }
            #endregion

            #region Small 
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.85f);
            }
            #endregion

            #region Dull 
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.85f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.8f);
            }
            #endregion

            #region Unhappy
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.15f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.85f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.85f);
            }
            #endregion

            #region Bulky 
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.05f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.05f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.0f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.05f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.15f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.20f);
            }
            #endregion

            #region Shameful 
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.8f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.75f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.85f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.05f);
            }
            #endregion

            #region Heavy 
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.15f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.1f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.15f);
            }
            #endregion

            #region Light 
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.85f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.85f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.9f);
            }
            #endregion

            #region Sighted 
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.05f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcI4(3)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_I4, 1);
            }
            #endregion

            #region Rapid 
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.85f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.9f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.05f);
            }
            #endregion

            #region Hasty 
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.95f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.15f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.1f);
            }
            #endregion

            #region Intimidating 
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.15f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.1f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.05f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.0f);
            }
            #endregion

            #region Deadly 
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.05f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.05f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.05f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.0f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.05f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.95f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.0f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcI4(2)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_I4, 1);
            }
            #endregion

            #region Staunch 
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.15f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.1f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.05f);
            }
            #endregion

            #region Unreal
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.15f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.10f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.15f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.10f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcI4(5)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_I4, 2);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.95f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.05f);
            }
            #endregion

            #region Awful
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.85f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.85f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.85f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.8f);
            }
            #endregion

            #region Lethargic 
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.15f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.2f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.85f);
            }
            #endregion

            #region Awkward
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.15f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.8f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.75f);
            }
            #endregion

            #region Powerful 
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.15f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.15f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.1f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcI4(1)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_I4, 0);
            }
            #endregion

            #region Frenzying
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.85f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.9f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.85f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.8f);
            }
            #endregion

            #region Mystic 
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.85f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.9f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.05f);
            }
            #endregion

            #region Adept
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.85f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.9f);
            }
            #endregion

            #region Masterful 
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.85f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.9f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.15f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.1f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.05f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.0f);
            }
            #endregion

            #region Mythical
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.15f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.1f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.15f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.1f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcI4(5)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_I4, 2);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.95f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.95f);
            }
            #endregion

            #region Inept
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.15f);
            }
            #endregion

            #region Ignorant
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.2f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.25f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.85f);
            }
            #endregion

            #region Deranged 
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.85f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.85f);
            }
            #endregion

            #region Intense 
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.15f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.2f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.05f);
            }
            #endregion

            #region Taboo
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.15f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.05f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.95f);
            }
            #endregion

            #region Celestial
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.95f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.05f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.15f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.05f);
            }
            #endregion

            #region Furious powerfist aha aha aha
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.2f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.25f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.15f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.1f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.15f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.1f);
            }
            #endregion

            #region Manic
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.95f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.85f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.95f);
            }
            #endregion

            #region Legendary2? what is that
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.17f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.12f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.17f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.12f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcI4(8)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_I4, 3);
            }
            #endregion

            #region Keen
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcI4(3)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_I4, 2);
            }
            #endregion

            #region Superior 
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.05f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcI4(3)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_I4, 1);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.05f);
            }
            #endregion

            #region Forceful 
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.15f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.1f);
            }
            #endregion

            #region Hurtful 
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.05f);
            }
            #endregion

            #region Strong
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.15f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.1f);
            }
            #endregion

            #region Unpleasant 
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.15f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.1f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.05f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.0f);
            }
            #endregion

            #region Godly
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.15f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.1f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.15f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.1f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcI4(5)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_I4, 2);
            }
            #endregion

            #region Demonic
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.15f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.1f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcI4(5)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_I4, 2);
            }
            #endregion

            #region Zealous
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcI4(5)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_I4, 3);
            }
            #endregion

            #region Broken
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.7f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.65f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.8f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.75f);
            }
            #endregion

            #region Damaged
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.85f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.8f);
            }
            #endregion

            #region Weak
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.8f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.75f);
            }
            #endregion

            #region Shoddy
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.85f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.8f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.85f);
            }
            #endregion

            #region Ruthless
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.85f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.18f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.13f);
            }
            #endregion

            #region Quick 
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.95f);
            }
            #endregion

            #region Deadly2?????
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.1f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.05f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.95f);
            }
            #endregion

            #region Agile
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.95f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcI4(3)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_I4, 2);
            }
            #endregion

            #region Nimble
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.95f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.97f);
            }
            #endregion

            #region Murderous
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcI4(3)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_I4, 2);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.94f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.97f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.07f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.02f);
            }
            #endregion

            #region Slow
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.15f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.20f);
            }
            #endregion

            #region Sluggish
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.2f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.25f);
            }
            #endregion

            #region Lazy
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.08f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.13f);
            }
            #endregion

            #region Annoying
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.8f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.75f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.15f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.2f);
            }
            #endregion

            #region Nasty
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.85f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(0.9f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 0.95f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcR4(1.05f)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_R4, 1.05f);
            }
            if (c.TryGotoNext(MoveType.Before, i => i.MatchLdcI4(2)))
            {
                c.Remove();
                c.Emit(OpCodes.Ldc_I4, 1);
            }
            #endregion
        }

        #endregion
    }
}
