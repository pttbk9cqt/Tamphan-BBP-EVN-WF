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
    public partial class frmHome : Form
    {
        private AccountService accountService = new AccountService();
        public string maKH;
        //private static readonly HashSet<string> danhsachmaKHcoGopMa = new HashSet<string> { "PB01050032992", "PB01050036030", "PB01050036935", "PB01050037389", "PB01050039344", "PB01050039586"};
        private static readonly HashSet<string> danhsachmaKHcoGopMa = new HashSet<string> { "PB01050032992", "PB01050036030", "PB01050036935", "PB01050037389", "PB01050039344" };
        ////////////////////////////////////////////////////////////////////////////////////////////////
        public frmHome()
        {
            InitializeComponent();
            this.FormClosed += Home_FormClosed;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private void Home_Load(object sender, EventArgs e)
        {
            // Load toàn bộ Excel vào RAM ngay khi mở Form
            accountService.LoadAccounts();
        }
        // ==============================
        // Lấy Account từ textbox
        // ==============================
        private AccountEVN GetAccountFromInput()
        {
            string maKH = NormalizeMaKH(txtFrmHomeMaKH.Text);
            txtFrmHomeMaKH.Text = maKH;
            AccountEVN acc = accountService.GetAccount(maKH);

            if (acc == null)
            {
                MessageBox.Show("Mã khách hàng không tồn tại trong file Excel");
                return null;
            }

            if (string.IsNullOrWhiteSpace(txtFrmHomePassword.Text) || txtFrmHomePassword.Text != acc.Password)
            {
                txtFrmHomePassword.Text = acc.Password;
            }

            return acc;
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
        // Login EVN
        // ==============================
        private void btnFrmHomeLogin_Click(object sender, EventArgs e)
        {
            var acc = GetAccountFromInput();
            if (acc == null) return;
            frmEVNLogin frm = new frmEVNLogin(acc.MaKH, accountService);
            frm.Show();
        }
        // ==============================
        // Download thông báo
        // ==============================
        private void btnFrmHomeOneDownload_Click(object sender, EventArgs e)
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
            frmDownload frm = new frmDownload(acc.MaKH, accountService);
            frm.ShowDialog();
        }
        // Drag file Excel: kéo thả file excel vào panel và hiển thị lên datagridview, lưu ý phải chỉnh dragdrop của panel là true, và các event dragenter và dragdrop phải được gán đúng (mở vào Design, chọn panel, vào event và gán đúng event dragenter và dragdrop)
        private void pnlDropExcel_DragEnter(object sender, DragEventArgs e)
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

        private void pnlDropExcel_DragDrop(object sender, DragEventArgs e)
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
            dgvFrmHome.DataSource = null;
            dgvFrmHome.Rows.Clear();
            dgvFrmHome.Columns.Clear();

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
                    dgvFrmHome.DataSource =
                        table.DefaultView.ToTable(false, table.Columns[0].ColumnName, table.Columns[4].ColumnName, table.Columns[5].ColumnName, table.Columns[6].ColumnName, table.Columns[7].ColumnName, table.Columns[8].ColumnName, table.Columns[10].ColumnName);//chỉ hiện thị các column cần thiết, bỏ qua các cột đã hide, nếu unhide toàn bộ sheet thì STT là cols thứ 0, và Bên phụ trách là cols thứ 4, tương tự về sau
                    dgvFrmHome.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                }
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private void btnImportExcelSource_Click(object sender, EventArgs e)
        {
            var list = accountService.GetAllAccounts();

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

            dgvFrmHome.DataSource = table;
            dgvFrmHome.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private async void btnFrmHomeMultiDownload_Click(object sender, EventArgs e)
        {
            if (dgvFrmHome.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để download");
                return;
            }

            foreach (DataGridViewRow row in dgvFrmHome.Rows)
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

                using (frmDownload frm = new frmDownload(maKH, accountService))
                {
                    frm.ShowDialog(); // chờ download xong
                }

                await Task.Delay(1000); // nghỉ 1s tránh EVN block
            }

            MessageBox.Show("Done");
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////
        private void btnFrmHomeCreAccount_Click(object sender, EventArgs e)
        {
            string newuser = txtFrmHomeMaKH.Text.Trim();
            string newpassword = txtFrmHomePassword.Text.Trim();
            frmCreAcc frm = new frmCreAcc(newuser, newpassword);
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