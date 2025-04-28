using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace LuneWoL.Common.LWoLPlayers
{
    public partial class LWoLPlayer : ModPlayer
    {
        public async void EnterWorldMessage()
        {
            if (LuneWoL.LWoLClientConfig.STFUCHAT) return;

            await Task.Delay(5000);

            if (Player.whoAmI == Main.myPlayer)
            {
                Main.NewText($"{((!LuneLib.LuneLib.instance.ChatSourceLoaded) ? "[LuneWoL] "  : "")}Dont forget to join the Discord!... Please? I need suggestions for the mod...\nYou can turn this message off in the client config.", 70, 80, 150);
            }
        }
    }
}
