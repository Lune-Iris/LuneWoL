namespace LuneWoL;

public partial class LuneWoL
{
    internal enum MessageType : byte
    {
        dedsec
    }
    public override void HandlePacket(BinaryReader rd, int whoAmI)
    {
        MessageType msgtype = (MessageType)rd.ReadByte();

        if (msgtype == MessageType.dedsec)
        {
            byte num = rd.ReadByte();
            LWoLPlayer plr = Main.player[num].GetModPlayer<LWoLPlayer>();
            plr.ReciveDeathPenalty(rd);
            if (Main.netMode == NetmodeID.Server)
            {
                plr.SyncPlayer(-1, whoAmI, false);
            }
        }
    }
}
