namespace Starlight.Asus.AuraKeyboard;

public class AuraKeyboardInformation
{
    public byte BacklightType { get; }
    public byte ProductionYearHex { get; }
    public byte LayoutType { get; }
    public byte FeatureBitmap1 { get; }
    public byte FeatureBitmap2 { get; }
    public byte? ModelType { get; }

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
        ModelType = modelType;
    }
}