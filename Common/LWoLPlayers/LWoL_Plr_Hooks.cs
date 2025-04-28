using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace LuneWoL.Common.LWoLPlayers
{
    public partial class LWoLPlayer : ModPlayer
    {

        #region Hooks

        public override void OnEnterWorld()
        {
            EnterWorldMessage();
        }

        public override void OnRespawn()
        {
            ActualBrainrotCodeOnRespawn();
            okback2();
            THEBLACKONE();
        }

        public override void PostUpdateEquips()
        {
            HellIsQuiteHot();

            ArmourReworked();

            HeatExhaustionUpdEquips();
        }

        public override void PostUpdateRunSpeeds()
        {
            SlowWater();

            HeatExhaustionUpdRunSpeed();
        }

        public override void PreUpdateBuffs()
        {
            prebuffBurnFreeze();

            WaterPoison();

            WeatherPain();

            ColdMakeColdBrrrrr();

            Thisissoevillmfao();

            DarkWaters();
        }

        public override void PostUpdateMiscEffects()
        {
            HeatExhaustion();
        }

        public override void PostUpdate()
        {
            var Config = LuneWoL.LWoLServerConfig.Main;

            if (Player.whoAmI == Main.myPlayer && LuneLib.LuneLib.clientConfig.DebugMessages)
            {
                Main.NewText($"Heat = {HeatStrokeCounter}, Bliz = {tundraBlizzardCounter}");
            }

            if (DmgPlrBcCrit && Config.CritFailMode > 0)
            {
                CritFailDamage(Player);
            }

            uhghhhghhg();

            // https://steamcommunity.com/sharedfiles/filedetails/?id=2395507804
        }

        public override void OnHitNPC(NPC npc, NPC.HitInfo hit, int damageDone)
        {
            var Config = LuneWoL.LWoLServerConfig.Main;

            if (!IsCritFail && Config.CritFailMode != 0)
            {
                CritFail(Player, npc);
            }
        }

        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            ActualBrainrotCodeFuckThisImTired(out health, out mana);
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            syncthething(toWho, fromWho, newPlayer);
        }

        public override void CopyClientState(ModPlayer targetCopy)
        {
            copytheclientthing(targetCopy);
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            sendtheclientthing(clientPlayer);
        }

        public override void SaveData(TagCompound tag)
        {
            ActualBrainrotCodeTHISAGUST12TH2025(tag);
        }

        public override void LoadData(TagCompound tag)
        {
            ActualBrainrotCodebutcooler(tag);
        }

        #endregion

    }
}