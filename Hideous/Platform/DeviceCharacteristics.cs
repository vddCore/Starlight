using HidSharp;

namespace Hideous.Platform
{
    public record DeviceCharacteristics(
        ushort VendorID,
        ushort ProductID,
        int? MaxFeatureReportLength = null,
        int? MaxInputReportLength = null,
        int? MaxOutputReportLength = null)
    {
        public bool SatisfiedBy(HidDevice device)
        {
            var result = device.VendorID == VendorID
                         && device.ProductID == ProductID;

            if (result == false)
                return false;

            if (MaxFeatureReportLength != null)
                result = device.GetMaxFeatureReportLength() == MaxFeatureReportLength;

            if (MaxInputReportLength != null)
                result = result && device.GetMaxInputReportLength() == MaxInputReportLength;

            if (MaxOutputReportLength != null)
                result = result && device.GetMaxOutputReportLength() == MaxOutputReportLength;

            return result;
        }
    }
}