using System.Drawing;

namespace Starlight.Asus.Aura
{
    public class AuraMode
    {
        private readonly AuraDevice _aura;

        internal AuraMode(AuraDevice aura)
        {
            _aura = aura;
        }
        
        public void Static(Color color)
            => Static(0, color);

        public void Static(byte zoneId, Color color)
        {
            _aura.Write(
                _aura.Hid<AuraApperancePacket>(0x5D, 0xB3)
                    .Zone(zoneId)
                    .Animation(AuraAnimation.Static)
                    .PrimaryColor(color)
            );
        }
        
        public void Breathe(Color primary, Color secondary, AuraAnimationSpeed speed = AuraAnimationSpeed.Medium)
            => Breathe(0, primary, secondary, speed);

        public void Breathe(
            byte zoneId,
            Color primary,
            Color secondary,
            AuraAnimationSpeed speed = AuraAnimationSpeed.Medium
        ) => Animate(zoneId, AuraAnimation.Breathe, primary, secondary, speed);
        
        public void Pulse(
            byte zoneId,
            Color color,
            AuraAnimationSpeed speed = AuraAnimationSpeed.Medium
        ) => Animate(zoneId, AuraAnimation.Pulse, color, speed);
        
        public void Pulse(Color color, AuraAnimationSpeed speed = AuraAnimationSpeed.Medium)
            => Pulse(0, color, speed);
        
        public void Strobe(
            byte zoneId,
            AuraAnimationSpeed speed = AuraAnimationSpeed.Medium
        ) => Animate(zoneId, AuraAnimation.Strobe, speed);
        
        public void Strobe(AuraAnimationSpeed speed = AuraAnimationSpeed.Medium)
            => Strobe(0, speed);

        private void Animate(
            byte zoneId,
            AuraAnimation animation,
            Color primary,
            Color secondary,
            AuraAnimationSpeed speed
        )
        {
            _aura.Write(
                _aura.Hid<AuraApperancePacket>(0x5D, 0xB3)
                    .Zone(zoneId)
                    .Animation(animation)
                    .Speed(speed)
                    .PrimaryColor(primary)
                    .SecondaryColor(secondary)
            );
            
            _aura.Present();
        }
        
        private void Animate(byte zoneId, AuraAnimation animation, Color primary, AuraAnimationSpeed speed)
        {
            _aura.Write(
                _aura.Hid<AuraApperancePacket>(0x5D, 0xB3)
                    .Zone(zoneId)
                    .Animation(animation)
                    .Speed(speed)
                    .PrimaryColor(primary)
            );
            
            _aura.Present();
        }
        
        private void Animate(byte zoneId,
            AuraAnimation animation,
            AuraAnimationSpeed speed
        )
        {
            _aura.Write(
                _aura.Hid<AuraApperancePacket>(0x5D, 0xB3)
                    .Zone(zoneId)
                    .Animation(animation)
                    .Speed(speed)
            );
            
            _aura.Present();
        }
    }
}