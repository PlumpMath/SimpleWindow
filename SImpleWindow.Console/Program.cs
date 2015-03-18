using System;
using System.Diagnostics;

namespace SimpleWindow.Console
{
    class Program
    {
        private static void Main(string[] args)
        {
            var processes = ProcessHelper.GetCurrentUsersProcesses();
            foreach (var process in processes)
            {
                System.Console.WriteLine(process.Id + "-" + process.MainWindowTitle);
            }

            var pidString = System.Console.ReadLine();

            WindowData window;
            try
            {
                window = WindowGrabber.CaptureWindow(Convert.ToUInt32(pidString));
                window.WindowScreenshot.Save("C:\\test.bmp");
                Process.Start("C:\\Windows\\system32\\mspaint.exe", "C:\\test.bmp");
            }
            catch (FormatException){ }

            System.Console.ReadLine();
        }
    }
}
