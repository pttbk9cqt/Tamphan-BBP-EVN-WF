using CefSharp;
using CefSharp.WinForms;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tamphan_BBP_EVN_WF.Models;
using Tamphan_BBP_EVN_WF.Services;

namespace Tamphan_BBP_EVN_WF
{
    public partial class EVNSPC_WEB_LOGIN : Form
    {
        private string _maKH;
        private CaptchaHelper _captchaHelper;
        private bool _DownloadBtnClicked = false;
        private bool _LoginSuccess = false;

        public EVNSPC_WEB_LOGIN(string maKH)
        {
            InitializeComponent();
            _maKH = maKH;
            this.WindowState = FormWindowState.Maximized;
            InitBrowser();
            _captchaHelper = new CaptchaHelper(weblogin); // tạo helper

        }

        private void InitBrowser()
        {
            if (Cef.IsInitialized != true)
            {
                CefSettings settings = new CefSettings();
                settings.BrowserSubprocessPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "CefSharp.BrowserSubprocess.exe");
                Cef.Initialize(settings);
            }
            weblogin.FrameLoadEnd += Browser_FrameLoadEndAsync;
            string url = "https://cskh.evnspc.vn/TaiKhoan/DangNhap?previousLink=/TraCuu/HoaDonTienDien";
            MousePositionHelper.Start(this);
            weblogin.Load(url);
        }
        private async void Browser_FrameLoadEndAsync(object sender, FrameLoadEndEventArgs e)
        {
            if (!e.Frame.IsMain)
                return;
            if (!e.Url.Contains("cskh.evnspc.vn/TaiKhoan/DangNhap"))
                return;
            if (_LoginSuccess)
                return;
            if (_DownloadBtnClicked)
                return;

            ExcelAccountEVNService service = new ExcelAccountEVNService();
            AccountEVN acc = service.GetAccount(_maKH);

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
            weblogin.ExecuteScriptAsync(fill_maKH_pass_Script);//tới đây là đã tự điền mã KH và pass
            await Task.Delay(500);
            await _captchaHelper.AutoFillCaptchaAsync();// ĐẾN ĐÂY LÀ ĐÃ ĐIỀN XONG CAPTCHA VÀO TRANG WEB
            await Task.Delay(8000);
            weblogin.ExecuteScriptAsync("document.getElementById('btnDangNhap').click();");// Tiếp đó là bấm nút đăng nhập
            await Task.Delay(1200); // chờ load trang sau khi đăng nhập
            //nếu bị lỗi captcha hoặc đăng nhập không thành công thì thử lại
            for (int i = 0; i <= 3; i++) // thử 3 lần
                if (weblogin.Address.Contains("cskh.evnspc.vn/TaiKhoan/DangNhap"))
                {
                    weblogin.Reload();
                    await Task.Delay(1000); // chờ load lại trang
                    weblogin.ExecuteScriptAsync(fill_maKH_pass_Script);
                    await _captchaHelper.AutoFillCaptchaAsync();
                    await Task.Delay(700);
                    weblogin.ExecuteScriptAsync("document.getElementById('btnDangNhap').click();");
                    await Task.Delay(2000);
                }
                else
                {
                    _LoginSuccess = true;
                    break; // đăng nhập thành công, thoát vòng lặp
                }
        }
    }
}
