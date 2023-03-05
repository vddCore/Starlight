using Starlight.Communication;

namespace Starlight.Asus.Aura
{
    public class AuraDevice : Device
    {
        public AuraMode Mode { get; }

        public AuraDevice()
            : base(new(0x0B05, 0x19B6, maxFeatureReportLength: 64, maxInputReportLength: 32))
        {
            Mode = new AuraMode(this);
        }

        public void SetBrightness(BrightnessLevel level)
        {
            Write(Hid<AuraPacket>(0x5D, 0xBA, 0xC5, 0xC4, (byte)level));
        }

        internal void Present()
        {
            Write(Hid<AuraPacket>(0x5D, 0xB4));
        }
    }
}