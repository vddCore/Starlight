using System.Text;
using Starlight.Communication;

namespace Starlight.Asus.AuraKeyboard
{
    public class AuraKeyboardDevice : Device
    {
        public AuraMode Mode { get; }

        public AuraKeyboardDevice()
            : base(new(0x0B05, 0x19B6, maxFeatureReportLength: 64, maxInputReportLength: 32))
        {
            Mode = new AuraMode(this);
        }

        public AuraKeyboardInformation GetStatus()
        {
            Set(Feature<AuraPacket>().AppendData(Encoding.ASCII.GetBytes("ASUS Tech.Inc.")));
            Get(Feature<AuraPacket>());

            var str = Encoding.ASCII.GetBytes(" 1");

            Set(Feature<AuraPacket>()
                .AppendData(0x05)
                .AppendData(str)
                .AppendData(0x00, 0x20)
            );

            var statusData = Get(Feature<AuraPacket>());
            // statusData[5]: Keyboard Status Data Length (starts at statusData[9])
            // statusData[9]: Keyboard Backlight Type
            // statusData[10]: Keyboard Production Year (in hex, i.e. 2022 == 0x22)
            // statusData[11]: unused
            // statusData[12]: Keyboard Layout Type
            // statusData[13]: Bitmap:
            //   statusData[13] & 0x01: Has logo LED (1 == supported, 0 == unsupported)
            //   statusData[13] & 0x02: Has lightbar (1 == supported, 0 == unsupported)
            //   statusData[13] & 0x10: Has VCut (whatever that is) (1 == supported, 0 == unsupported)
            //   statusData[13] & 0x20: Has Aero (whatever that is) (1 == supported, 0 == unsupported)
            //   statusData[13] & 0x40: Has Bump (whatever that is) (1 == supported, 0 == unsupported)
            //   statusData[13] & 0x80: Has rear glow (1 == supported, 0 == unsupported)
            // statusData[14]: Bitmap:
            //    statusData[14] & 0x04: Supports default color
            //    statusData[14] & 0x08: Supports RGB wheel
            //    statusData[14] & 0x10: Supports One Zone Red effect (whatever that is)
            //    statusData[14] & 0x40: Supports bit format key position
            // statusData[17]: Keyboard Model Type -- valid only if statusData[10] >= 23

            return new AuraKeyboardInformation(
                statusData[0x09],
                statusData[0x0A],
                statusData[0x0C],
                statusData[0x0D],
                statusData[0x0E],
                statusData[0x0A] >= 0x23 ? statusData[0x11] : null // only preset on models from 2023 onwards
            );
        }

        public void SetBrightness(BrightnessLevel level)
        {
            Set(Feature<AuraPacket>(0xBA, 0xC5, 0xC4, (byte)level));
        }

        internal void Present()
        {
            Set(Feature<AuraPacket>(0xBA, 0xB4));
        }
    }
}