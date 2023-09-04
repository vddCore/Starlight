namespace Starlight.Asus.AuraKeyboard;

public class AuraKeyboardInformation
{
    public enum KeyboardFamily : byte
    {
        Strix = 0x0F,
        TUF = 0x15,
        Zephyrus = 0x16,
        NR2301 = 0x17,
        Flow = 0x18,
        Desktop = 0x1F,
        Generic = 0xFF
    }
    
    public byte BacklightType { get; }
    public byte ProductionYearHex { get; }
    public byte LayoutType { get; }
    public byte FeatureBitmap1 { get; }
    public byte FeatureBitmap2 { get; }
    public byte? ModelTypeByte { get; }

    public KeyboardFamily Family
    {
        get
        {
            return ModelTypeByte switch
            {
                null => throw new InvalidOperationException("This field is not supported by the current device."),
                0x01 => KeyboardFamily.Strix,
                0x02 => KeyboardFamily.Flow,
                0x04 => KeyboardFamily.Zephyrus,
                0x08 => KeyboardFamily.TUF,
                0x10 => KeyboardFamily.NR2301,
                0x20 => KeyboardFamily.Desktop,
                _ => KeyboardFamily.Generic
            };
        }
    }
    
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
    
    public AuraKeyboardInformation(byte backlightType, byte productionYearHex, byte layoutType, byte featureBitmap1, byte featureBitmap2, byte? modelType)
    {
        BacklightType = backlightType;
        ProductionYearHex = productionYearHex;
        LayoutType = layoutType;
        FeatureBitmap1 = featureBitmap1;
        FeatureBitmap2 = featureBitmap2;
        ModelTypeByte = modelType;
    }
}