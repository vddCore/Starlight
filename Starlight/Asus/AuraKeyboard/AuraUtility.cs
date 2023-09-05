namespace Starlight.Asus.AuraKeyboard
{
    static internal class AuraUtility
    {
        public static byte RemapDefaultNKeyMode(byte defaultModeByte)
        {
            return defaultModeByte switch
            {
                0x00 => 0,
                0x01 => 1,
                0x02 => 3,
                0x03 => 4,
                0x04 => 9,
                0x05 => 10,
                0x06 => 11,
                0x07 => 12,
                0x08 => 13,
                0x0A => 2,
                0x0B => 5,
                0x0C => 6,
                0x0D => 16,
                0x0E => 14,
                0x11 => 17,
                _ => defaultModeByte
            };
        }
    }
}