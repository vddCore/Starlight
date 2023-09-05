namespace Hideous
{
    public abstract class FeaturePacket : Packet<FeaturePacket>
    {
        public FeaturePacket(byte reportId, int packetLength, params byte[] data)
            : base(1, packetLength, data)
        {
            Data[0] = reportId;
        }
    }
}