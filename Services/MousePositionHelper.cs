using System.Windows.Forms;

namespace Tamphan_BBP_EVN_WF.Services
{
    internal class MousePositionHelper
    {
        private static Timer _timer;
        private static Form _form;

        public static void Start(Form form, int interval = 30)
        {
            if (_timer != null) return;

            _form = form;
            _timer = new Timer { Interval = interval };
            _timer.Tick += (s, e) =>
            {
                var p = Cursor.Position;
                _form.Text = $"X={p.X}, Y={p.Y}";
            };
            _timer.Start();
        }

        public static void Stop()
        {
            _timer?.Stop();
            _timer = null;
        }
    }
}
