using ClosedXML.Excel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tamphan_BBP_EVN_WF.Models;

namespace Tamphan_BBP_EVN_WF.Services
{
    public class AccountService
    {
        private const string ExcelPath = "Data\\Bảng quản lý cấp điện.xlsm";
        private Dictionary<string, AccountEVN> _cache_mappAcc;
        private Dictionary<string, List<string>> _map_gop = new Dictionary<string, List<string>>();
        public void LoadAccounts()
        {
            if (!File.Exists(ExcelPath))
                return;

            _cache_mappAcc = new Dictionary<string, AccountEVN>();

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
                    //
                    string maGop = ws.Cell(row, "H").GetString().Trim();
                    //
                    if (!_cache_mappAcc.ContainsKey(maKH))
                    {
                        _cache_mappAcc.Add(maKH, new AccountEVN
                        {
                            Id = ws.Cell(row, "A").GetString(),
                            MaKH = maKH,
                            MucDichSuDung = ws.Cell(row, "K").GetString(),
                            Username = ws.Cell(row, "F").GetString(),
                            Password = ws.Cell(row, "G").GetString()
                        });
                    }
                    //
                    if (string.IsNullOrEmpty(maGop))
                    {
                        if (!_map_gop.ContainsKey("no"))
                        {
                            _map_gop.Add("no", new List<string>() { maKH });
                        }
                        else
                        {
                            List<string> val = _map_gop["no"];
                            val.Add(maKH);
                            _map_gop["no"] = val;
                        }
                    }
                    else
                    {
                        if (!_map_gop.ContainsKey(maGop))
                        {
                            _map_gop.Add(maGop, new List<string>() { maKH });
                        }
                        else
                        {
                            List<string> val = _map_gop[maGop];
                            val.Add(maKH);
                            _map_gop[maGop] = val;
                        }
                    }
                }
            }
        }

        public AccountEVN GetAccount(string maKH)
        {
            if (string.IsNullOrWhiteSpace(maKH))
                return null;

            if (_cache_mappAcc == null)
                LoadAccounts();

            if (_cache_mappAcc.TryGetValue(maKH, out var account))
                return account;

            return null;
        }

        public List<AccountEVN> GetAllAccounts()
        {
            if (_cache_mappAcc == null)
                LoadAccounts();

            var values = new List<AccountEVN>(_cache_mappAcc.Values);
            var result = values.Where(x => !string.IsNullOrEmpty(x.MaKH));
            return result.ToList();
        }
        public Dictionary<string, List<string>> GetMapAccount()
        {
            return _map_gop;
        }
    }
}
