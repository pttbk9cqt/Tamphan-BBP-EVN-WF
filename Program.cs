using System;
using System.ComponentModel;
using System.Windows.Forms;
using Tamphan_WorkingBCMBP_WF.Services;

//namespace Tamphan_BBP_EVN_WF
//{
//    internal static class Program
//    {
//        /// <summary>
//        /// The main entry point for the application.
//        /// </summary>
//        [STAThread]
//        static void Main()
//        {
//            Application.EnableVisualStyles();
//            Application.SetCompatibleTextRenderingDefault(false);
//            Application.Run(new Home());
//            //Application.Run(new Cre1506("phanthanhtam","Mocungcunganhcungnhat@bcm26","https://eoffice.becamexbinhphuoc.com.vn/workflow/SitePages/NewWorkflow.aspx?mode=1&ListID=589dfff1-f412-41fd-8824-c48a2bf66309"));
//        }
//    }
//}

namespace Tamphan_BBP_EVN_WF
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Lấy MachineID
            string machineId = MachineService.GetMachineId();

            // HIỂN THỊ MACHINE ID ĐỂ COPY
            //MessageBox.Show(machineId);

            // Kiểm tra license
            LicenseService licenseService = new LicenseService();
            bool valid = licenseService.CheckLicense(machineId, "TamPhan");

            if (!valid)
            {
                MessageBox.Show("Không thể kết nối server",
                                "License Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            Application.Run(new Home());
        }
    }
}

//namespace Tamphan_BBP_EVN_WF
//{
//    internal static class Program
//    {
//        [STAThread]
//        static void Main()
//        {
//            Application.EnableVisualStyles();
//            Application.SetCompatibleTextRenderingDefault(false);

//            // Lấy MachineID
//            string machineId = MachineService.GetMachineId();

//            // Kiểm tra license
//            LicenseService licenseService = new LicenseService();
//            bool valid = licenseService.CheckLicense(machineId, "TamPhan");

//            if (!valid)
//            {
//                MessageBox.Show("Không thể kết nối server ","License Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
//                return; // không chạy tool
//            }

//            // License OK → chạy tool
//            Application.Run(new Home());
//        }
//    }
//}
