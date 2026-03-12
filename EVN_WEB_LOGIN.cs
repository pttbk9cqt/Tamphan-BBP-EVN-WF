using CefSharp;
using CefSharp.WinForms;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tamphan_BBP_EVN_WF.Models;
using Tamphan_BBP_EVN_WF.Services;

namespace Tamphan_BBP_EVN_WF
{
    public partial class EVN_WEB_LOGIN : Form
    {
        private string _maKH;
        private CaptchaHelper _captchaHelper;
        private bool _LoginSuccess = false;
        private ExcelAccountEVNService excelService;
        ////////////////////////////////////////////////////////////////////////////////////////////////
        public EVN_WEB_LOGIN(string maKH, ExcelAccountEVNService service)
        {
            InitializeComponent();
            _maKH = maKH;
            excelService = new ExcelAccountEVNService();
            this.WindowState = FormWindowState.Maximized;
            InitBrowser();
            _captchaHelper = new CaptchaHelper(weblogin, "imgCaptcha");
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private void InitBrowser()
        {
            if (Cef.IsInitialized != true)
            {
                CefSettings settings = new CefSettings();
                settings.BrowserSubprocessPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),"CefSharp.BrowserSubprocess.exe");
                // USER AGENT CHROME THẬT
                settings.UserAgent ="Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " + "(KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36";
                Cef.Initialize(settings);
            }

            weblogin.FrameLoadEnd += Browser_FrameLoadEndAsync;
            string url = "https://cskh.evnspc.vn/TaiKhoan/DangNhap?previousLink=/TraCuu/HoaDonTienDien";
            weblogin.Load(url);
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private async void Browser_FrameLoadEndAsync(object sender, FrameLoadEndEventArgs e)
        {
            if (!e.Frame.IsMain) return;

            if (!e.Url.Contains("DangNhap")) return;

            if (_LoginSuccess) return;

            AccountEVN acc = excelService.GetAccount(_maKH);

            if (acc == null) 
                return;

            await AutoLogin(acc);
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private async Task AutoLogin(AccountEVN acc)
        {
            string loginScript = $@"
            (function()
            {{
                let userInput = document.querySelector('input[placeholder=""TÊN ĐĂNG NHẬP""]');
                let passInput = document.querySelector('input[placeholder=""MẬT KHẨU""]');

                if(userInput && passInput)
                {{
                    userInput.value = '{acc.Username}';
                    passInput.value = '{acc.Password}';

                    userInput.dispatchEvent(new Event('input', {{bubbles:true}}));
                    passInput.dispatchEvent(new Event('input', {{bubbles:true}}));
                }}
            }})();
            ";

            // điền user pass
            weblogin.ExecuteScriptAsync(loginScript);
            await Task.Delay(400);
            // captcha
            await _captchaHelper.AutoFillCaptchaAsync();
            await Task.Delay(800);
            // click login
            weblogin.ExecuteScriptAsync("document.getElementById('btnDangNhap').click();");
            await Task.Delay(2000);
            await RetryLoginIfFailed(acc);
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private async Task RetryLoginIfFailed(AccountEVN acc)
        {
            for (int i = 0; i < 3; i++)
            {
                if (!weblogin.Address.Contains("DangNhap"))
                {
                    _LoginSuccess = true;
                    return;
                }

                await Task.Delay(1000);
                weblogin.Reload();
                await Task.Delay(1500);
                string retryScript = $@"
                (function()
                {{
                    let userInput = document.querySelector('input[placeholder=""TÊN ĐĂNG NHẬP""]');
                    let passInput = document.querySelector('input[placeholder=""MẬT KHẨU""]');

                    if(userInput && passInput)
                    {{
                        userInput.value = '{acc.Username}';
                        passInput.value = '{acc.Password}';
                    }}
                }})();
                ";

                weblogin.ExecuteScriptAsync(retryScript);
                await Task.Delay(600);
                await _captchaHelper.AutoFillCaptchaAsync();
                await Task.Delay(600);
                weblogin.ExecuteScriptAsync("document.getElementById('btnDangNhap').click();");
                await Task.Delay(2000);
            }
        }

        private async void btn_changepassword_Click(object sender, EventArgs e)
        {
            await weblogin.EvaluateScriptAsync(@"document.querySelector('a[href=""\/TaiKhoan\/ThongTinTaiKhoan""]').click();");
            await Task.Delay(600);
            await weblogin.EvaluateScriptAsync(@"document.querySelector('a[href=""/TaiKhoan/ThayDoiMatKhau""]').click();");
            await Task.Delay(600);
            await weblogin.EvaluateScriptAsync("document.getElementById('old-password').value = '12345678';");
            await Task.Delay(600);
            await weblogin.EvaluateScriptAsync("document.getElementById('new-password').value = 'binhphuoc';");
            await Task.Delay(6000);
            await weblogin.EvaluateScriptAsync("document.getElementById('confirm-password').value = 'binhphuoc';");
            await Task.Delay(400);
            await weblogin.EvaluateScriptAsync("document.querySelector('input[name=\"update-btn\"]').click();");
            await Task.Delay(400);
        }

    }
}