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
            LWoL_Plr plr = Main.player[num].GetModPlayer<LWoL_Plr>();
            plr.ReciveDeathPenalty(rd);
            if (Main.netMode == NetmodeID.Server)
            {
                plr.SyncPlayer(-1, whoAmI, false);
            }
        }
    }
}
