using System.Text;
using Hideous;

namespace Starlight.Asus.AuraKeyboard
{
    public class AuraKeyboardDevice : Device
    {
        public AuraMode Mode { get; }

        public AuraKeyboardDevice()
            : base(new(0x0B05, 0x19B6, MaxFeatureReportLength: 64, MaxInputReportLength: 32))
        {
            Mode = new AuraMode(this);
        }

        public AuraKeyboardInformation QueryStatus()
        {
            Set(Feature<AuraPacket>().AppendData(Encoding.ASCII.GetBytes("ASUS Tech.Inc.")));
            Get(Feature<AuraPacket>());

            // 0x05, " 1\0 "
            Set(Feature<AuraPacket>().AppendData(0x05, 0x20, 0x31, 0x00, 0x20));
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

        public AuraKeyboardLayout QueryKeyboardLayout()
        {
            var packet = Feature<AuraPacket>(0xB9);
            Set(packet);
            return (AuraKeyboardLayout)Get(packet)[4];
        }

        public byte[] QueryDefaultModes()
        {
            var packet = Feature<AuraPacket>(0x9B, 0x01, 0x01, 0x08);
            Set(packet);
            return Get(packet);

            // todo: organize this into a model class
            // 0x05: NKeyDefaultMode, ASUS driver remaps it for some reason. See AuraUtility for details.
            // 0x06: NKeyDefaultColorR
            // 0x07: NKeyDefaultColorG
            // 0x08: NKeyDefaultColorB
            // 0x09: NKDefaultPowerMode
            // 0x0A: NKDefaultPowerColorR
            // 0x0B: NKDefaultPowerColorG
            // 0x0C: NKDefaultPowerColorB
        }

        // currently no clue what is this about.
        // data[3] is supposed to contain what the driver calls
        // FnKeyStatus
        private bool IsFnKeyPressed()
        {
            var packet = Feature<AuraPacket>(0xC3, 0x01);
            Set(packet);
            var data = Get(packet);

            return data[3] != 0;
        }

        // probably for GA401:
        // data[2] is the current config status bitmap:
        //   data[2] & 0x08: Built-in bootup animation
        //   data[2] & 0x04: Built-in shutdown animation
        //   data[2] & 0x02: Built-in sleep animation
        //   data[2] & 0x01: Built-in running animation
        //
        private byte[] QueryAnimeMatrixConfig()
        {
            Set(Feature<AuraPacket>(0xC6));
            return Get(Feature<AuraPacket>(0xC6));
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