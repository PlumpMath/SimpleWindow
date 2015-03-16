using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SimpleWindow
{
    public class ProcessWindow
    {
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool BringWindowToTop(IntPtr hWnd);

        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_SHOWMAXIMIZED = 3;

        private readonly IntPtr hWnd;

        public ProcessWindow(int pID) : this(Process.GetProcessById(pID)) { }

        public ProcessWindow(Process process)
        {
            hWnd = process.MainWindowHandle;
        }

        public void Focus()
        {
            ShowWindow(hWnd, SW_SHOWNORMAL);
        }

        public void Minimize()
        {
            ShowWindow(hWnd, SW_SHOWMINIMIZED);
        }

        public void Maximize()
        {
            ShowWindow(hWnd, SW_SHOWMAXIMIZED);
        }

        public void BringToTop()
        {
            BringWindowToTop(hWnd);
        }
    }
}
