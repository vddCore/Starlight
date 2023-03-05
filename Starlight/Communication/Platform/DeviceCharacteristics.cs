using HidSharp;

namespace Starlight.Communication.Platform
{
    public struct DeviceCharacteristics
    {
        public ushort VendorID { get; }
        public ushort ProductID { get; }

        public int? MaxFeatureReportLength { get; }
        public int? MaxInputReportLength { get; }
        public int? MaxOutputReportLength { get; }

        public DeviceCharacteristics(
            ushort vendorId,
            ushort productId,
            int? maxFeatureReportLength = null,
            int? maxInputReportLength = null,
            int? maxOutputReportLength = null)
        {
            VendorID = vendorId;
            ProductID = productId;
            
            MaxFeatureReportLength = maxFeatureReportLength;
            MaxInputReportLength = maxInputReportLength;
            MaxOutputReportLength = maxOutputReportLength;
        }

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