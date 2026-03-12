using System;
using System.Linq;
using System.Net;
using System.Windows.Forms;

public class LicenseService
{
       private string sheetUrl = "https://docs.google.com/spreadsheets/d/1eKYApSWPyZN-InhtzP-6RXt-5aMHklAg9KtIeeP46es/export?format=csv&gid=0"; //đây là url khi mở qua tab sheet và copy trên browser xuống

    public bool CheckLicense(string machineId, string user, string password)
    {
        try
        {
            using (WebClient wc = new WebClient())
            {
                string csv = wc.DownloadString(sheetUrl);

                var lines = csv.Split('\n').Skip(1);

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var cols = line.Split(',');

                    if (cols.Length < 5)
                        continue;

                    string sheetMachine = cols[0].Trim();
                    string sheetCustomer = cols[1].Trim();
                    string sheetPassword = cols[2].Trim();
                    string expire = cols[3].Trim();
                    string status = cols[4].Trim();

                    if (DateTime.Parse(expire) < DateTime.Today)
                        continue;

                    if (sheetMachine == machineId &&
                        sheetCustomer == user &&
                        sheetPassword == password &&
                        status.Equals("allow", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
        }
        catch
        {
            MessageBox.Show("Không thể kết nối server license.");
        }

        return false;
    }
}