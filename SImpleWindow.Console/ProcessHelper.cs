using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Security.Principal;

namespace SimpleWindow.Console
{
    public static class ProcessHelper
    {
        public static IEnumerable<ProcessData> GetCurrentUsersProcesses()
        {
            var currentUser = WindowsIdentity.GetCurrent();

            var searcher = new ManagementObjectSearcher("Select * from Win32_Process");
            if (searcher.Get().Count == 0) yield break;
            foreach (var managementBaseObject in searcher.Get())
            {
                var obj = (ManagementObject) managementBaseObject;
                var o = new String[1];
                try
                {
                    obj.InvokeMethod("GetOwnerSid", (object[])o);
                }
                catch (Exception e)
                {
                    // TODO
                }

                var sid = o[0];

                if (sid != currentUser.User.Value) continue;

                var id = (uint)obj["ProcessId"];
                var description = (string)obj["Description"];
                var name = (string)obj["Name"];

                var title = Process.GetProcessById((int)id).MainWindowTitle;

                yield return new ProcessData() { Id = id, MainWindowTitle = title, Name = name };
            }
        }
    }
}
