using CefSharp;
using CefSharp.WinForms;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Tamphan_BBP_EVN_WF
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {  //Dọn các subprocess còn sót từ lần chạy trước
            foreach (var p in Process.GetProcessesByName("CefSharp.BrowserSubprocess"))
            {
                try { p.Kill(); } catch { }
            }
            //thêm đoạn này để bắt lỗi nếu có, tránh crash mà không biết lý do
            Application.ThreadException += (sender, e) => { MessageBox.Show(e.Exception.ToString(), "ThreadException");};
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => {MessageBox.Show(e.ExceptionObject.ToString(), "UnhandledException");};
            // ==============================
            // Cấu hình CEF
            // ==============================
            var settings = new CefSettings();
            // thư mục cache
            string cachePath = Path.Combine(Application.StartupPath, "cache");
            settings.CachePath = cachePath;

            // GIỚI HẠN CACHE 400MB
            settings.CefCommandLineArgs.Add("disk-cache-size", (400 * 1024 * 1024).ToString());

            // tắt các thành phần không cần thiết
            settings.CefCommandLineArgs.Add("disable-gpu", "1");
            settings.CefCommandLineArgs.Add("disable-extensions", "1");
            settings.CefCommandLineArgs.Add("disable-plugins", "1");
            settings.CefCommandLineArgs.Add("disable-media-cache", "1");
            settings.CefCommandLineArgs.Add("disable-component-update", "1");
            settings.CefCommandLineArgs.Add("disable-background-networking", "1");
            settings.CefCommandLineArgs.Add("disable-sync", "1");
            settings.CefCommandLineArgs.Add("disable-translate", "1");
            settings.CefCommandLineArgs.Add("disable-logging", "1");

            // user agent
            settings.UserAgent =
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
            "(KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36";

            // log debug
            settings.LogSeverity = LogSeverity.Info;
            settings.LogFile = Path.Combine(Application.StartupPath, "cef.log");

            // khởi tạo CEF
            Cef.Initialize(settings);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Chạy form đăng nhập
            //Application.Run(new Login());
            Application.Run(new Home());
            // Khi form đóng, dọn CEF
            Cef.Shutdown();
            //Buộc dọn subprocess còn đang chạy
            foreach (var p in Process.GetProcessesByName("CefSharp.BrowserSubprocess"))
            {
                try { p.Kill(); } catch { }
            }
            //Application.Run(new Cre1506("phanthanhtam","Mocungcunganhcungnhat@bcm26","https://eoffice.becamexbinhphuoc.com.vn/workflow/SitePages/NewWorkflow.aspx?mode=1&ListID=589dfff1-f412-41fd-8824-c48a2bf66309"));
        }
    }
}

