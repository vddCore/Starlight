using Hideous.Platform;

namespace Hideous
{
    public abstract class Device : IDisposable
    {
        private UsbProvider _usbProvider;

        protected Device(DeviceCharacteristics characteristics)
        {
            if (OperatingSystem.IsLinux())
            {
                _usbProvider = new LinuxUsbProvider(characteristics);
            }
            else if (OperatingSystem.IsWindows())
            {
                _usbProvider = new WindowsUsbProvider(characteristics);
            }
            else
            {
                throw new PlatformNotSupportedException("Your platform is not supported.");
            }
        }

        public T Feature<T>(params byte[] command) where T : FeaturePacket
        {
            try
            {
                return (T)Activator.CreateInstance(typeof(T), command)!;
            }
            catch
            {
                return (T)Activator.CreateInstance(typeof(T))!;
            }
        }

        public T Packet<T>(params byte[] command) where T : Packet
        {
            try
            {
                return (T)Activator.CreateInstance(typeof(T), command)!;
            }
            catch
            {
                return (T)Activator.CreateInstance(typeof(T))!;
            }
        }

        public void Set(FeaturePacket packet)
            => _usbProvider.Set(packet.Data);

        public byte[] Get(FeaturePacket packet)
            => _usbProvider.Get(packet.Data);

        public void Write(Packet packet)
            => _usbProvider.Write(packet.Data);

        public void Dispose()
            => _usbProvider.Dispose();
    }
}