using CefSharp;
using CefSharp.WinForms;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
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
        public EVN_WEB_LOGIN(string maKH, ExcelAccountEVNService ExcelAccountEVN)
        {
            InitializeComponent();
            _maKH = maKH;
            excelService = ExcelAccountEVN;
            this.WindowState = FormWindowState.Maximized;
            InitBrowser();
            _captchaHelper = new CaptchaHelper(weblogin, "imgCaptcha");
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////
        private void InitBrowser()
        {
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
            for (int i = 0; i <= 3; i++)
            {
                // Nếu login thành công thì thoát
                if (!weblogin.Address.Contains("DangNhap"))
                {
                    _LoginSuccess = true;
                    return;
                }

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
        }})();";

                // điền user pass
                weblogin.ExecuteScriptAsync(loginScript);
                await Task.Delay(500);

                // captcha
                await _captchaHelper.AutoFillCaptchaAsync();
                await Task.Delay(700);

                // click login
                weblogin.ExecuteScriptAsync("document.getElementById('btnDangNhap').click();");
                await Task.Delay(2000);

                // nếu vẫn ở trang login thì reload để thử lại
                if (weblogin.Address.Contains("DangNhap"))
                {
                    weblogin.Reload();
                    await Task.Delay(1500);
                }
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
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