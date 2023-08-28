using Starlight.Communication.Platform;

namespace Starlight.Communication
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
        }

        internal T Feature<T>(params byte[] command) where T : FeaturePacket
        {
            return (T)Activator.CreateInstance(typeof(T), command)!;
        }
        
        internal T Hid<T>(params byte[] command) where T : Packet
        {
            try
            {
                return (T)Activator.CreateInstance(typeof(T), command)!;
            }
            catch
            {
                // we'll try something else.
            }

            return (T)Activator.CreateInstance(typeof(T))!;
        }

        internal void Set(FeaturePacket packet)
            => _usbProvider?.Set(packet.Data);

        internal byte[] Get(FeaturePacket packet)
            => _usbProvider?.Get(packet.Data);

        internal void Write(HidPacket packet)
            => _usbProvider?.Write(packet.Data);

        public void Dispose()
        {
            _usbProvider?.Dispose();
        }
    }
}