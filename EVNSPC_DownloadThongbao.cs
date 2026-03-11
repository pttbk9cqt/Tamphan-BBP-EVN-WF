using CefSharp;
using CefSharp.WinForms;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tamphan_BBP_EVN_WF.Models;
using Tamphan_BBP_EVN_WF.Services;

namespace Tamphan_BBP_EVN_WF
{
    public partial class EVNSPC_DownloadThongbao : Form
    {
        private string _maKH;
        private CaptchaHelper captchaHelper;
        private EvnInformationInvoiceService invoiceInforService;
        string kyHoaDon = DateTime.Now.AddMonths(-1).ToString("MM-yyyy");
        private bool processStarted = false;
        private ExcelAccountEVNService excelService;


        public EVNSPC_DownloadThongbao(string maKH, string username)
        {
            InitializeComponent();
            _maKH = maKH;
            excelService = new ExcelAccountEVNService();
            this.WindowState = FormWindowState.Maximized;
            InitBrowser();
            captchaHelper = new CaptchaHelper(evndownload);
            invoiceInforService = new EvnInformationInvoiceService(evndownload);
        }

        private void InitBrowser()
        {
            if (Cef.IsInitialized != true)
            {
                CefSettings settings = new CefSettings();
                settings.BrowserSubprocessPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "CefSharp.BrowserSubprocess.exe");
                // USER AGENT CHROME THẬT
                settings.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " + "(KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36";
                Cef.Initialize(settings);
            }
            evndownload.FrameLoadEnd += Browser_FrameLoadEndAsync;
            string url = "https://cskh.evnspc.vn/TaiKhoan/DangNhap?previousLink=/TraCuu/HoaDonTienDien";
            MousePositionHelper.Start(this);
            //var downloadHandler = new BlobPdfDownloadHandler(@"C:\Users\pttbk\Downloads", () => BuildPdfName(_maKH));
            var downloadHandler = new BlobPdfDownloadHandler(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),"Downloads"),() => BuildPdfName(_maKH));
            //downloadHandler.PdfDownloaded += delegate (string path) {Console.WriteLine("PDF saved: " + path);}; thay thế bằng 01 dòng code ngay phía bên dưới downloadHandler.PdfDownloaded += OnPdfDownloaded; và chương trình hàm OnPdfDownloaded để sau khi tải xong sẽ tự động đóng form
            downloadHandler.PdfDownloaded += OnPdfDownloaded;
            evndownload.DownloadHandler = downloadHandler;
            evndownload.Load(url);
        }
        private async void Browser_FrameLoadEndAsync(object sender, FrameLoadEndEventArgs e)
        {
            if (!e.Frame.IsMain) return;

            if (!e.Url.Contains("DangNhap")) return;

            if (processStarted) return;


            processStarted = true;

            AccountEVN acc = excelService.GetAccount(_maKH);

            if (acc == null)
                return;
            await AutoLogin(acc);
        }


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
            evndownload.ExecuteScriptAsync(loginScript);
            await Task.Delay(400);
            // captcha
            await captchaHelper.AutoFillCaptchaAsync();
            await Task.Delay(800);
            // click login
            evndownload.ExecuteScriptAsync("document.getElementById('btnDangNhap').click();");
            await Task.Delay(2000);
            await RetryLoginIfFailed(acc);

            //tới đây là đã đăng nhập thành công rồi, click vào nút view thông báo/hóa đơn (nếu có thông báo thì vẫn nút đó, nếu có hóa đơn rồi thì vẫn nút tên đó không đổi)
            evndownload.ExecuteScriptAsync("document.querySelector('a.invoice-btn.view-btn.cursor').click();");
            await Task.Delay(5000);// chờ 5s để chắc chắn view file thông báo lên
            //click vào nút tải hóa đơn
            int X = 1350;//Convert.ToInt32(weblogin.Width * 0.711); tính ngược lại ra 1899.7; thì ở setup là 1900
            int Y = 140;//Convert.ToInt32(weblogin.Height * 0.139);tính ngược lại ra 1007.2; thì ở setup là 1000
            //int X = 1365;//ứng với setup 1920
            //int Y = 165;//ứng với setup 1080
            //3 dòng dưới đây là giả lập click chuột tại tọa độ X Y
            evndownload.GetBrowser().GetHost().SendMouseClickEvent(X, Y, MouseButtonType.Left, false, 1, CefEventFlags.None);
            await Task.Delay(150);
            evndownload.GetBrowser().GetHost().SendMouseClickEvent(X, Y, MouseButtonType.Left, true, 1, CefEventFlags.None);
            //Application.Exit();
        }

        private async Task RetryLoginIfFailed(AccountEVN acc)
        {
            for (int i = 0; i < 3; i++)
            {
                if (!evndownload.Address.Contains("DangNhap"))
                {
                    return;
                }

                await Task.Delay(1000);
                evndownload.Reload();
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

                evndownload.ExecuteScriptAsync(retryScript);
                await Task.Delay(400);
                await captchaHelper.AutoFillCaptchaAsync();
                await Task.Delay(600);
                evndownload.ExecuteScriptAsync("document.getElementById('btnDangNhap').click();");
                await Task.Delay(2000);
            }
        }


        string BuildPdfName(string maKH)
        {
            AccountEVN acc = excelService.GetAccount(maKH);
            return acc.MucDichSuDung + "_Thông báo tiền điện tháng " + kyHoaDon + "_" + maKH + ".pdf";
        }
        private void OnPdfDownloaded(string path)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnPdfDownloaded(path)));
                return;
            }

            Console.WriteLine("PDF saved: " + path);

            // delay nhỏ để chắc chắn file rename xong
            Task.Delay(500).ContinueWith(t =>
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        this.Close();
                    }));
                }
                else
                {
                    this.Close();
                }
            });
        }

        private async void btn_exporttable_Click(object sender, EventArgs e)
        {
            var list = await invoiceInforService.ExportInforAsync();

            if (list.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu hóa đơn");
                return;
            }

            string path = invoiceInforService.ExportToExcel(list, _maKH);
            MessageBox.Show($"Xuất Excel thành công:\n{path}");
        }
    }
}