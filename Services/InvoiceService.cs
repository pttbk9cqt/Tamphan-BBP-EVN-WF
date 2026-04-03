using CefSharp;
using CefSharp.WinForms;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;


namespace Tamphan_BBP_EVN_WF.Services
{
    public class InvoiceService
    {
        private ChromiumWebBrowser _browser;
        private AccountService _accountService;
        public InvoiceService(ChromiumWebBrowser browser, AccountService accountService)
        {
            _browser = browser;
            _accountService = accountService;
        }

        public async Task<List<dynamic>> GetInvoicesAsync()
        {
            string script = @"(function () {
                let rows = document.querySelectorAll('.result-tbl table tbody tr');
                let data = [];

                rows.forEach(r => {
                    let tds = r.querySelectorAll('td');
                    if (tds.length >= 5) {
                        data.push({
                            stt: tds[0].innerText.trim(),
                            maKH: tds[1].innerText.trim(),
                            idHoaDon: tds[2].innerText.trim(),
                            kyHieu: tds[3].innerText.trim(),
                            tongTien: tds[4].innerText.trim()
                        });
                    }
                });

                return data;
            })();";

            var result = await _browser.EvaluateScriptAsync(script);

            if (!result.Success || result.Result == null)
                return new List<dynamic>();

            return (List<dynamic>)result.Result;
        }

        public string ExportInvoiceToExcel(List<dynamic> list, string maKH)
        {
            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("HoaDonEVN");

                ws.Cell(1, 1).Value = "STT";
                ws.Cell(1, 2).Value = "Mã KH";
                ws.Cell(1, 3).Value = "ID Hóa Đơn";
                ws.Cell(1, 4).Value = "Ký Hiệu";
                ws.Cell(1, 5).Value = "Tổng Tiền";
                ws.Cell(1, 6).Value = "Mục đích sử dụng";

                int row = 2;

                foreach (dynamic item in list)
                {
                    string maKH_item = item.maKH;

                    var acc = _accountService.GetAccount(maKH_item);
                    string mucDich = acc?.MucDichSuDung ?? "";

                    // convert tiền
                    string raw = item.tongTien;
                    string cleaned = raw
                        .Replace(".", "")
                        .Replace(",", "")
                        .Replace("đ", "")
                        .Trim();

                    decimal.TryParse(cleaned, out decimal money);


                    ws.Cell(row, 1).Value = item.stt;
                    ws.Cell(row, 2).Value = item.maKH;
                    ws.Cell(row, 3).Value = item.idHoaDon;
                    ws.Cell(row, 4).Value = item.kyHieu;
                    //ws.Cell(row, 5).Value = item.tongTien;
                    ws.Cell(row, 5).Value = money;
                    ws.Cell(row, 6).Value = mucDich;

                    row++;
                }
                // Format + tiện ích
                ws.Columns().AdjustToContents();
                ws.RangeUsed().SetAutoFilter();
                ws.SheetView.FreezeRows(1);
                ws.Column(5).Style.NumberFormat.Format = "#,##0";

                //string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"HoaDonEVN_{maKH}_{DateTime.Now:MM-yyyy}.xlsx");
                string filePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    "Downloads",
                    $"HoaDonEVN_{maKH}_{DateTime.Now:MM-yyyy}.xlsx"
                );

                wb.SaveAs(filePath);
                return filePath;
            }
            //done
        }
    }
}
