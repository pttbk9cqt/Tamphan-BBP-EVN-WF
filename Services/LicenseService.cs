using System;
using System.Linq;
using System.Net;
using System.Windows.Forms;

public class LicenseService
{
    private string sheetUrl = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRQqLPD4eoMjVRychK6m1nEcsMNmjF1gW2M0KYo4IHSs2QPmjtEXNSy0Rk0LgKSSZVRd5nz8dlL1Gvo/pub?output=csv";

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

                    // Vì Google Form có Timestamp ở cột 0
                    string sheetMachine = cols[1].Trim();
                    string sheetCustomer = cols[2].Trim();
                    string sheetPassword = cols[3].Trim();
                    string status = cols[4].Trim();

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