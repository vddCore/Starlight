using Starlight.Communication;

namespace Starlight.Asus.AnimeMatrix
{
    internal class AnimeMatrixPacket : FeaturePacket
    {
        protected AnimeMatrixPacket(byte[] command)
            : base(0x5E, 640, command)
        {
        }
    }
}