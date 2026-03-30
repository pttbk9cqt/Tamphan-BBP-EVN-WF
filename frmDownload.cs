using CefSharp;
using CefSharp.WinForms;
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
    public partial class frmDownload : Form
    {
        string _mode = "";
        private string url = "https://cskh.evnspc.vn/TaiKhoan/DangNhap?previousLink=/TraCuu/HoaDonTienDien";
        private string _maKH;
        private List<string> _arrMaKH = new List<string>();
        private AccountEVN _account;
        private CaptchaHelper captchaHelper;
        private AccountService _accountService;
        private InvoiceService _invoiceService;
        private List<string> _arrayInvoiceID_Downloaded = new List<string>();
        private Dictionary<string, List<string>> _arrDownloadFailed = new Dictionary<string, List<string>>();
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
            _mode = "single";
        }
        public frmDownload(List<string> arrayMaKH, AccountService accountService)
        {
            InitializeComponent();
            _accountService = accountService;
            _downloadSingleOnly = false;
            _arrMaKH = arrayMaKH;
            this.WindowState = FormWindowState.Maximized;
            InitBrowser();
            captchaHelper = new CaptchaHelper(chromiumdownload, "imgCaptcha");
            _invoiceService = new InvoiceService(chromiumdownload);
            _mode = "all";
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        private void InitBrowser()
        {
            chromiumdownload.FrameLoadEnd += Browser_FrameLoadEndAsync;
            chromiumdownload.Load(url);
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private async void Browser_FrameLoadEndAsync(object sender, FrameLoadEndEventArgs e)
        {
            if (!e.Frame.IsMain || !e.Url.Contains("DangNhap") || _loginProcessStarted) return;

            _loginProcessStarted = true;

            List<AccountEVN> arrAccount = _accountService.GetAllAccounts();
            Dictionary<string, List<string>> mappGop = _accountService.GetMapAccount();
            List<string> arrAllMaKH_khong_gop = new List<string>();
            List<string> arrAllMaKH_gop = new List<string>();

            //
            if (mappGop.ContainsKey("no"))
            {
                arrAllMaKH_khong_gop = mappGop["no"];
            }
            //
            arrAllMaKH_gop = mappGop.Keys.ToList();
            arrAllMaKH_gop.Remove("no");
            //
            if (_mode == "all")
            {
                Dictionary<string, List<string>> mappingMaGop_maKH_need_download = new Dictionary<string, List<string>>();
                //
                List<string> arrMaMK_khong_gop = arrAllMaKH_khong_gop.Where(ite => _arrMaKH.Contains(ite)).ToList();
                List<string> arrMaKH_of_gop = _arrMaKH.Where(ite => !arrMaMK_khong_gop.Contains(ite)).ToList();
                foreach (string item in arrMaKH_of_gop)
                {
                    foreach (string d in arrAllMaKH_gop)
                    {
                        List<string> arrOfDic = mappGop[d];
                        if (arrOfDic.Contains(item))
                        {
                            if (!mappingMaGop_maKH_need_download.ContainsKey(d))
                            {
                                mappingMaGop_maKH_need_download.Add(d, new List<string>() { item });
                            }
                            else
                            {
                                List<string> val = mappingMaGop_maKH_need_download[d];
                                val.Add(item);
                                mappingMaGop_maKH_need_download[d] = val;
                            }
                        }
                    }
                }
                //
                //xu ly khong gop truoc
                for (int i = 0; i < arrMaMK_khong_gop.Count; i++)
                {
                    AccountEVN acc = _accountService.GetAccount(arrMaMK_khong_gop[i]);
                    //
                    await AutoLoginAndDownload(acc);
                    //
                    await Task.Delay(1000);
                    Cef.GetGlobalCookieManager().DeleteCookies("", "");
                    await Task.Delay(1000);
                    chromiumdownload.Load(url);
                    await Task.Delay(1500);
                }
                //
                //xu ly gop
                foreach (var item in mappingMaGop_maKH_need_download)
                {
                    AccountEVN acc = _accountService.GetAccount(item.Key);
                    //
                    await AutoLoginAndDownload(acc, item.Value);
                    //
                    await Task.Delay(1000);
                    Cef.GetGlobalCookieManager().DeleteCookies("", "");
                    chromiumdownload.Load(url);
                    await Task.Delay(1500);
                }
                await Task.Delay(500);
                this.Invoke(new Action(() => { this.Close(); }));//xong roi thi close form
            }
            else
            {
                //xu ly rieng tung cai nhap vao
                AccountEVN accLogin = _accountService.GetAccount(_maKH);
                //
                if (_downloadSingleOnly)
                {
                    await AutoLoginAndDownload(accLogin, new List<string>() { accLogin.MaKH });
                    await Task.Delay(500);
                    this.Invoke(new Action(() => { this.Close(); }));//xong roi thi close form
                }
                else
                {
                    await AutoLoginAndDownload(accLogin);
                    //
                    await Task.Delay(500);
                    this.Invoke(new Action(() => { this.Close(); }));//xong roi thi close form
                }
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////
        private async Task AutoLoginAndDownload(AccountEVN acc, List<string> arrMaKH_of_gop = null)
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
            var list_invoiceInWeb = await _invoiceService.GetInvoicesAsync();
            List<object> arrAllOnClick = (List<object>)response.Result;
            List<object> arrOnClick = new List<object>();
            if (arrMaKH_of_gop != null && arrMaKH_of_gop.Count > 0)
            {
                var list_invoiceNeedDownload = list_invoiceInWeb.Where(ite => arrMaKH_of_gop.Contains(ite.maKH));
                foreach (var item in list_invoiceNeedDownload)
                {
                    arrOnClick.AddRange(arrAllOnClick.Where(ite => ite.ToString().Contains(item.idHoaDon)));
                }
            }
            else
            {
                arrOnClick = arrAllOnClick;
            }
            //
            foreach (var item in arrOnClick)
            {
                int retry = 0;
            Retry:
                string onclick = item.ToString();
                string idHoaDon = onclick.Split('\'')[1]; // Lấy idHoaDon từ chuỗi onclick, ở vị trí thứ 2 (index 1) sau khi split bằng dấu nháy đơn
                // tìm dòng tương ứng
                //idhoadon da tai roi thi continue.
                if (_arrayInvoiceID_Downloaded.Contains(idHoaDon))
                    continue;
                //
                var invoice = list_invoiceInWeb.FirstOrDefault(x => x.idHoaDon == idHoaDon);
                //
                if (invoice == null)
                    continue;
                //
                string maKH = invoice.maKH;
                //
                AccountEVN accInfo = _accountService.GetAccount(maKH);
                string mucDich = accInfo?.MucDichSuDung ?? "";

                chromiumdownload.ExecuteScriptAsync(onclick);//click vào nút xem thông báo/hóa đơn để mở file pdf
                await Task.Delay(3000);
                await chromiumdownload.WaitForInitialLoadAsync();
                // set handler đúng theo từng hóa đơn
                string fileName = "";
                SetDownloadHandler(maKH, out fileName);

                // chuẩn bị chờ download
                _downloadCompleted = new TaskCompletionSource<bool>();
                await Task.Delay(3000); // đợi nút download render
                //lay vi tri nut download
                Point pointDownload = GetPoinDownloadButton();
                // click vào nút download
                chromiumdownload.GetBrowser().GetHost().SendMouseClickEvent(pointDownload.X, pointDownload.Y, MouseButtonType.Left, false, 1, CefEventFlags.None);
                await Task.Delay(100);
                chromiumdownload.GetBrowser().GetHost().SendMouseClickEvent(pointDownload.X, pointDownload.Y, MouseButtonType.Left, true, 1, CefEventFlags.None);
                // chờ download xong
                var completed = await Task.WhenAny(_downloadCompleted.Task, Task.Delay(10000));
                //check download co thanh cong chua
                if (completed != _downloadCompleted.Task)
                {
                    if (retry < 3)
                    {
                        retry++;
                        await Task.Delay(10000);
                        goto Retry;
                    }
                    else if (!FailedInvoices.Any(x => x.maKH == maKH))
                    {
                        FailedInvoices.Add((maKH, mucDich));
                        continue;
                    }
                }
                else if (!_arrayInvoiceID_Downloaded.Contains(idHoaDon))
                {
                    _arrayInvoiceID_Downloaded.Add(idHoaDon);
                }
                await Task.Delay(1000); // buffer nhỏ cho chắc

            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private async Task RetryLoginIfFailed(AccountEVN acc)
        {
            for (int i = 0; i < 3; i++)
            {
                if (!chromiumdownload.Address.Contains("DangNhap")) { return; } // đã đăng nhập thành công
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

            chromiumdownload.ExecuteScriptAsync(script);
            await Task.Delay(400);
        }
        //////////////////////////////////////////////////////////////////////////////////////////////
        /////Hàm set DownloadHandler động với mỗi maKH mới ở maKH = invoice.maKH; để truyền mã khách hàng của từng row cho file download
        private void SetDownloadHandler(string maKH, out string fileName)
        {
            fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", BuildPdfName(maKH));
            var downloadHandler = new BlobPdfDownloadHandler(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"), () => BuildPdfName(maKH)); // dùng maKH mới
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
                var response = chromiumdownload.EvaluateScriptAsync(script).Result;
                if (response.Success && response.Result != null)
                {
                    dynamic result = response.Result;

                    double x = result.x;
                    double y = result.y;

                    return new Point(Convert.ToInt32(x) - 96, Convert.ToInt32(y) + 28);
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