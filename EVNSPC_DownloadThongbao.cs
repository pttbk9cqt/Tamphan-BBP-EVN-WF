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
        private CaptchaHelper _captchaHelper;
        private EvnInformationInvoiceService _invoiceInforService;
        string kyHoaDon = DateTime.Now.AddMonths(-1).ToString("MM-yyyy");
        private bool _processStarted = false;


        public EVNSPC_DownloadThongbao(string maKH)
        {
            InitializeComponent();
            _maKH = maKH;
            this.WindowState = FormWindowState.Maximized;
            InitBrowser();
            _captchaHelper = new CaptchaHelper(evndownload);
            _invoiceInforService = new EvnInformationInvoiceService(evndownload);
        }

        private void InitBrowser()
        {
            if (Cef.IsInitialized != true)
            {
                CefSettings settings = new CefSettings();
                settings.BrowserSubprocessPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "CefSharp.BrowserSubprocess.exe");
                Cef.Initialize(settings);
            }
            evndownload.FrameLoadEnd += Browser_FrameLoadEndAsync;
            string url = "https://cskh.evnspc.vn/TaiKhoan/DangNhap?previousLink=/TraCuu/HoaDonTienDien";
            MousePositionHelper.Start(this);
            //var downloadHandler = new BlobPdfDownloadHandler(@"C:\Users\pttbk\Downloads", () => BuildPdfName(_maKH));
            var downloadHandler = new BlobPdfDownloadHandler(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),"Downloads"),() => BuildPdfName(_maKH));
            downloadHandler.PdfDownloaded += delegate (string path) { Console.WriteLine("PDF saved: " + path); };
            evndownload.DownloadHandler = downloadHandler;
            evndownload.Load(url);
        }
        private async void Browser_FrameLoadEndAsync(object sender, FrameLoadEndEventArgs e)
        {
            if (!e.Frame.IsMain)
                return;

            if (_processStarted)
                return;

            if (!e.Url.Contains("cskh.evnspc.vn/TaiKhoan/DangNhap"))
                return;

            _processStarted = true;

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
            evndownload.ExecuteScriptAsync(fill_maKH_pass_Script);//tới đây là đã tự điền mã KH và pass
            await Task.Delay(500);
            await _captchaHelper.AutoFillCaptchaAsync();// ĐẾN ĐÂY LÀ ĐÃ ĐIỀN XONG CAPTCHA VÀO TRANG WEB
            await Task.Delay(1200);
            evndownload.ExecuteScriptAsync("document.getElementById('btnDangNhap').click();");
            await Task.Delay(1200); // chờ load trang sau khi đăng nhập

            //nếu bị lỗi captcha hoặc đăng nhập không thành công thì thử lại
            for (int i = 0; i <= 3; i++) // thử 3 lần
                if (evndownload.Address.Contains("cskh.evnspc.vn/TaiKhoan/DangNhap"))
                {
                    evndownload.Reload();
                    await Task.Delay(1000); // chờ load lại trang
                    evndownload.ExecuteScriptAsync(fill_maKH_pass_Script);
                    await _captchaHelper.AutoFillCaptchaAsync();
                    await Task.Delay(700);
                    evndownload.ExecuteScriptAsync("document.getElementById('btnDangNhap').click();");
                    await Task.Delay(2000);
                }
                else
                {
                    break;
                }

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

        string BuildPdfName(string maKH)
        {
            ExcelAccountEVNService service = new ExcelAccountEVNService();
            AccountEVN acc = service.GetAccount(maKH);
            return acc.MucDichSuDung + "_Thông báo tiền điện tháng " + kyHoaDon + "_" + maKH + ".pdf";
        }

        private async void btn_exporttable_Click(object sender, EventArgs e)
        {
            var list = await _invoiceInforService.ExportInforAsync();

            if (list.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu hóa đơn");
                return;
            }

            string path = _invoiceInforService.ExportToExcel(list, _maKH);
            MessageBox.Show($"Xuất Excel thành công:\n{path}");
        }
    }
}
