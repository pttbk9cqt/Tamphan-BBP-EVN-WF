using CefSharp;
using CefSharp.WinForms;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tamphan_BBP_EVN_WF.Services;

namespace Tamphan_BBP_EVN_WF
{
    public partial class EVN_NewAccount : Form
    {
        private string _newuser;
        private string _newpassword;
        private CaptchaHelper _captchaHelper;
        public EVN_NewAccount(string newuser, string newpassword)
        {
            _newuser = newuser;
            _newpassword = newpassword;
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            InitBrowser();
            _captchaHelper = new CaptchaHelper(chrome_newaccount, "imgCaptcha1");
        }

        private async void InitBrowser()
        {
            if (Cef.IsInitialized != true)
            {
                CefSettings settings = new CefSettings();
                settings.BrowserSubprocessPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "CefSharp.BrowserSubprocess.exe");
                // USER AGENT CHROME THẬT
                settings.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " + "(KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36";
                Cef.Initialize(settings);
            }

            string url = "https://cskh.evnspc.vn/TaiKhoan/DangNhap?Menu=dangky&ActiveTab=tab3";
            chrome_newaccount.FrameLoadEnd += Browser_FrameLoadEndAsync;
            chrome_newaccount.Load(url);
        }

        private async void Browser_FrameLoadEndAsync(object sender, FrameLoadEndEventArgs e)
        {
            if (!e.Frame.IsMain) return;

            if (string.IsNullOrWhiteSpace(_newpassword))
            { _newpassword = "binhphuoc"; }

            await Task.Delay(1000);
            await chrome_newaccount.EvaluateScriptAsync("document.querySelector('#FinishModalClose .btn-close').click();"); //đóng thông báo popup bật lên
            await Task.Delay(1000);
            //sau đó cuộn chuột xuống 1 lần
            await chrome_newaccount.EvaluateScriptAsync(@"
                                                            (async ()=>{
                                                                for(let i=0;i<1;i++){
                                                                    window.scrollBy(0,400);
                                                                    await new Promise(r=>setTimeout(r,300));
                                                                }
                                                            })();
                                                            ");
            ///cuộn xong rồi thì bắt đầu điền thông tin
            await creaccount_fillinfo();
        }



        private async void btn_creaccount_fillinfo_Click(object sender, EventArgs e)
        {
            await creaccount_fillinfo();
        }

        private async Task creaccount_fillinfo()
        {
            await chrome_newaccount.EvaluateScriptAsync($"document.getElementById('idTenDangNhap').value = '{_newuser}';");
            await Task.Delay(600);
            await chrome_newaccount.EvaluateScriptAsync($"document.getElementById('idMaKhachHang').value = '{_newuser}';");
            await Task.Delay(600);
            await chrome_newaccount.EvaluateScriptAsync($"document.getElementById('idSoDienThoai').value = '0707966066';");
            await Task.Delay(600);
            await chrome_newaccount.EvaluateScriptAsync($"document.getElementById('idMatKhau').value = '{_newpassword}';");
            await Task.Delay(600);
            await chrome_newaccount.EvaluateScriptAsync($"document.getElementById('idNhapLaiMatKhau').value = '{_newpassword}';");
            await Task.Delay(600);
            // captcha
            await _captchaHelper.AutoFillCaptchaAsync();
            await Task.Delay(800);
        }
    }
}
