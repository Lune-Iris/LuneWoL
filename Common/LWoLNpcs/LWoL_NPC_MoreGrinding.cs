//using LuneWoL.Content.Items;
//using System;
//using Terraria;
//using Terraria.GameContent.ItemDropRules;
//using Terraria.ID;
//using Terraria.ModLoader;

//namespace LuneWoL.Common.Npcs
//{
//    public partial class WoLNpc : GlobalNPC
//    {
//        public void MoreGrinding(NPC npc, NPCLoot npcLoot)
//        {
//            foreach (var rule in npcLoot.Get())
//            {
//                if (rule is CommonDrop commonDrop)
//                {
//                    if (commonDrop.chanceDenominator > 1)
//                    {
//                        commonDrop.chanceDenominator = 1;
//                    }
//                    else if (commonDrop.chanceDenominator < 1)
//                    {
//                        commonDrop.chanceDenominator = 1;
//                    }
//                }
//                if (rule is LeadingConditionRule drop)
//                {
//                    foreach (var chainedRule in drop.ChainedRules)
//                    {
//                        if (chainedRule.RuleToChain is CommonDrop cD)
//                        {
//                            if (cD.chanceDenominator > 1)
//                            {
//                                cD.chanceDenominator = 1;
//                            }
//                        }
//                    }
//                }
//            }
//        }
//    }
//}
