using System.Drawing;

namespace Starlight.Asus.Aura
{
    internal class AuraApperancePacket : AuraPacket
    {
        public AuraApperancePacket()
            : base(new byte[] { 0x5D, 0xB3 })
        {
        }
        
        public AuraApperancePacket Zone(byte zoneId)
        {
            Data[2] = zoneId;
            
            return this;
        }

        public AuraApperancePacket Animation(AuraAnimation animation)
        {
            Data[3] = (byte)animation;

            return this;
        }
        
        public AuraApperancePacket PrimaryColor(Color color)
        {
            Data[4] = color.R;
            Data[5] = color.G;
            Data[6] = color.B;

            return this;
        }
        
        public AuraApperancePacket Speed(AuraAnimationSpeed speed)
        {
            Data[7] = (byte)speed;
            
            return this;
        }

        public AuraApperancePacket SecondaryColor(Color color)
        {
            Data[10] = color.R;
            Data[11] = color.G;
            Data[12] = color.B;

            return this;
        }
    }
}