using CefSharp;
using CefSharp.WinForms;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Tamphan_BBP_EVN_WF.Services
{
    public class EvnInformationInvoiceService
    {
        private readonly ChromiumWebBrowser _browser;

        public EvnInformationInvoiceService(ChromiumWebBrowser browser)
        {
            _browser = browser;
        }

        /// <summary>
        /// Lấy danh sách hóa đơn từ bảng trên website EVN
        /// </summary>
        public async Task<List<dynamic>> ExportInforAsync()
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

        /// <summary>
        /// Xuất danh sách hóa đơn ra file Excel
        /// </summary>
        public string ExportToExcel(List<dynamic> list, String maKH)
        {
            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("HoaDonEVN");

                // Header
                ws.Cell(1, 1).Value = "STT";
                ws.Cell(1, 2).Value = "Mã KH";
                ws.Cell(1, 3).Value = "ID Hóa Đơn";
                ws.Cell(1, 4).Value = "Ký Hiệu";
                ws.Cell(1, 5).Value = "Tổng Tiền";

                int row = 2;
                foreach (dynamic item in list)
                {
                    ws.Cell(row, 1).Value = item.stt;
                    ws.Cell(row, 2).Value = item.maKH;
                    ws.Cell(row, 3).Value = item.idHoaDon;
                    ws.Cell(row, 4).Value = item.kyHieu;
                    ws.Cell(row, 5).Value = item.tongTien;
                    row++;
                }
                // Format + tiện ích
                ws.Columns().AdjustToContents();
                ws.RangeUsed().SetAutoFilter();
                ws.SheetView.FreezeRows(1);
                ws.Column(5).Style.NumberFormat.Format = "#,##0";

                //string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"HoaDonEVN_{maKH}_{DateTime.Now:MM-yyyy}.xlsx");
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),"Downloads",$"HoaDonEVN_{maKH}_{DateTime.Now:MM-yyyy}.xlsx");
                wb.SaveAs(filePath);
                return filePath;
            }
        }
    }
}
