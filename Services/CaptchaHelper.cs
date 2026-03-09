using CefSharp;
using CefSharp.WinForms;
using Newtonsoft.Json;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Tesseract;

namespace Tamphan_BBP_EVN_WF.Services
{
    // Đây là class dữ liệu mô tả tọa độ captcha
    public class CaptchaRect
    {
        public double x { get; set; }
        public double y { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public double devicePixelRatio { get; set; }
    }

    // Class helper để thao tác với captcha
    public class CaptchaHelper
    {
        private readonly ChromiumWebBrowser _browser;
        private readonly CaptchaOcrService _ocrService;
        public CaptchaHelper(ChromiumWebBrowser browser)
        {
            _browser = browser;
            _ocrService = new CaptchaOcrService(); // hoặc inject nếu bạn muốn
        }

        // đoạn dưới đây sẽ lấy ảnh captcha ra
        public async Task<CaptchaRect> GetCaptchaRectAsync()
        {
            var jsCode = @"
            (function () {
                var img = document.getElementById('imgCaptcha');
                if (!img) return null;

                var rect = img.getBoundingClientRect();
                return {
                    x: rect.left,
                    y: rect.top,
                    width: rect.width,
                    height: rect.height,
                    devicePixelRatio: window.devicePixelRatio
                };
            })();
            ";

            var response = await _browser.EvaluateScriptAsync(jsCode);

            if (!response.Success || response.Result == null)
                return null;

            return JsonConvert.DeserializeObject<CaptchaRect>(
                JsonConvert.SerializeObject(response.Result)
            );
        }

        public async Task<Bitmap> CaptureBrowserAsync()
        {
            // CaptureScreenshotAsync returns a byte[] (PNG format), so convert it to Bitmap
            byte[] pngBytes = await _browser.CaptureScreenshotAsync();
            using (var ms = new MemoryStream(pngBytes))
            {
                return new Bitmap(ms);
            }
        }


        public Bitmap CropCaptcha(Bitmap fullImage, CaptchaRect rect)
        {
            float scale = (float)rect.devicePixelRatio;

            Rectangle cropRect = new Rectangle(
                (int)(rect.x * scale),
                (int)(rect.y * scale),
                (int)(rect.width * scale),
                (int)(rect.height * scale)
            );

            return fullImage.Clone(cropRect, fullImage.PixelFormat);
        }


        // đoạn này chỉ làm đúng 1 việc: trả về text captcha
        public async Task<string> SolveCaptchaAsync()
        {
            var rect = await GetCaptchaRectAsync();
            if (rect == null)
                return null;

            Bitmap fullPage = await CaptureBrowserAsync();
            Bitmap captcha = CropCaptcha(fullPage, rect);

            captcha.Save("captcha.png"); // debug nếu cần
            string text = _ocrService.ReadCaptcha(captcha);

            return text;
        }


        //đoạn code điền captcha sau khi đã xử lý hoàn chỉnh vào web
        public async Task AutoFillCaptchaAsync()
        {
            string captchaText = await SolveCaptchaAsync();
            if (string.IsNullOrEmpty(captchaText))
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

            var result = await _browser.EvaluateScriptAsync(js);
        }
    }

    public class CaptchaOcrService
    {
        private string _tessDataPath;

        // ===== B1: TIỀN XỬ LÝ ẢNH =====
        public Bitmap PreprocessCaptcha(Bitmap src)
        {
            Bitmap bmp = new Bitmap(src.Width, src.Height);

            for (int y = 0; y < src.Height; y++)
            {
                for (int x = 0; x < src.Width; x++)
                {
                    Color c = src.GetPixel(x, y);

                    // Lọc màu đỏ
                    bool isRed =
                        c.R > 120 &&
                        c.R > c.G * 1.3 &&
                        c.R > c.B * 1.3;

                    bmp.SetPixel(x, y, isRed ? Color.Black : Color.White);
                }
            }

            return bmp;
        }

        // ===== B2: OCR =====
        public string ReadCaptcha(Bitmap bitmap)
        {
            string result = "";

            using (var engine = new TesseractEngine(_tessDataPath, "eng", EngineMode.Default))
            {
                engine.SetVariable(
                    "tessedit_char_whitelist",
                    "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");

                using (var pix = PixConverter.ToPix(bitmap))
                {
                    using (var page = engine.Process(pix))
                    {
                        result = page.GetText();
                    }
                }
            }

            return result.Trim();
        }
    }
}


