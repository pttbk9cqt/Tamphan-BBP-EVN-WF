using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tamphan_WorkingBCMBP_WF.Models
{
    public class LicenseInfo
    {
        public string MachineId { get; set; }
        public string Customer { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Status { get; set; }
    }
}
