using CefSharp;
using System;
using System.Collections.Concurrent;
using System.IO;

namespace Tamphan_BBP_EVN_WF.Services
{
    public class BlobPdfDownloadHandler : IDownloadHandler
    {
        private readonly string _targetFolder;
        private readonly Func<string> _buildFileName;
        private readonly ConcurrentDictionary<int, bool> _activeDownloads;

        public event Action<string> PdfDownloaded;

        public BlobPdfDownloadHandler(string targetFolder, Func<string> buildFileName)
        {
            _targetFolder = targetFolder;
            _buildFileName = buildFileName;
            _activeDownloads = new ConcurrentDictionary<int, bool>();

            if (!Directory.Exists(_targetFolder))
                Directory.CreateDirectory(_targetFolder);
        }

        public bool OnBeforeDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
        {
            if (callback == null)
                return false;
            if (callback.IsDisposed)
                return false;

            string tempPath = Path.Combine(_targetFolder, downloadItem.SuggestedFileName);

            _activeDownloads[downloadItem.Id] = true;

            using (callback)
            {
                callback.Continue(tempPath, false);
            }
            return true;
        }

        public void OnDownloadUpdated(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
        {
            bool active;
            if (!_activeDownloads.TryGetValue(downloadItem.Id, out active))
                return;

            if (downloadItem.IsComplete)
            {
                _activeDownloads.TryRemove(downloadItem.Id, out active);

                string finalPath = EnsureUniqueFileName(
                    Path.Combine(_targetFolder, _buildFileName())
                );

                try
                {
                    File.Move(downloadItem.FullPath, finalPath);
                }
                catch (IOException)
                {
                    finalPath = EnsureUniqueFileName(finalPath);
                    File.Move(downloadItem.FullPath, finalPath);
                }

                if (PdfDownloaded != null)
                    PdfDownloaded(finalPath);
            }
        }

        private static string EnsureUniqueFileName(string path)
        {
            if (!File.Exists(path))
                return path;

            string dir = Path.GetDirectoryName(path);
            string name = Path.GetFileNameWithoutExtension(path);
            string ext = Path.GetExtension(path);

            int i = 1;
            string newPath;

            do
            {
                newPath = Path.Combine(dir, name + "_" + i + ext);
                i++;
            }
            while (File.Exists(newPath));

            return newPath;
        }

        public bool CanDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, string url, string requestMethod)
        {
            return true;//throw new NotImplementedException();
        }
    }
}
