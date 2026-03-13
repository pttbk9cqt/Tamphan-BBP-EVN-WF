using System;

namespace Tamphan_BBP_EVN_WF.Models
{
    public class LicenseInfo
    {
        public string MachineId { get; set; }
        public string Customer { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Status { get; set; }
    }
}
