using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SimpleWindow
{
    public static class WindowGrabber
    {
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);

        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        public static WindowData CaptureWindow(uint processId)
        {
            var proc = Process.GetProcessById((int)processId);
            var hWnd = proc.MainWindowHandle;

            var window = new ProcessWindow(proc);
            window.Focus();
            window.BringToTop();

            Rect rc;
            GetWindowRect(hWnd, out rc);

            var bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
            var gfxBmp = Graphics.FromImage(bmp);
            IntPtr hdcBitmap = gfxBmp.GetHdc();
            PrintWindow(hWnd, hdcBitmap, 0);
            gfxBmp.ReleaseHdc(hdcBitmap);
            gfxBmp.Dispose();

            Debug.WriteLine("Grabber : " + rc);
            return new WindowData { WindowPosition = new Rectangle(rc.X, rc.Y, rc.Width, rc.Height), WindowScreenshot = bmp };
        }
    }

}
