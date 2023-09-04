namespace Starlight.Asus.AuraKeyboard
{
    public class AuraKeyboardInformation
    {
        public byte BacklightTypeByte { get; }
        public byte ProductionYearHex { get; }
        public byte LayoutTypeByte { get; }
        public byte FeatureBitmap1 { get; }
        public byte FeatureBitmap2 { get; }
        public byte? ModelTypeByte { get; }

        public AuraKeyboardFamily Family
        {
            get
            {
                return ModelTypeByte switch
                {
                    null => throw new InvalidOperationException("This field is not supported by the current device."),
                    0x01 => AuraKeyboardFamily.Strix,
                    0x02 => AuraKeyboardFamily.Flow,
                    0x04 => AuraKeyboardFamily.Zephyrus,
                    0x08 => AuraKeyboardFamily.TUF,
                    0x10 => AuraKeyboardFamily.NR2301,
                    0x20 => AuraKeyboardFamily.Desktop,
                    _ => AuraKeyboardFamily.Generic
                };
            }
        }

        public AuraKeyboardBacklightType BacklightType => (AuraKeyboardBacklightType)BacklightTypeByte;
        public AuraKeyboardLayout Layout => (AuraKeyboardLayout)LayoutTypeByte;
    
        public bool SupportsLogoLED => (FeatureBitmap1 & 0x01) != 0;
        public bool SupportsLightBar => (FeatureBitmap1 & 0x02) != 0;
        public bool SupportsVCut => (FeatureBitmap1 & 0x10) != 0;
        public bool SupportsAero => (FeatureBitmap1 & 0x20) != 0;
        public bool SupportsBump => (FeatureBitmap1 & 0x40) != 0;
        public bool SupportsRearGlow => (FeatureBitmap1 & 0x80) != 0;
        public bool SupportsDefaultColor => (FeatureBitmap2 & 0x04) != 0;
        public bool SupportsRGBWheel => (FeatureBitmap2 & 0x08) != 0;
        public bool SupportsOneZoneRedEffect => (FeatureBitmap2 & 0x10) != 0;
        public bool SupportsBitFormatKeyPosition => (FeatureBitmap2 & 0x40) != 0;
    
        public AuraKeyboardInformation(
            byte backlightTypeByte,
            byte productionYearHex,
            byte layoutTypeByte,
            byte featureBitmap1, 
            byte featureBitmap2,
            byte? modelTypeByte)
        {
            BacklightTypeByte = backlightTypeByte;
            ProductionYearHex = productionYearHex;
            LayoutTypeByte = layoutTypeByte;
            FeatureBitmap1 = featureBitmap1;
            FeatureBitmap2 = featureBitmap2;
            ModelTypeByte = modelTypeByte;
        }
    }
}