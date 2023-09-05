using System.ComponentModel;
using HidSharp;

namespace Hideous.Platform
{
    internal class WindowsUsbProvider : UsbProvider
    {
        protected HidDevice HidDevice { get; }
        protected HidStream HidStream { get; }

        public WindowsUsbProvider(DeviceCharacteristics characteristics) 
            : base(characteristics.VendorID, characteristics.ProductID)
        {
            try
            {
                HidDevice = DeviceList.Local
                    .GetHidDevices(VendorID, ProductID)
                    .First(characteristics.SatisfiedBy);
            }
            catch
            {
                throw new IOException("The requested HID device was not found on your machine.");
            }

            var config = new OpenConfiguration();
            config.SetOption(OpenOption.Interruptible, true);
            config.SetOption(OpenOption.Exclusive, false);
            config.SetOption(OpenOption.Priority, 10);

            HidStream = HidDevice.Open(config);
        }

        public override void Set(byte[] data)
        {
            WrapException(() =>
            {
                HidStream.SetFeature(data);
                HidStream.Flush();
            });
        }

        public override byte[] Get(byte[] data)
        {
            var outData = new byte[data.Length];
            Array.Copy(data, outData, data.Length);
            
            WrapException(() =>
            {
                HidStream.GetFeature(outData);
                HidStream.Flush();
            });

            return outData;
        }

        public override void Write(byte[] data)
        {
            WrapException(() =>
            {
                HidStream.Write(data);
                HidStream.Flush();
            });
        }

        public override void Dispose()
        {
            HidStream.Dispose();
        }
        
        private void WrapException(Action action)
        {
            try
            {
                action();
            }
            catch (IOException e)
            {
                if (e.InnerException is Win32Exception w32e)
                {
                    if (w32e.NativeErrorCode != 0)
                    {
                        throw;
                    }
                }
            }
        }
    }
}