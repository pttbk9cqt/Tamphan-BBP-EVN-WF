using CefSharp;
using CefSharp.WinForms;
using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tamphan_BBP_EVN_WF.Models;
using Tamphan_BBP_EVN_WF.Services;

namespace Tamphan_BBP_EVN_WF
{
    public partial class frmDownloadFullYear : Form
    {
        private string url = "https://cskh.evnspc.vn/TaiKhoan/DangNhap?previousLink=/TraCuu/HoaDonTienDien";
        private List<string> _arrMaKH = new List<string>();
        private CaptchaHelper captchaHelper;
        private AccountService _accountService;
        private InvoiceService _invoiceService;
        private List<string> _arrayInvoiceID_Downloaded = new List<string>();
        private Dictionary<string, List<string>> _arrDownloadFailed = new Dictionary<string, List<string>>();
        private bool _loginProcessStarted = false;
        private TaskCompletionSource<bool> _downloadCompleted;
        public bool IsCompleted { get; private set; } = false;
        public List<(string maKH, string mucDich)> FailedInvoices { get; private set; } = new List<(string, string)>();
        ////////////////////////////////////////////////////////////////////////////////////////////////
        public frmDownloadFullYear(List<string> arrayMaKH, AccountService accountService)
        {
            InitializeComponent();
            _accountService = accountService;
            _arrMaKH = arrayMaKH;
            this.WindowState = FormWindowState.Maximized;
            InitBrowser();
            captchaHelper = new CaptchaHelper(chromiumdownloadfullyear, "imgCaptcha");
            _invoiceService = new InvoiceService(chromiumdownloadfullyear, _accountService);
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        private void InitBrowser()
        {
            chromiumdownloadfullyear.FrameLoadEnd += Browser_FrameLoadEndAsync;
            chromiumdownloadfullyear.Load(url);
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private async void Browser_FrameLoadEndAsync(object sender, FrameLoadEndEventArgs e)
        {
            if (!e.Frame.IsMain || !e.Url.Contains("DangNhap") || _loginProcessStarted) return;

            _loginProcessStarted = true;

            List<AccountEVN> arrAccount = _accountService.GetAllAccounts(); //sau lệnh này được 1 list 329 phần tử với model account
            Dictionary<string, List<string>> mappGop = _accountService.GetMapAccount();//sau lệnh này được danh sách 7 phần tử gộp bao gồm 6 mã gộp và 1 phần tử null

            for (int i = 0; i < _arrMaKH.Count; i++)            //_arrMaKH là mã khách hàng từ các rows trong datagridview đưa vào
            {
                AccountEVN acc = _accountService.GetAccount(_arrMaKH[i]);  //móc phần tử acc là từng rows dạng model account từ source ra
                //
                await AutoLoginAndDownload(acc);
                //
                await Task.Delay(1000);
                Cef.GetGlobalCookieManager().DeleteCookies("", "");
                await Task.Delay(1000);
                chromiumdownloadfullyear.Load(url);
                await Task.Delay(1500);
            }
            await Task.Delay(500);
            this.Invoke(new Action(() => { this.Close(); }));//xong roi thi close form
        }
        //////////////////////////////////////////////////////////////////////////////////////////////
        private async Task AutoLoginAndDownload(AccountEVN acc, List<string> _arrMaKH = null)
        {
            await FillLoginForm(acc);
            await Task.Delay(400);

            await captchaHelper.AutoFillCaptchaAsync();
            await Task.Delay(800);

            chromiumdownloadfullyear.ExecuteScriptAsync("document.getElementById('btnDangNhap').click();");
            await Task.Delay(2000);

            await RetryLoginIfFailed(acc);

            // ===== LOOP 12 THÁNG =====
            for (int month = 1; month <= 12; month++)
            {
                Console.WriteLine($"Đang xử lý tháng {month}/2025");

                await SelectMonthYear(month, 2025);

                var response = await chromiumdownloadfullyear.EvaluateScriptAsync(@"
                                                                                    Array.from(document.querySelectorAll('a.invoice-btn.view-btn.cursor'))
                                                                                    .map(b => b.getAttribute('onclick'))
                                                                                    .filter(x=>x)
                                                                                ");

                var list_invoiceInWeb = await _invoiceService.GetInvoicesAsync();

                List<object> arrAllOnClick = (List<object>)response.Result; //arrAllOnclick trả về XemHoaDonTienDien('1492538262','1','1','2025');
                List<object> arrOnClick = new List<object>();//arrOnclick trả về XemHoaDonTienDien('1492538262','1','1','2025');

                if (_arrMaKH != null && _arrMaKH.Count > 0)
                {
                    var list_invoiceNeedDownload = list_invoiceInWeb
                        .Where(ite => _arrMaKH.Contains(ite.maKH));

                    foreach (var item in list_invoiceNeedDownload)
                    {
                        arrOnClick.AddRange(arrAllOnClick
                            .Where(ite => ite.ToString().Contains(item.idHoaDon)));
                    }
                }
                else
                {
                    arrOnClick = arrAllOnClick;
                }

                if (arrOnClick.Count == 0)
                {
                    Console.WriteLine($"Tháng {month} không có hóa đơn");
                    continue;
                }

                foreach (var item in arrOnClick)
                {
                    int retry = 0;

                Retry:
                    string onclick = item.ToString();
                    string idHoaDon = onclick.Split('\'')[1];

                    if (_arrayInvoiceID_Downloaded.Contains(idHoaDon))
                        continue;

                    var invoice = list_invoiceInWeb.FirstOrDefault(x => x.idHoaDon == idHoaDon);
                    if (invoice == null) continue;

                    string maKH = invoice.maKH;
                    AccountEVN accInfo = _accountService.GetAccount(maKH);
                    string mucDich = accInfo?.MucDichSuDung ?? "";

                    chromiumdownloadfullyear.ExecuteScriptAsync(onclick);
                    await Task.Delay(3000);
                    await chromiumdownloadfullyear.WaitForInitialLoadAsync();

                    // truyền tháng vào file name
                    string fileName = "";
                    SetDownloadHandler(maKH, month, 2025, out fileName); ;

                    _downloadCompleted = new TaskCompletionSource<bool>();

                    await Task.Delay(3000);

                    Point pointDownload = GetPoinDownloadButton();

                    chromiumdownloadfullyear.GetBrowser().GetHost()
                        .SendMouseClickEvent(pointDownload.X, pointDownload.Y, MouseButtonType.Left, false, 1, CefEventFlags.None);

                    await Task.Delay(100);

                    chromiumdownloadfullyear.GetBrowser().GetHost()
                        .SendMouseClickEvent(pointDownload.X, pointDownload.Y, MouseButtonType.Left, true, 1, CefEventFlags.None);

                    var completed = await Task.WhenAny(_downloadCompleted.Task, Task.Delay(5000));

                    if (completed != _downloadCompleted.Task)
                    {
                        if (retry < 3)
                        {
                            retry++;
                            await Task.Delay(5000);
                            goto Retry;
                        }
                        else if (!FailedInvoices.Any(x => x.maKH == maKH))
                        {
                            FailedInvoices.Add((maKH, mucDich));
                            continue;
                        }
                    }
                    else
                    {
                        _arrayInvoiceID_Downloaded.Add(idHoaDon);
                    }

                    await Task.Delay(1000);
                }

                await Task.Delay(1000);
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private async Task RetryLoginIfFailed(AccountEVN acc)
        {
            for (int i = 0; i < 3; i++)
            {
                if (!chromiumdownloadfullyear.Address.Contains("DangNhap")) { return; } // đã đăng nhập thành công
                await Task.Delay(1000);
                chromiumdownloadfullyear.Reload();
                await Task.Delay(1500);
                await FillLoginForm(acc);
                await Task.Delay(1000);
                await captchaHelper.AutoFillCaptchaAsync();
                await Task.Delay(600);
                chromiumdownloadfullyear.ExecuteScriptAsync("document.getElementById('btnDangNhap').click();");
                await Task.Delay(2000);
            }
            // ===== Nếu chạy hết 3 lần vẫn ở trang đăng nhập =====
            if (chromiumdownloadfullyear.Address.Contains("DangNhap"))
            {
                MessageBox.Show($"Đăng nhập thất bại sau 3 lần.\nBỏ qua mã khách hàng: {acc.MaKH}", "EVN Tool");
                this.Close(); // đóng form
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////
        private async Task FillLoginForm(AccountEVN acc)
        {
            if (string.IsNullOrEmpty(acc.Username))
                acc.Username = acc.MaKH;
            if (string.IsNullOrEmpty(acc.Password))
                acc.Password = "binhphuoc";
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

            chromiumdownloadfullyear.ExecuteScriptAsync(script);
            await Task.Delay(400);
        }
        ////////////phần thêm để chọn tháng//////////////////////////
        private async Task SelectMonthYear(int month, int year)
        {
            await chromiumdownloadfullyear.EvaluateScriptAsync($@"
        (function() 
            {{
            var monthEl = document.getElementById('month');
            var yearEl = document.getElementById('year');

            if (monthEl && yearEl) 
            {{
                monthEl.value = '{month}';
                yearEl.value = '{year}';
                monthEl.dispatchEvent(new Event('change', {{ bubbles: true }}));
                yearEl.dispatchEvent(new Event('change', {{ bubbles: true }}));
            }}
            }})();
            ");

            // delay để JS web xử lý
            await Task.Delay(2000);
            await WaitForInvoiceLoad();
            await Task.Delay(1500);
        }

        private async Task WaitForInvoiceLoad()
        {
            int lastCount = -1;

            for (int i = 0; i < 15; i++)
            {
                var check = await chromiumdownloadfullyear.EvaluateScriptAsync(@" document.querySelectorAll('a.invoice-btn.view-btn.cursor').length ");

                if (check.Success)
                {
                    int current = (int)check.Result;
                    // đợi đến khi dữ liệu ổn định (load xong)
                    if (current == lastCount)
                        return;
                    lastCount = current;
                }

                await Task.Delay(1000);
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////
        /////Hàm set DownloadHandler động với mỗi maKH mới ở maKH = invoice.maKH; để truyền mã khách hàng của từng row cho file download
        private void SetDownloadHandler(string maKH, int month, int year, out string fileName)
        {
            fileName = Path.Combine( Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", BuildPdfName(maKH, month, year));

            var downloadHandler = new BlobPdfDownloadHandler(
                                                            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"),
                                                            () => BuildPdfName(maKH, month, year)
                                                            );

            downloadHandler.PdfDownloaded += OnPdfDownloaded;
            chromiumdownloadfullyear.DownloadHandler = downloadHandler;
        }
        //////////////////////////////////////////////////////////////////////////////////////////////
        string BuildPdfName(string maKH, int month, int year)
        {
            AccountEVN acc = _accountService.GetAccount(maKH);
            string mucDich = acc?.MucDichSuDung ?? "";
            string ky = $"{month:D2}-{year}";
            return $"HoaDon_{ky}_{maKH}_{mucDich}.pdf";
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private void OnPdfDownloaded(string path)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnPdfDownloaded(path)));
                return;
            }
            // báo cho luồng chính biết là đã download xong
            _downloadCompleted?.TrySetResult(true);
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        /// Hàm lấy vị trí nút download
        private Point GetPoinDownloadButton()
        {
            try
            {
                var script = @"
                                (function() {
                                    var el = document.getElementById('pdfFrame');
                                    if (!el) return null;

                                    var rect = el.getBoundingClientRect();
                                    return {
                                        x: rect.right,
                                        y: rect.top
                                    };
                                })();
                                ";
                var response = chromiumdownloadfullyear.EvaluateScriptAsync(script).Result;
                if (response.Success && response.Result != null)
                {
                    dynamic result = response.Result;
                    double x = result.x;
                    double y = result.y;
                    return new Point(Convert.ToInt32(x) - 102, Convert.ToInt32(y) + 28);
                }
                else
                {
                    return new Point(1350, 165); //MessageBox.Show("khong tim thay vi tri nut download, set mac dinh");
                }
            }
            catch (Exception)
            {
                return new Point(1350, 165); //MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace + "\r\nSet mac dinh");
            }
        }
    }
}
