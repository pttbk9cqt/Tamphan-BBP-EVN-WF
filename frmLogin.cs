using System;
using System.Windows.Forms;
using Tamphan_BBP_EVN_WF.Services;

namespace Tamphan_BBP_EVN_WF
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string LoginUsername = txtLoginUsername.Text.Trim();
            string LoginPassword = txtLoginPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(LoginUsername) || string.IsNullOrWhiteSpace(LoginPassword))
            {
                MessageBox.Show("Vui lòng nhập Username và Password.");
                return;
            }
            // Lấy MachineID của máy tính hiện tại và kiểm tra với server xem đã được cấp phép chưa
            string machineId = MachineService.GetMachineId();
            LicenseService licenseService = new LicenseService();
            //chỗ này nó tự động gửi machineId, username và pass lên server để check, nếu chưa có machineId nhưng người dùng nhập đúng Username và pass đã được lưu sẵn trong sheet Private, thì nó sẽ gửi một rows trạng thái đăng nhập lên sheet Log, và admin sẽ mở file LicenseTamphanEVNtools, copy cái machineId bên sheet Log sau đó add vào MachineID của sheet Private, thì lần đăng nhập tiếp theo ngay sau đó sẽ thành công
            // Ngoài ra, thêm try catch để nếu server lỗi, mạng lỗi hoặc Google Sheet API lỗi tránh app có thể crash, và sẽ hiện thông báo lỗi kết nối
            try
            {
                if (licenseService.CheckLicense(machineId, LoginUsername, LoginPassword))
                {
                    MachineService.SendMachineId(LoginUsername, LoginPassword);
                    frmHome Home = new frmHome();
                    Home.Show();
                    this.Hide();
                    // Khi form Home đóng lại thì form Login cũng sẽ đóng theo
                    Home.FormClosed += (s, args) => this.Close();
                }
                else
                {
                    MachineService.SendMachineId(LoginUsername, LoginPassword);
                    MessageBox.Show("Máy này chưa được kích hoạt.\n" + "Vui lòng liên hệ admin để mở quyền sử dụng.", "License", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể kết nối server.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
