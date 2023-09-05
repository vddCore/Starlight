namespace Hideous
{
    internal abstract class HidPacket : Packet<HidPacket>
    {
        protected HidPacket(int initialDataIndex, int packetLength, params byte[] data)
            : base(initialDataIndex, packetLength, data)
        {
        }
    }
}