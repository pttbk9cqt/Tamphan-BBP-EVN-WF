using CefSharp;
using CefSharp.WinForms;

public class DrawCircleService
{
    private readonly ChromiumWebBrowser _browser;

    public DrawCircleService(ChromiumWebBrowser browser)
    {
        _browser = browser;
    }

    public void DrawCircle(int x, int y, int size = 20)
    {
        int radius = size / 2;

        string script = $@"
        (function() {{
            let circle = document.createElement('div');

            circle.style.position = 'absolute';
            circle.style.width = '{size}px';
            circle.style.height = '{size}px';
            circle.style.border = '2px solid red';
            circle.style.borderRadius = '50%';
            circle.style.left = '{x - radius}px';
            circle.style.top = '{y - radius}px';
            circle.style.zIndex = 999999;

            document.body.appendChild(circle);
        }})();
        ";

        _browser.ExecuteScriptAsync(script);
    }
}