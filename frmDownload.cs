using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tamphan_BBP_EVN_WF.Models;
using Tamphan_BBP_EVN_WF.Services;

namespace Tamphan_BBP_EVN_WF
{
    public partial class frmDownload : Form
    {
        private string _maKH;
        private AccountEVN _account;
        private CaptchaHelper captchaHelper;
        private AccountService _accountService;
        private InvoiceService _invoiceService;
        string kyHoaDon = DateTime.Now.AddMonths(-1).ToString("MM-yyyy");
        private bool _loginProcessStarted = false;
        private TaskCompletionSource<bool> _downloadCompleted;
        public bool IsCompleted { get; private set; } = false;
        private bool _downloadSingleOnly;
        private HashSet<string> _allowedMaKH;
        public List<(string maKH, string mucDich)> FailedInvoices { get; private set; } = new List<(string, string)>();
        ////////////////////////////////////////////////////////////////////////////////////////////////
        public frmDownload(string maKH, AccountService accountService, bool downloadSingleOnly, List<string> allowedMaKH = null)
        {
            InitializeComponent();
            _maKH = maKH;
            _accountService = accountService;
            _downloadSingleOnly = downloadSingleOnly;
            _account = _accountService.GetAccount(_maKH);
            _allowedMaKH = allowedMaKH != null ? new HashSet<string>(allowedMaKH) : null;
            this.WindowState = FormWindowState.Maximized;
            InitBrowser();
            captchaHelper = new CaptchaHelper(chromiumdownload, "imgCaptcha");
            _invoiceService = new InvoiceService(chromiumdownload);
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private void InitBrowser()
        {
            chromiumdownload.FrameLoadEnd += Browser_FrameLoadEndAsync;
            string url = "https://cskh.evnspc.vn/TaiKhoan/DangNhap?previousLink=/TraCuu/HoaDonTienDien";
            MousePositionHelper.Start(this);
            //var downloadHandler = new BlobPdfDownloadHandler(@"C:\Users\pttbk\Downloads", () => BuildPdfName(_maKH));
            //downloadHandler.PdfDownloaded += delegate (string path) {Console.WriteLine("PDF saved: " + path);}; 
            //thay bằng 2 dòng liền tiếp ở dưới
            var downloadHandler = new BlobPdfDownloadHandler(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"), () => BuildPdfName(_maKH));
            downloadHandler.PdfDownloaded += OnPdfDownloaded;
            chromiumdownload.DownloadHandler = downloadHandler;
            chromiumdownload.Load(url);
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private async void Browser_FrameLoadEndAsync(object sender, FrameLoadEndEventArgs e)
        {
            if (!e.Frame.IsMain) return;

            if (!e.Url.Contains("DangNhap")) return;

            if (_loginProcessStarted) return;

            _loginProcessStarted = true;

            AccountEVN acc = _accountService.GetAccount(_maKH);
            if (acc == null)
            {
                MessageBox.Show("Không tìm thấy account");
                return;
            }

            await AutoLogin(acc);
        }
        //////////////////////////////////////////////////////////////////////////////////////////////
        private async Task AutoLogin(AccountEVN acc)
        {
            // điền user pass
            await FillLoginForm(acc);
            await Task.Delay(400);
            // captcha
            await captchaHelper.AutoFillCaptchaAsync();
            await Task.Delay(800);
            // click login
            chromiumdownload.ExecuteScriptAsync("document.getElementById('btnDangNhap').click();");
            await Task.Delay(2000);
            await RetryLoginIfFailed(acc);
            //tới đây là đã đăng nhập thành công rồi
            var response = await chromiumdownload.EvaluateScriptAsync(@"
                                                                         Array.from(document.querySelectorAll('a.invoice-btn.view-btn.cursor'))
                                                                        .map(b => b.getAttribute('onclick'))
                                                                        .filter(x=>x)
                                                                        ");
            //lấy danh sách onclick của tất cả nút xem thông báo/hóa đơn, sau đó sẽ click từng cái một để mở file pdf tương ứng rồi mới click tiếp nút download trong pdf đó
            var list = await _invoiceService.GetInvoicesAsync();
            bool found = false;
            foreach (var item in (List<object>)response.Result)
            {
                string onclick = item.ToString(); //lúc này nó sẽ ra một mảng là rows đầu tiên trong table trên web
                string idHoaDon = onclick.Split('\'')[1]; // Lấy idHoaDon từ chuỗi onclick, ở vị trí thứ 2 (index 1) sau khi split bằng dấu nháy đơn

                // tìm dòng tương ứng
                var invoice = list.FirstOrDefault(x => x.idHoaDon == idHoaDon);

                if (invoice == null)
                {
                    continue;
                }

                string maKH = invoice.maKH;

                AccountEVN accInfo = _accountService.GetAccount(maKH);
                string mucDich = accInfo?.MucDichSuDung ?? "";

                // CHẶN DOWNLOAD NGOÀI DANH SÁCH
                if (_allowedMaKH != null && !_allowedMaKH.Contains(invoice.maKH))
                    continue;

                // CHẾ ĐỘ CHỈ 1 HÓA ĐƠN
                if (_downloadSingleOnly && invoice.maKH != _maKH)
                    continue;
                found = true; //đánh dấu đã tìm thấy

                chromiumdownload.ExecuteScriptAsync(onclick);//click vào nút xem thông báo/hóa đơn để mở file pdf
                await Task.Delay(3000);

                // set handler đúng theo từng hóa đơn
                SetDownloadHandler(maKH);

                // chuẩn bị chờ download
                _downloadCompleted = new TaskCompletionSource<bool>();

                await Task.Delay(1000); // đợi nút download render
                // click vào nút download
                int X = 1350;//Convert.ToInt32(weblogin.Width * 0.711); tính ngược lại ra 1899.7; thì ở setup là 1900
                int Y = 140;//Convert.ToInt32(weblogin.Height * 0.139);tính ngược lại ra 1007.2; thì ở setup là 1000
                //int X = 1365;//ứng với setup 1920
                //int Y = 165;//ứng với setup 1080
                chromiumdownload.GetBrowser().GetHost().SendMouseClickEvent(X, Y, MouseButtonType.Left, false, 1, CefEventFlags.None);
                await Task.Delay(100);
                chromiumdownload.GetBrowser().GetHost().SendMouseClickEvent(X, Y, MouseButtonType.Left, true, 1, CefEventFlags.None);
                // chờ download xong
                var completed = await Task.WhenAny(_downloadCompleted.Task, Task.Delay(10000));

                if (completed != _downloadCompleted.Task)
                {
                    Console.WriteLine("Timeout download → skip");

                    if (!FailedInvoices.Any(x => x.maKH == maKH))
                        FailedInvoices.Add((maKH, mucDich));

                    continue;
                }
                await Task.Delay(500); // buffer nhỏ cho chắc
                // nếu là mode 1 hóa đơn → dừng luôn
                if (_downloadSingleOnly)
                    break;
            }

            ///Sau khi download xong hết tất cả hóa đơn kể cả mã gộp, đóng form
            if (found)
            {
                IsCompleted = true;
            }
            else
            {
                IsCompleted = false;

                this.Invoke(new Action(() =>
                {
                    MessageBox.Show($"Không tìm thấy hóa đơn cho mã KH: {_maKH}");
                }));
            }

            this.Invoke(new Action(() =>
            {
                this.Close();
            }));
        }
        
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private async Task RetryLoginIfFailed(AccountEVN acc)
        {
            for (int i = 0; i < 3; i++)
            {
                if (!chromiumdownload.Address.Contains("DangNhap"))
                {
                    // đã đăng nhập thành công
                    return;
                }

                await Task.Delay(1000);
                chromiumdownload.Reload();
                await Task.Delay(1500);
                await FillLoginForm(acc);
                await Task.Delay(1000);
                await captchaHelper.AutoFillCaptchaAsync();
                await Task.Delay(600);
                chromiumdownload.ExecuteScriptAsync("document.getElementById('btnDangNhap').click();");
                await Task.Delay(2000);
            }
            // ===== Nếu chạy hết 3 lần vẫn ở trang đăng nhập =====
            if (chromiumdownload.Address.Contains("DangNhap"))
            {
                MessageBox.Show($"Đăng nhập thất bại sau 3 lần.\nBỏ qua mã khách hàng: {acc.MaKH}", "EVN Tool");
                this.Close(); // đóng form
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////
        private async Task FillLoginForm(AccountEVN acc)
        {
            string script = $@"
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

            chromiumdownload.ExecuteScriptAsync(script);
            await Task.Delay(400);
        }
        //////////////////////////////////////////////////////////////////////////////////////////////
        /////Hàm set DownloadHandler động với mỗi maKH mới ở maKH = invoice.maKH;
        private void SetDownloadHandler(string maKH)
        {
            var downloadHandler = new BlobPdfDownloadHandler(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"),
                () => BuildPdfName(maKH) // dùng maKH mới
            );

            downloadHandler.PdfDownloaded += OnPdfDownloaded;

            chromiumdownload.DownloadHandler = downloadHandler;
        }
        //////////////////////////////////////////////////////////////////////////////////////////////
        string BuildPdfName(string maKH)
        {
            AccountEVN acc = _accountService.GetAccount(maKH);
            string mucDich = acc?.MucDichSuDung ?? "";

            if (DateTime.Today.Day <= 10)
            {
                return "Thông báo điện kỳ " + kyHoaDon + "_" + maKH + "_" + acc.MucDichSuDung + ".pdf";
            }
            else
            {
                return "Hóa đơn điện kỳ " + kyHoaDon + "_" + maKH + "_" + acc.MucDichSuDung + ".pdf";
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private void OnPdfDownloaded(string path)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnPdfDownloaded(path)));
                return;
            }

            Console.WriteLine("PDF saved: " + path);

            // báo cho luồng chính biết là đã download xong
            _downloadCompleted?.TrySetResult(true);
        }
 
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private async void btnExporttable_Click(object sender, EventArgs e)
        {
            var list = await _invoiceService.GetInvoicesAsync();

            if (list.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu hóa đơn");
                return;
            }

            string path = _invoiceService.ExportInvoiceToExcel(list, _maKH);
            MessageBox.Show($"Xuất Excel thành công:\n{path}");
        }
    }
}