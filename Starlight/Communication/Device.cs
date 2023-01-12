using Starlight.Communication.Platform;

namespace Starlight.Communication
{
    public abstract class Device : IDisposable
    {
        private static UsbProvider _usbProvider;

        protected Device(ushort vendorId, ushort productId, int maxFeatureReportLength)
        {
            if (OperatingSystem.IsLinux())
            {
                _usbProvider = new LinuxUsbProvider(vendorId, productId);
            }
            else if (OperatingSystem.IsWindows())
            {
                _usbProvider = new WindowsUsbProvider(vendorId, productId, maxFeatureReportLength);
            }
        }

        protected T Packet<T>(params byte[] command) where T : Packet
        {
            return (T)Activator.CreateInstance(typeof(T), command)!;
        }

        public void Set(Packet packet)
            => _usbProvider?.Set(packet.Data);

        public byte[] Get(Packet packet)
            => _usbProvider?.Get(packet.Data);

        public void Dispose()
        {
            _usbProvider?.Dispose();
        }
    }
}