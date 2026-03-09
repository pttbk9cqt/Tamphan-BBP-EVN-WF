using CefSharp;
using CefSharp.WinForms;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tamphan_BBP_EVN_WF.Models;
using Tamphan_BBP_EVN_WF.Services;

namespace Tamphan_BBP_EVN_WF
{
    public partial class Auto_Download : Form
    {
        private string _maKH_Hidden;
        private CaptchaHelper _captchaHelper_Hidden;
        string kyHoaDon_Hidden = "12.2025";
        public Auto_Download(string maKH_no_UI)
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            _maKH_Hidden = maKH_no_UI;

            this.WindowState = FormWindowState.Maximized;
            this.MouseDown += (s, e) =>
            {
                MessageBox.Show($"Click at {e.X},{e.Y}");
            };
            try
            {
                InitBrowser();
                _captchaHelper_Hidden = new CaptchaHelper(webhidden); // tạo helper
            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi khởi tạo trình duyệt: " + ex.Message);
            }
        }
        string BuildPdfName_Hidden(string maKH_no_UI)
        {
            _maKH_Hidden = maKH_no_UI;
            ExcelAccountEVNService service_Hidden = new ExcelAccountEVNService();
            AccountEVN acc_Hidden = service_Hidden.GetAccount(_maKH_Hidden);
            //MessageBox.Show("Đang lưu file cho mã KH: " + acc.MucDichSuDung);
            return acc_Hidden.MucDichSuDung + "_" + "Thông báo tiền điện tháng " + kyHoaDon_Hidden + "_" + _maKH_Hidden + ".pdf";
        }
        private void InitBrowser()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(InitBrowser));
            }
            else
            {
                if (Cef.IsInitialized != true)
                {
                    CefSettings settings = new CefSettings();
                    //settings.ChromeRuntime = true;
                    settings.BrowserSubprocessPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "CefSharp.BrowserSubprocess.exe");
                    Cef.Initialize(settings);
                }
                webhidden.FrameLoadEnd += WebBrowser_Hidden_FrameLoadEnd;
                string url = "https://cskh.evnspc.vn/TaiKhoan/DangNhap?previousLink=/TraCuu/HoaDonTienDien";
                //weblogin.DownloadHandler = new DownloadHandler();
                var downloadHandler = new BlobPdfDownloadHandler(
                                                    @"C:\Users\pttbk\Downloads", () => BuildPdfName_Hidden(_maKH_Hidden));
                downloadHandler.PdfDownloaded += delegate (string path)
                {
                    Console.WriteLine("PDF saved: " + path);
                };
                webhidden.DownloadHandler = downloadHandler;
                webhidden.Load(url);
            }
        }

        private async void WebBrowser_Hidden_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (!e.Frame.IsMain)
                return;
            if (!e.Url.Contains("cskh.evnspc.vn/TaiKhoan/DangNhap"))
                return;
            ExcelAccountEVNService service = new ExcelAccountEVNService();
            AccountEVN acc = service.GetAccount(_maKH_Hidden);

            if (acc == null)
                return;

            string fill_maKH_pass_Script = $@"
            (function() 
            {{
                let userInput = document.querySelector('input[placeholder=""TÊN ĐĂNG NHẬP""]');
                let passInput = document.querySelector('input[placeholder=""MẬT KHẨU""]');
                if (userInput && passInput) 
                {{
                    userInput.value = '{acc.MaKH}';
                    userInput.dispatchEvent(new Event('input', {{ bubbles: true }}));
                    passInput.value = '{acc.Password}';
                    passInput.dispatchEvent(new Event('input', {{ bubbles: true }}));
                }}
            }})();
            ";
            webhidden.ExecuteScriptAsync(fill_maKH_pass_Script);
            //tới đây là đã tự điền mã KH và pass

            // đợi captcha render
            await Task.Delay(500);

            // GỌI AUTO CAPTCHA TẠI ĐÂY
            await _captchaHelper_Hidden.AutoFillCaptchaAsync();
            // ĐẾN ĐÂY LÀ ĐÃ ĐIỀN XONG CAPTCHA VÀO TRANG WEB

            //Ghi lại địa chỉ trang web hiện tại
            string currentUrl = webhidden.Address;

            await Task.Delay(700);
            // Tiếp đó là bấm nút đăng nhập
            webhidden.ExecuteScriptAsync("document.getElementById('btnDangNhap').click();");

            await Task.Delay(700); // chờ load trang sau khi đăng nhập

            //nếu bị lỗi captcha hoặc đăng nhập không thành công thì thử lại
            for (int i = 0; i <= 3; i++) // thử 3 lần
                if (webhidden.Address.Contains("cskh.evnspc.vn/TaiKhoan/DangNhap"))
                {
                    webhidden.Reload();
                    await Task.Delay(1000); // chờ load lại trang
                    webhidden.ExecuteScriptAsync(fill_maKH_pass_Script);
                    await _captchaHelper_Hidden.AutoFillCaptchaAsync();
                    await Task.Delay(2000);
                    //MessageBox.Show("ID:" + acc.Id + " mã KH:" + acc.MaKH + " " + acc.MucDichSuDung);
                    webhidden.ExecuteScriptAsync("document.getElementById('btnDangNhap').click();");
                    await Task.Delay(2000);
                }
                else
                {
                    break; // đăng nhập thành công, thoát vòng lặp
                }
            // tới đây là đã đăng nhập thành công rồi
            //Bấm vào nút xác nhận như kiểu tài khoản đăng nhập mới, còn không có thì thôi kệ
            webhidden.ExecuteScriptAsync("document.getElementById('btnXacNhan').click();");
            await Task.Delay(1000); // chờ load trang sau khi bấm xác nhận
            //Bấm nút kết thúc để vào trang tra cứu hóa đơn
            webhidden.ExecuteScriptAsync("document.getElementById('btnKetThuc').click();");
            await Task.Delay(2000); // chờ load trang sau khi bấm xác nhận
            //Nếu chưa nhấn OK thì vẫn dừng ở đây, và chưa chạy dòng code ở dưới đâu
            //click vào nút xem hóa đơn
            webhidden.ExecuteScriptAsync("document.querySelector('a.invoice-btn.view-btn.cursor').click();");
            //
            //auto trigger pdf view and auto download
            // chờ 15s để chắc chắn view file thông báo lên
            await Task.Delay(5000);
            //auto trigger pdf view and auto download
            //click vào nút tải hóa đơn
            int X = 1350;//Convert.ToInt32(weblogin.Width * 0.711); tính ngược lại ra 1899.7
            int Y = 140;//Convert.ToInt32(weblogin.Height * 0.139);tính ngược lại ra 1007.2
            webhidden.GetBrowser().GetHost().SendMouseClickEvent(X, Y, MouseButtonType.Left, false, 1, CefEventFlags.None);
            await Task.Delay(150);
            webhidden.GetBrowser().GetHost().SendMouseClickEvent(X, Y, MouseButtonType.Left, true, 1, CefEventFlags.None);
            await Task.Delay(5000);
            Application.Exit();
        }
    }
}
