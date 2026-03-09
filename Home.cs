using System;
using System.Windows.Forms;
using Tamphan_BBP_EVN_WF.Models;
using Tamphan_BBP_EVN_WF.Services;

namespace Tamphan_BBP_EVN_WF
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void Btn_Login_account_riêng_lẻ_Click(object sender, EventArgs e)
        {
            string maKH = textBox_nhập_mã_khách_hàng.Text.Trim();
            //Kiểm tra mã khách hàng
            if (maKH.Length == 5 && !maKH.StartsWith("PB010500"))
            {
                maKH = "PB010500" + maKH;
                textBox_nhập_mã_khách_hàng.Text = maKH;
            }
            ExcelAccountEVNService service = new ExcelAccountEVNService();
            AccountEVN acc = service.GetAccount(maKH);
            if (acc == null)
            {
                MessageBox.Show("Mã khách hàng không tồn tại trong file excel");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox_password.Text) || textBox_password.Text != acc.Password)
            {
                textBox_password.Text = acc.Password;
            }
            //MessageBox.Show($"ID: {acc.Id}\n" + $"Mục đích sử dụng: {acc.MucDichSuDung}\n" + $"Tên đăng nhập: {acc.MaKH}\n" + $"Pass: {acc.Password}");

            EVNSPC_WEB_LOGIN frmwebdienluc = new EVNSPC_WEB_LOGIN(maKH);
            frmwebdienluc.Show();
        }

        private void Btn_login_download_no_UI_Click(object sender, EventArgs e)
        {
            string maKH = textBox_nhập_mã_khách_hàng.Text.Trim();
            if (maKH.Length == 5 && !maKH.StartsWith("PB010500"))
            {
                maKH = "PB010500" + maKH;
                textBox_nhập_mã_khách_hàng.Text = maKH;
            }
            ExcelAccountEVNService service_no_UI = new ExcelAccountEVNService();
            AccountEVN acc_no_UI = service_no_UI.GetAccount(maKH);
            if (acc_no_UI == null)
            {
                MessageBox.Show("Mã khách hàng không tồn tại trong file excel");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox_password.Text) || textBox_password.Text != acc_no_UI.Password)
            {
                textBox_password.Text = acc_no_UI.Password;
            }
            Auto_Download frmwebdienluc_no_UI = new Auto_Download(maKH);
            frmwebdienluc_no_UI.Show();
        }

        private void btn_evn_download_Click(object sender, EventArgs e)
        {
            string maKH = textBox_nhập_mã_khách_hàng.Text.Trim();
            if (maKH.Length == 5 && !maKH.StartsWith("PB010500"))
            {
                maKH = "PB010500" + maKH;
                textBox_nhập_mã_khách_hàng.Text = maKH;
            }
            ExcelAccountEVNService service = new ExcelAccountEVNService();
            AccountEVN acc = service.GetAccount(maKH);
            if (acc == null)
            {
                MessageBox.Show("Mã khách hàng không tồn tại trong file excel");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox_password.Text) || textBox_password.Text != acc.Password)
            {
                textBox_password.Text = acc.Password;
            }
            EVNSPC_DownloadThongbao frmwebdienluc_download = new EVNSPC_DownloadThongbao(maKH);
            frmwebdienluc_download.Show();
        }

    }
}
