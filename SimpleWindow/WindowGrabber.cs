using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SimpleWindow
{
    public static class WindowGrabber
    {
        private static bool DWMWA_EXTENDED_FRAME_BOUNDS(IntPtr handle, out Rectangle rectangle)
        {
            RECT rect;
            var result = DwmGetWindowAttribute(handle, (int) Dwmwindowattribute.DwmwaExtendedFrameBounds,
                out rect, Marshal.SizeOf(typeof (RECT)));
            rectangle = Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
            return result >= 0;
        }

        [DllImport(@"dwmapi.dll")]
        private static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out RECT pvAttribute, int cbAttribute);

        private enum Dwmwindowattribute
        {
            DwmwaExtendedFrameBounds = 9
        }

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        public static Rectangle GetWindowRectangle(IntPtr handle)
        {
            if (Environment.OSVersion.Version.Major < 6)
            {
                return GetWindowRect(handle);
            }
            else
            {
                Rectangle rectangle;
                return DWMWA_EXTENDED_FRAME_BOUNDS(handle, out rectangle)
                           ? rectangle
                           : GetWindowRect(handle);
            }
        }

        private static Rectangle GetWindowRect(IntPtr handle)
        {
            RECT rect;
            GetWindowRect(handle, out rect);
            return Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        public static WindowData CaptureWindow(uint processId)
        {
            var proc = Process.GetProcessById((int)processId);
            var hWnd = proc.MainWindowHandle;
            var processWindowRectangle = GetWindowRectangle(hWnd);
            var bmp = new Bitmap(processWindowRectangle.Width, processWindowRectangle.Height, PixelFormat.Format32bppArgb);
            var gfxBmp = Graphics.FromImage(bmp);
            IntPtr hdcBitmap = gfxBmp.GetHdc();
            PrintWindow(hWnd, hdcBitmap, 0);
            gfxBmp.ReleaseHdc(hdcBitmap);
            gfxBmp.Dispose();

            return new WindowData { WindowPosition = processWindowRectangle, WindowScreenshot = bmp };
        }
    }

}
