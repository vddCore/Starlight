using System.Runtime.InteropServices;

namespace Hideous.Platform.Linux
{
    internal class LibUsb
    {
        private const string LibraryName = "libusb-1.0";

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int libusb_init(out IntPtr ctx);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int libusb_init(IntPtr ctx);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libusb_open_device_with_vid_pid(IntPtr ctx, ushort vid, ushort pid);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int libusb_reset_device(IntPtr devh);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "libusb_strerror")]
        private static extern IntPtr libusb_strerror_INTERNAL(int code);

        public static string libusb_strerror(int code)
        {
            return Marshal.PtrToStringAnsi(
                libusb_strerror_INTERNAL(code) /* as per libusb_manual: strerror never returns a null pointer */
            );
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int libusb_detach_kernel_driver(IntPtr devh, int iface);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int libusb_claim_interface(IntPtr devh, int iface);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int libusb_control_transfer(
            IntPtr devh,
            byte bmRequestType,
            byte bRequest,
            ushort wValue,
            ushort wIndex,
            IntPtr data,
            ushort wLength,
            uint timeout
        );
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int libusb_release_interface(IntPtr devh, int iface);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void libusb_close(IntPtr ctx);
    }
}