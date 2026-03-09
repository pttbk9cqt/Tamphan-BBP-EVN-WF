using ClosedXML.Excel;
using System.IO;
using Tamphan_BBP_EVN_WF.Models;

namespace Tamphan_BBP_EVN_WF.Services
{
    internal class ExcelAccountEVNService
    {
        private const string ExcelPath = "Data\\AccountEVN-addWF.xlsm";

        public AccountEVN GetAccount(string maKH)
        {
            if (!File.Exists(ExcelPath))
                return null;

            using (var wb = new XLWorkbook(ExcelPath))
            {
                var ws = wb.Worksheet(1);

                for (int row = 2; row <= 1000; row++)
                {
                    string maKHExcel = ws.Cell(row, "B").GetString().Trim();

                    if (maKHExcel == maKH)
                    {
                        return new AccountEVN
                        {
                            Id = ws.Cell(row, "A").GetString(),
                            MaKH = maKHExcel,
                            MucDichSuDung = ws.Cell(row, "C").GetString(),
                            Password = ws.Cell(row, "E").GetString()
                        };
                    }
                }
            }

            return null;
        }
    }
}
