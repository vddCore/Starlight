using static Starlight.Communication.Platform.Linux.LibUsb;

namespace Starlight.Communication.Platform
{
    internal class LinuxUsbProvider : UsbProvider
    {
        private IntPtr _libUsbContext;
        private IntPtr _deviceHandle;
        
        public LinuxUsbProvider(ushort vendorId, ushort productId) 
            : base(vendorId, productId)
        {
            var errcode = 0;
            
            if ((errcode = libusb_init(out _libUsbContext)) < 0)
            {
                throw new IOException($"Unable to create a LibUSB context: {libusb_strerror(errcode)} ({errcode})");
            }

            if (_libUsbContext == IntPtr.Zero)
            {
                throw new IOException("Unable to create a LibUSB context.");
            }
            
            _deviceHandle = libusb_open_device_with_vid_pid(_libUsbContext, vendorId, productId);

            if (_deviceHandle == IntPtr.Zero)
            {
                throw new IOException(
                    "AniMe Matrix control device was not found on your machine or you have no permissions to open it."
                );
            }

            if ((errcode = libusb_reset_device(_deviceHandle)) < 0)
            {
                throw new IOException(
                    $"Unable to reset the AniMe Matrix control device: {libusb_strerror(errcode)} ({errcode})"
                );
            }

            if ((errcode = libusb_detach_kernel_driver(_deviceHandle, 0)) < 0 && errcode != -5)
            {
                throw new IOException(
                    $"Unable to detach kernel driver from the AniMe Matrix control device: {libusb_strerror(errcode)} ({errcode})"
                );
            }

            if ((errcode = libusb_claim_interface(_deviceHandle, 0)) < 0)
            {
                throw new IOException(
                    $"Unable to claim interface 0 for the AniMe Matrix control device: {libusb_strerror(errcode)} ({errcode})"
                );
            }
        }

        public override void Set(byte[] data)
        {
            if (_libUsbContext == IntPtr.Zero)
            {
                throw new IOException("LibUSB context was null.");
            }

            if (_deviceHandle == IntPtr.Zero)
            {
                throw new IOException("Device handle was null.");
            }

            byte bmRequestType = 0x21;
            byte bRequest = 0x09;
            ushort wValue = 0x035E;
            ushort wIndex = 0;
            ushort wLength = (ushort)data.Length;

            unsafe
            {
                fixed (byte* bp = &data[0])
                {
                    var errcode = libusb_control_transfer(
                        _deviceHandle,
                        bmRequestType,
                        bRequest,
                        wValue,
                        wIndex,
                        (IntPtr)bp,
                        wLength,
                        250
                    );

                    if (errcode < 0)
                    {
                        throw new IOException(
                            $"Unable to write to the AniMe Matrix control device: {libusb_strerror(errcode)})"
                        );
                    }
                }
            }
        }

        public override byte[] Get(byte[] data)
        {
            return null;
        }

        public override void Dispose()
        {
            if (_deviceHandle != IntPtr.Zero)
            {
                libusb_release_interface(_deviceHandle, 0);
                _deviceHandle = IntPtr.Zero;
            }

            if (_libUsbContext != IntPtr.Zero)
            {
                libusb_close(_libUsbContext);
                _libUsbContext = IntPtr.Zero;
            }
        }
    }
}