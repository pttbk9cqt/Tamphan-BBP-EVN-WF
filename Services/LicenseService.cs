using System;
using System.Linq;
using System.Net;
using System.Windows.Forms;

public class LicenseService
{
    private string sheetUrl = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRQqLPD4eoMjVRychK6m1nEcsMNmjF1gW2M0KYo4IHSs2QPmjtEXNSy0Rk0LgKSSZVRd5nz8dlL1Gvo/pub?output=csv";

    public bool CheckLicense(string machineId, string customer)
    {
        try
        {
            using (WebClient wc = new WebClient())
            {
                string csv = wc.DownloadString(sheetUrl);

                var lines = csv.Split('\n').Skip(1);

                foreach (var line in lines)
                {
                    var cols = line.Split(',');

                    if (cols.Length < 4)
                        continue;

                    string sheetMachine = cols[0].Trim();
                    string sheetCustomer = cols[1].Trim();
                    DateTime expire = DateTime.Parse(cols[2]);
                    string status = cols[3].Trim();

                    if (sheetMachine == machineId &&
                        sheetCustomer == customer &&
                        status == "allow" &&
                        DateTime.Now <= expire)
                    {
                        return true;
                    }
                }
            }
        }
        catch
        {
            MessageBox.Show("Không thể kết nối server license.");
            return false;
        }

        return false;
    }
}