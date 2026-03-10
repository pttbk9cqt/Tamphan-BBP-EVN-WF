using System;
using System.Windows.Forms;
using Tamphan_BBP_EVN_WF.Models;
using Tamphan_BBP_EVN_WF.Services;
using System.Data;
using System.IO;
using ExcelDataReader;
using System.Threading.Tasks;

namespace Tamphan_BBP_EVN_WF
{
    public partial class Home : Form
    {
        private ExcelAccountEVNService _excelService = new ExcelAccountEVNService();

        public Home()
        {
            InitializeComponent();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            // Load toàn bộ Excel vào RAM ngay khi mở Form
            _excelService.LoadData();
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
            string maKH = NormalizeMaKH(textBox_nhập_mã_khách_hàng.Text);
            textBox_nhập_mã_khách_hàng.Text = maKH;

            AccountEVN acc = _excelService.GetAccount(maKH);

            if (acc == null)
            {
                MessageBox.Show("Mã khách hàng không tồn tại trong file Excel");
                return null;
            }

            if (string.IsNullOrWhiteSpace(textBox_username.Text) || textBox_username.Text != acc.MaKH)
            {
                textBox_username.Text = acc.TenDangNhap;
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

            EVNSPC_WEB_LOGIN frm = new EVNSPC_WEB_LOGIN(acc.MaKH, acc.TenDangNhap ,_excelService);
            //MessageBox.Show($"ID: {acc.Id}\n" + $"Mục đích sử dụng: {acc.MucDichSuDung}\n" + $"Tên đăng nhập: {acc.MaKH}\n" + $"Pass: {acc.Password}");
            frm.Show();
        }

        // ==============================
        // Download thông báo
        // ==============================
        private void btn_evn_download_Click(object sender, EventArgs e)
        {
            var acc = GetAccountFromInput();
            if (acc == null) return;

            EVNSPC_DownloadThongbao frm = new EVNSPC_DownloadThongbao(acc.MaKH, acc.TenDangNhap);
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
                        table.DefaultView.ToTable(false, table.Columns[0].ColumnName, table.Columns[4].ColumnName, table.Columns[5].ColumnName, table.Columns[6].ColumnName, table.Columns[7].ColumnName, table.Columns[8].ColumnName, table.Columns[10].ColumnName);//chỉ hiện thị các column cần thiết, bỏ qua các cột đã hide
                    dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                }
            }
        }


        private void btn_import_excelsource_Click(object sender, EventArgs e)
        {
            var list = _excelService.GetAllAccounts();

            DataTable table = new DataTable();
            table.Columns.Add("STT");
            table.Columns.Add("Tên đăng nhập");
            table.Columns.Add("Password");
            table.Columns.Add("Mã KH");
            table.Columns.Add("Mục đích sử dụng");

            foreach (var acc in list)
            {
                table.Rows.Add(acc.Id, acc.TenDangNhap, acc.Password, acc.MaKH, acc.MucDichSuDung);
            }

            dataGridView.DataSource = table;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

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

                string maKH = row.Cells[4].Value?.ToString(); // cột Mã KH
                string tenDangNhap = row.Cells[2].Value?.ToString(); // cột Tên đăng nhập

                if (string.IsNullOrWhiteSpace(maKH))
                    continue;

                maKH = NormalizeMaKH(maKH);

                using (EVNSPC_DownloadThongbao frm = new EVNSPC_DownloadThongbao(maKH, tenDangNhap))
                {
                    frm.ShowDialog(); // chờ download xong
                }

                await Task.Delay(1000); // nghỉ 1s tránh EVN block
            }

            MessageBox.Show("Done");
        }
    }
    
}