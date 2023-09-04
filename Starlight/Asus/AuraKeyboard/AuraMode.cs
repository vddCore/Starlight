using System.Drawing;

namespace Starlight.Asus.AuraKeyboard
{
    public class AuraMode
    {
        private readonly AuraKeyboardDevice _auraKeyboard;

        internal AuraMode(AuraKeyboardDevice auraKeyboard)
        {
            _auraKeyboard = auraKeyboard;
        }
        
        public void Static(Color color)
            => Static(0, color);

        public void Static(byte zoneId, Color color)
        {
            _auraKeyboard.Set(
                _auraKeyboard.Feature<AuraApperancePacket>()
                    .Zone(zoneId)
                    .Animation(AuraAnimation.Static)
                    .PrimaryColor(color)
                    .SecondaryColor(color)
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

        public void Animate(
            byte zoneId,
            AuraAnimation animation,
            Color primary,
            Color secondary,
            AuraAnimationSpeed speed
        )
        {
            _auraKeyboard.Set(
                _auraKeyboard.Feature<AuraApperancePacket>()
                    .Zone(zoneId)
                    .Animation(animation)
                    .Speed(speed)
                    .PrimaryColor(primary)
                    .SecondaryColor(secondary)
            );
            
            _auraKeyboard.Present();
        }
        
        private void Animate(byte zoneId, AuraAnimation animation, Color primary, AuraAnimationSpeed speed)
        {
            _auraKeyboard.Set(
                _auraKeyboard.Feature<AuraApperancePacket>()
                    .Zone(zoneId)
                    .Animation(animation)
                    .Speed(speed)
                    .PrimaryColor(primary)
            );
            
            _auraKeyboard.Present();
        }
        
        private void Animate(byte zoneId,
            AuraAnimation animation,
            AuraAnimationSpeed speed
        )
        {
            _auraKeyboard.Set(
                _auraKeyboard.Feature<AuraApperancePacket>()
                    .Zone(zoneId)
                    .Animation(animation)
                    .Speed(speed)
            );
            
            _auraKeyboard.Present();
        }
    }
}