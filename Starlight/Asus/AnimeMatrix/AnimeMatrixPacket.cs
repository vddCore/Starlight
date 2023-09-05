using Hideous;

namespace Starlight.Asus.AnimeMatrix
{
    internal class AnimeMatrixPacket : FeaturePacket
    {
        public AnimeMatrixPacket(byte[] command)
            : base(0x5E, 640, command)
        {
        }
    }
}