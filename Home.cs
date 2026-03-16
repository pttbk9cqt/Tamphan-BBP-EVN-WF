using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tamphan_BBP_EVN_WF.Models;
using Tamphan_BBP_EVN_WF.Services;
using System.Diagnostics;
using CefSharp;

namespace Tamphan_BBP_EVN_WF
{
    public partial class Home : Form
    {
        private AccountService _accountService = new AccountService();
        public string maKH;
        //private static readonly HashSet<string> danhsachmaKHcoGopMa = new HashSet<string> { "PB01050032992", "PB01050036030", "PB01050036935", "PB01050037389", "PB01050039344", "PB01050039586" };
        private static readonly HashSet<string> danhsachmaKHcoGopMa = new HashSet<string> { "PB01050032992", "PB01050036030", "PB01050036935", "PB01050037389", "PB01050039344" };

        ////////////////////////////////////////////////////////////////////////////////////////////////
        public Home()
        {
            InitializeComponent();

            this.FormClosed += Home_FormClosed;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private void Home_Load(object sender, EventArgs e)
        {
            // Load toàn bộ Excel vào RAM ngay khi mở Form
            _accountService.LoadAccounts();
            //MessageBox.Show(MachineService.GetMachineId());
        }

        // ==============================
        // Chuẩn hóa Mã KH
        // ==============================
        private string NormalizeMaKH(string maKH)
        {
            maKH = maKH.Trim();

            if (maKH.Length == 5 && !maKH.StartsWith("PB010500"))
            {
                maKH = "PB010500" + maKH;
            }

            return maKH;
        }

        // ==============================
        // Lấy Account từ textbox
        // ==============================
        private AccountEVN GetAccountFromInput()
        {
            string maKH = NormalizeMaKH(textBox_maKH.Text);
            textBox_maKH.Text = maKH;
            AccountEVN acc = _accountService.GetAccount(maKH);

            if (acc == null)
            {
                MessageBox.Show("Mã khách hàng không tồn tại trong file Excel");
                return null;
            }

            if (string.IsNullOrWhiteSpace(textBox_password.Text) || textBox_password.Text != acc.Password)
            {
                textBox_password.Text = acc.Password;
            }

            return acc;
        }

        // ==============================
        // Login EVN
        // ==============================
        private void Btn_Login_account_riêng_lẻ_Click(object sender, EventArgs e)
        {
            var acc = GetAccountFromInput();
            if (acc == null) return;
            EVN_WEB_LOGIN frm = new EVN_WEB_LOGIN(acc.MaKH, _accountService);
            //MessageBox.Show($"Mục đích sử dụng: {acc.MucDichSuDung}\n" + $"Tên đăng nhập: {acc.MaKH}\n");
            frm.Show();
        }

        // ==============================
        // Download thông báo
        // ==============================
        private void btn_evn_download_Click(object sender, EventArgs e)
        {
            var acc = GetAccountFromInput();
            if (acc == null)
                return;

            //////phần này tra danh sách các mã đã gộp, nếu nó có nhiều mã được gộp thì download sẽ bị sai, trả file pdf đúng tên đúng mã KH nhưng không đúng hóa đơn, nó sẽ nhầm sang căn khác nên phải ngăn ngừa
            if (danhsachmaKHcoGopMa.Contains(acc.MaKH))
            {
                MessageBox.Show("Mã KH được gộp, có nhiều thông báo và hóa đơn, cần tải riêng lẻ");
                return;
            }
            //////////////////////////////////////////////////////////////
            EVN_DownloadThongbao frm = new EVN_DownloadThongbao(acc.MaKH, _accountService);
            frm.ShowDialog();
        }

        // ==============================
        // Drag file Excel: kéo thả file excel vào panel và hiển thị lên datagridview, lưu ý phải chỉnh dragdrop của panel là true, và các event dragenter và dragdrop phải được gán đúng (mở vào Design, chọn panel, vào event và gán đúng event dragenter và dragdrop)
        // ==============================
        private void panelDropExcel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string file = files[0];

                if (file.EndsWith(".xlsx") || file.EndsWith(".xls") || file.EndsWith(".xlsm"))
                {
                    e.Effect = DragDropEffects.Copy;
                }
            }
        }

        private void panelDropExcel_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (files.Length == 0)
                return;

            string filePath = files[0];

            if (!filePath.EndsWith(".xlsx") && !filePath.EndsWith(".xls") && !filePath.EndsWith(".xlsm"))
            {
                MessageBox.Show("Chỉ hỗ trợ file Excel");
                return;
            }

            LoadExcel(filePath);
        }

        // ==============================
        // Load Excel vào DataGridView
        // ==============================
        private void LoadExcel(string filePath)
        {   // reset DataGridView trước
            dataGridView.DataSource = null;
            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });

                    DataTable table = result.Tables[0];
                    //dataGridView.DataSource = table;  //Hiển thị tất cả dữ liệu, kể các các cột và hàng đã hide
                    dataGridView.DataSource =
                        table.DefaultView.ToTable(false, table.Columns[0].ColumnName, table.Columns[4].ColumnName, table.Columns[5].ColumnName, table.Columns[6].ColumnName, table.Columns[7].ColumnName, table.Columns[8].ColumnName, table.Columns[10].ColumnName);//chỉ hiện thị các column cần thiết, bỏ qua các cột đã hide, nếu unhide toàn bộ sheet thì STT là cols thứ 0, và Bên phụ trách là cols thứ 4, tương tự về sau
                    dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                }
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private void btn_import_excelsource_Click(object sender, EventArgs e)
        {
            var list = _accountService.GetAllAccounts();

            DataTable table = new DataTable();
            table.Columns.Add("STT");
            table.Columns.Add("Tên đăng nhập");
            table.Columns.Add("Password");
            table.Columns.Add("Mã KH");
            table.Columns.Add("Mục đích sử dụng");

            foreach (var acc in list)
            {
                table.Rows.Add(acc.Id, acc.Username, acc.Password, acc.MaKH, acc.MucDichSuDung);
            }

            dataGridView.DataSource = table;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private async void btn_multidownload_Click(object sender, EventArgs e)
        {
            if (dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để download");
                return;
            }

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.IsNewRow) continue;

                string maKH = row.Cells[5].Value?.ToString(); // cột Mã KH
                string tenDangNhap = row.Cells[2].Value?.ToString(); // cột Tên đăng nhập

                if (string.IsNullOrWhiteSpace(maKH))
                    continue;

                maKH = NormalizeMaKH(maKH);

                // nếu mã KH nằm trong danh sách gộp mã thì bỏ qua
                if (danhsachmaKHcoGopMa.Contains(maKH))
                    continue;

                using (EVN_DownloadThongbao frm = new EVN_DownloadThongbao(maKH, _accountService))
                {
                    frm.ShowDialog(); // chờ download xong
                }

                await Task.Delay(1000); // nghỉ 1s tránh EVN block
            }

            MessageBox.Show("Done");
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private void btn_newaccount_Click(object sender, EventArgs e)
        {
            string newuser = textBox_maKH.Text.Trim();
            string newpassword = textBox_password.Text.Trim();
            EVN_NewAccount frm = new EVN_NewAccount(newuser, newpassword);
            frm.ShowDialog();
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private void Home_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                // tắt CefSharp
                if (Cef.IsInitialized == true) 
                    Cef.Shutdown();
            }
            catch { }

            // kill các subprocess còn sót
            foreach (var p in Process.GetProcessesByName("CefSharp.BrowserSubprocess"))
            {
                try { p.Kill(); } catch { }
            }

            // thoát toàn bộ ứng dụng
            Application.Exit();
        }
    }

}