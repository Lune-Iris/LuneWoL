using LuneWoL.Common.LWoLPlayers;
using System.IO;
using Terraria;
using Terraria.ID;

namespace LuneWoL
{
    public partial class LuneWoL
    {
        internal enum MessageType : byte
        {
            dedsec
        }
        public override void HandlePacket(BinaryReader librarian, int whoAmI)
        {
            MessageType the = (MessageType)librarian.ReadByte();

            if (the == MessageType.dedsec)
            {
                byte thenumber = librarian.ReadByte();
                LWoLPlayer lWoLPlayer = Main.player[thenumber].GetModPlayer<LWoLPlayer>();
                lWoLPlayer.MistaWhiteImInFortnite(librarian);
                if (Main.netMode == NetmodeID.Server)
                {
                    lWoLPlayer.SyncPlayer(-1, whoAmI, false);
                }
            }
        }
    }
}
