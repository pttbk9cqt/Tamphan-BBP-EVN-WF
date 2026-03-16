using CefSharp.DevTools.CacheStorage;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tamphan_BBP_EVN_WF.Models;

namespace Tamphan_BBP_EVN_WF.Services
{
    public class AccountService
    {
        private const string ExcelPath = "Data\\Bảng quản lý cấp điện.xlsm";
        private Dictionary<string, AccountEVN> _cache;
        public void LoadAccounts()
        {
            if (!File.Exists(ExcelPath))
                return;

            _cache = new Dictionary<string, AccountEVN>();

            using (var wb = new XLWorkbook(ExcelPath))
            {
                var ws = wb.Worksheet(1);
                var lastRowUsed = ws.LastRowUsed();

                if (lastRowUsed == null)
                    return;

                int lastRow = lastRowUsed.RowNumber();

                for (int row = 2; row <= lastRow; row++)
                {
                    string maKH = ws.Cell(row, "I").GetString().Trim();

                    if (!_cache.ContainsKey(maKH))
                    {
                        _cache.Add(maKH, new AccountEVN
                        {
                            Id = ws.Cell(row, "A").GetString(),
                            MaKH = maKH,
                            MucDichSuDung = ws.Cell(row, "K").GetString(),
                            Username = ws.Cell(row, "F").GetString(),
                            Password = ws.Cell(row, "G").GetString()
                        });
                    }
                }
            }
        }

        public AccountEVN GetAccount(string maKH)
        {
            if (string.IsNullOrWhiteSpace(maKH))
                return null;

            if (_cache == null)
                LoadAccounts();

            if (_cache.TryGetValue(maKH, out var account))
                return account;

            return null;
        }

        public List<AccountEVN> GetAllAccounts()
        {
            if (_cache == null)
                LoadAccounts();

            return new List<AccountEVN>(_cache.Values);
        }
    }
}
