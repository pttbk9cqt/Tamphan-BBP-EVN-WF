using CefSharp;
using CefSharp.WinForms;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Tesseract;

namespace Tamphan_BBP_EVN_WF.Services
{
    // ==============================
    // Class mô tả tọa độ captcha
    // ==============================
    public class CaptchaRect
    {
        public double x { get; set; }
        public double y { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public double devicePixelRatio { get; set; }
    }

    // ==============================
    // Helper xử lý captcha
    // ==============================
    public class CaptchaHelper
    {
        private readonly ChromiumWebBrowser _browser;
        private readonly CaptchaOcrService _ocrService;
        private readonly string _captchaId;

        public CaptchaHelper(ChromiumWebBrowser browser, string captchaId)
        {
            _browser = browser;
            _captchaId = captchaId;
            _ocrService = new CaptchaOcrService();
        }

        public CaptchaHelper(ChromiumWebBrowser browser)
        {
            _browser = browser;
            _ocrService = new CaptchaOcrService();
        }

        // ==============================
        // Lấy tọa độ captcha từ DOM
        // ==============================
        public async Task<CaptchaRect> GetCaptchaRectAsync()
        {
            var jsCode = $@"
                            (function () {{
                                var img = document.getElementById('{_captchaId}');
                                if (!img) return null;

                                var rect = img.getBoundingClientRect();
                                return {{
                                    x: rect.left,
                                    y: rect.top,
                                    width: rect.width,
                                    height: rect.height,
                                    devicePixelRatio: window.devicePixelRatio
                                }};
                            }})();
                            ";

            var response = await _browser.EvaluateScriptAsync(jsCode);

            if (!response.Success || response.Result == null)
                return null;

            return JsonConvert.DeserializeObject<CaptchaRect>(
                JsonConvert.SerializeObject(response.Result)
            );
        }

        // ==============================
        // Chụp screenshot toàn bộ browser
        // ==============================
        public async Task<Bitmap> CaptureBrowserAsync()
        {
            byte[] pngBytes = await _browser.CaptureScreenshotAsync();

            using (var ms = new MemoryStream(pngBytes))
            {
                return new Bitmap(ms);
            }
        }

        // ==============================
        // Crop vùng captcha
        // ==============================
        public Bitmap CropCaptcha(Bitmap fullImage, CaptchaRect rect)
        {
            float scale = (float)rect.devicePixelRatio;

            int x = (int)(rect.x * scale);
            int y = (int)(rect.y * scale);
            int w = (int)(rect.width * scale);
            int h = (int)(rect.height * scale);

            // đảm bảo không vượt khỏi ảnh
            if (x < 0) x = 0;
            if (y < 0) y = 0;

            if (x + w > fullImage.Width)
                w = fullImage.Width - x;

            if (y + h > fullImage.Height)
                h = fullImage.Height - y;

            if (w <= 0 || h <= 0)
                throw new Exception("Captcha crop rect invalid");

            Rectangle cropRect = new Rectangle(x, y, w, h);

            return fullImage.Clone(cropRect, fullImage.PixelFormat);
        }

        // ==============================
        // Giải captcha
        // ==============================
        public async Task<string> SolveCaptchaAsync()
        {
            var rect = await GetCaptchaRectAsync();
            if (rect == null)
                return null;
            int again = 0;
        again:
            try
            {
                using (Bitmap fullPage = await CaptureBrowserAsync())
                {
                    using (Bitmap captcha = CropCaptcha(fullPage, rect))
                    {
                        // debug nếu cần
                        //captcha.Save("captcha_debug.png");

                        string text = _ocrService.ReadCaptcha(captcha);

                        if (string.IsNullOrWhiteSpace(text))
                            return null;

                        return text;
                    }
                }
            }
            catch (Exception)
            {
                //retry lai de tranh loi truong hop dang capture thi di chuyen/thay doi kich thuoc man hinh task.
                if (again < 50)
                {
                    again++;
                    await Task.Delay(1000);
                    goto again;
                }
                return null;
            }
        }

        // ==============================
        // Điền captcha vào input
        // ==============================
        public async Task AutoFillCaptchaAsync()
        {
            string captchaText = await SolveCaptchaAsync();

            if (string.IsNullOrWhiteSpace(captchaText))
                return;

            string js = $@"
            (function(){{
                var el = document.querySelector('input[placeholder=""Nhập chính xác nội dung ở trên.""]');
                if(!el) return 'NOT_FOUND';

                el.focus();
                el.value = '{captchaText}';

                el.dispatchEvent(new Event('input', {{ bubbles: true }}));
                el.dispatchEvent(new Event('change', {{ bubbles: true }}));

                return 'OK';
            }})();
            ";

            await _browser.EvaluateScriptAsync(js);
        }
    }

    // ==============================
    // OCR Service
    // ==============================
    public class CaptchaOcrService
    {
        private readonly string _tessDataPath;

        public CaptchaOcrService()
        {
            _tessDataPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "tessdata"
            );
        }

        // ==============================
        // Tiền xử lý captcha
        // ==============================
        public Bitmap PreprocessCaptcha(Bitmap src)
        {
            Bitmap bmp = new Bitmap(src.Width, src.Height);

            for (int y = 0; y < src.Height; y++)
            {
                for (int x = 0; x < src.Width; x++)
                {
                    Color c = src.GetPixel(x, y);

                    bool isRed =
                        c.R > 120 &&
                        c.R > c.G * 1.3 &&
                        c.R > c.B * 1.3;

                    bmp.SetPixel(x, y, isRed ? Color.Black : Color.White);
                }
            }

            return bmp;
        }

        // ==============================
        // OCR đọc captcha
        // ==============================
        public string ReadCaptcha(Bitmap bitmap)
        {
            using (var engine = new TesseractEngine(_tessDataPath, "eng", EngineMode.Default))
            {
                engine.SetVariable(
                    "tessedit_char_whitelist",
                    "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
                );

                using (var pix = PixConverter.ToPix(bitmap))
                {
                    using (var page = engine.Process(pix))
                    {
                        string text = page.GetText();

                        if (string.IsNullOrWhiteSpace(text))
                            return "";

                        return text
                            .Replace("\n", "")
                            .Replace(" ", "")
                            .Trim();
                    }
                }
            }
        }
    }
}

