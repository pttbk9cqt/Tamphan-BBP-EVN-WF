using System;
using System.ComponentModel;
using System.Windows.Forms;
using Tamphan_WorkingBCMBP_WF;
using Tamphan_WorkingBCMBP_WF.Services;

namespace Tamphan_BBP_EVN_WF
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
            //Application.Run(new Cre1506("phanthanhtam","Mocungcunganhcungnhat@bcm26","https://eoffice.becamexbinhphuoc.com.vn/workflow/SitePages/NewWorkflow.aspx?mode=1&ListID=589dfff1-f412-41fd-8824-c48a2bf66309"));
        }
    }
}

