using Starlight.Communication;

namespace Starlight.Asus.AuraKeyboard
{
    internal class AuraPacket : FeaturePacket
    {
        public AuraPacket(byte[] command) 
            : base(0x5D, 64, command)
        {
        }
    }
}