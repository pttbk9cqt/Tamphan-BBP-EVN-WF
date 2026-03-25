using CefSharp;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tamphan_BBP_EVN_WF.Models;
using Tamphan_BBP_EVN_WF.Services;

namespace Tamphan_BBP_EVN_WF
{
    public partial class frmEVNLogin : Form
    {
        private string _maKH;
        private CaptchaHelper _captchaHelper;
        private bool _LoginSuccess;
        private AccountService _accountService;
        ////////////////////////////////////////////////////////////////////////////////////////////////
        public frmEVNLogin(string maKH, AccountService accountService)
        {
            InitializeComponent();
            _maKH = maKH;
            _accountService = accountService;
            this.WindowState = FormWindowState.Maximized;
            InitBrowser();
            _captchaHelper = new CaptchaHelper(chromiumlogin, "imgCaptcha");
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private void InitBrowser()
        {
            chromiumlogin.FrameLoadEnd += Browser_FrameLoadEndAsync;
            string url = "https://cskh.evnspc.vn/TaiKhoan/DangNhap?previousLink=/TraCuu/HoaDonTienDien";
            chromiumlogin.Load(url);
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private async void Browser_FrameLoadEndAsync(object sender, FrameLoadEndEventArgs e)
        {
            if (!e.Frame.IsMain || !e.Url.Contains("DangNhap") || _LoginSuccess) return;

            AccountEVN acc = _accountService.GetAccount(_maKH);

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
                if (!chromiumlogin.Address.Contains("DangNhap"))
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
                chromiumlogin.ExecuteScriptAsync(loginScript);
                await Task.Delay(2000);
                // captcha
                await _captchaHelper.AutoFillCaptchaAsync();
                await Task.Delay(1000);

                // click login
                chromiumlogin.ExecuteScriptAsync("document.getElementById('btnDangNhap').click();");
                await Task.Delay(2000);

                // nếu vẫn ở trang login thì reload để thử lại
                if (chromiumlogin.Address.Contains("DangNhap"))
                {
                    chromiumlogin.Reload();
                    await Task.Delay(1500);
                }
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private async void btn_changepassword_Click(object sender, EventArgs e)
        {
            await chromiumlogin.EvaluateScriptAsync(@"document.querySelector('a[href=""\/TaiKhoan\/ThongTinTaiKhoan""]').click();");
            await Task.Delay(600);
            await chromiumlogin.EvaluateScriptAsync(@"document.querySelector('a[href=""/TaiKhoan/ThayDoiMatKhau""]').click();");
            await Task.Delay(600);
            await chromiumlogin.EvaluateScriptAsync("document.getElementById('old-password').value = '12345678';");
            await Task.Delay(600);
            await chromiumlogin.EvaluateScriptAsync("document.getElementById('new-password').value = 'binhphuoc';");
            await Task.Delay(6000);
            await chromiumlogin.EvaluateScriptAsync("document.getElementById('confirm-password').value = 'binhphuoc';");
            await Task.Delay(400);
            await chromiumlogin.EvaluateScriptAsync("document.querySelector('input[name=\"update-btn\"]').click();");
            await Task.Delay(400);
        }
    }
}