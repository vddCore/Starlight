using Starlight.Communication;

namespace Starlight.Asus.Aura
{
    internal class AuraPacket : HidPacket
    {
        public AuraPacket(byte[] command) 
            : base(0, 32, command)
        {
        }
    }
}