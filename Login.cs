using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tamphan_BBP_EVN_WF;
using Tamphan_WorkingBCMBP_WF.Services;

namespace Tamphan_WorkingBCMBP_WF
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private void btn_loginfrm_login_Click(object sender, EventArgs e)
        {
            // Lấy MachineID
            string machineId = MachineService.GetMachineId();
            string userlogintools = textBox_loginfrm_username.Text.Trim();
            string passwordlogintools = textBox_loginfrm_password.Text.Trim();
            // HIỂN THỊ MACHINE ID ĐỂ COPY
            //MessageBox.Show(machineId);

            // Kiểm tra license
            LicenseService licenseService = new LicenseService();

            if (licenseService.CheckLicense(machineId, userlogintools, passwordlogintools))
            {
                MachineService.SendMachineId(userlogintools, passwordlogintools);
                new Home().Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu, nhập lại");
            }
        }     
    }
}
