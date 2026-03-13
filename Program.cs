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
                try 
                { 
                    p.Kill(); 
                } 
                catch 
                { 
                }
            }

            Application.ThreadException += (sender, e) =>
            {
                MessageBox.Show(e.Exception.ToString(), "ThreadException");
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                MessageBox.Show(e.ExceptionObject.ToString(), "UnhandledException");
            };

            var settings = new CefSettings();
            string cachePath = Path.Combine(Application.StartupPath, "cache");   // tạo folder cache trong tool
                                                                                 // XÓA CACHE TRƯỚC KHI KHỞI TẠO CEF
            if (Directory.Exists(cachePath))
            {
                try
                {
                    Directory.Delete(cachePath, true);
                }
                catch { }
            }

            settings.CachePath = cachePath;
            // nên tắt để tránh crash trên một số máy
            settings.CefCommandLineArgs.Add("disable-gpu", "1");
            settings.CefCommandLineArgs.Add("disable-gpu-compositing", "1");
            settings.CefCommandLineArgs.Add("disable-extensions", "1");
            settings.CefCommandLineArgs.Add("process-per-site", "1");
            settings.CefCommandLineArgs.Add("disable-plugins", "1");
            settings.CefCommandLineArgs.Add("disable-media-cache", "1");
            settings.CefCommandLineArgs.Add("disable-component-update", "1");
            settings.CefCommandLineArgs.Add("disable-background-networking", "1");
            settings.CefCommandLineArgs.Add("disable-sync", "1");
            settings.CefCommandLineArgs.Add("disable-translate", "1");
            settings.CefCommandLineArgs.Add("disable-logging", "1");

            settings.LogSeverity = LogSeverity.Info;
            settings.LogFile = Path.Combine(Application.StartupPath, "cef.log");

            Cef.Initialize(settings);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Login());

            Cef.Shutdown();
            //Buộc dọn subprocess còn đang chạy
            foreach (var p in Process.GetProcessesByName("CefSharp.BrowserSubprocess"))
            {
                try
                {
                    p.Kill();
                }
                catch
                {
                }
            }
            //Application.Run(new Cre1506("phanthanhtam","Mocungcunganhcungnhat@bcm26","https://eoffice.becamexbinhphuoc.com.vn/workflow/SitePages/NewWorkflow.aspx?mode=1&ListID=589dfff1-f412-41fd-8824-c48a2bf66309"));
        }
    }
}

