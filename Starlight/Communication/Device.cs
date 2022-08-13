using System.Reflection;
using HidSharp;

namespace Starlight.Communication
{
    public abstract class Device : IDisposable
    {
        protected HidDevice HidDevice { get; }
        protected HidStream HidStream { get; }

        protected Device(ushort vendorId, ushort productId, int maxFeatureReportLength)
        {
            try
            {
                HidDevice = DeviceList.Local
                    .GetHidDevices(vendorId, productId)
                    .First(x => x.GetMaxFeatureReportLength() == maxFeatureReportLength);
            }
            catch
            {
                throw new IOException("AniMe Matrix control device was not found on your machine.");
            }

            var config = new OpenConfiguration();
            config.SetOption(OpenOption.Interruptible, true);
            config.SetOption(OpenOption.Exclusive, false);
            config.SetOption(OpenOption.Priority, 10);

            HidStream = HidDevice.Open(config);
        }

        protected T Packet<T>(params byte[] command) where T: Packet
        {
            return (T)Activator.CreateInstance(typeof(T), command)!;
        }

        protected void Set(Packet packet)
           => packet.Set(HidStream);

        protected byte[] Get(Packet packet)
            => packet.Get(HidStream);

        public virtual void Dispose()
        {
            HidStream.Dispose();
        }
    }
}