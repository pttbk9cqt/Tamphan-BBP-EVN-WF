using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Tamphan_WorkingBCMBP_WF.Services
{
    public static class MachineService
    {
        public static string GetMachineId()
        {
            var searcher = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor");

            foreach (ManagementObject obj in searcher.Get())
            {
                return obj["ProcessorId"].ToString();
            }

            return "UNKNOWN";
        }
    }
}
