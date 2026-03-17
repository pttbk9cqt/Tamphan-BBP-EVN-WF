using CefSharp;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tamphan_BBP_EVN_WF.Models;
using Tamphan_BBP_EVN_WF.Services;

namespace Tamphan_BBP_EVN_WF
{
    public partial class frmHome : Form
    {
        private AccountService accountService = new AccountService();
        public string maKH;
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
        // Download thông báo (tất cả thông báo nhìn thấy - nếu gộp mã, hoặc 1 thông báo với cái không gộp)
        // ==============================
        private void btnFrmHomeOneMakhALLDownload_Click(object sender, EventArgs e)
        {
            var acc = GetAccountFromInput();
            if (acc == null)
                return;

            frmDownload frm = new frmDownload(acc.MaKH, accountService, false);//true là chỉ down 1 maKH được nhập ở txt, false là download hết tất cả gộp có trong đăng nhập
            frm.ShowDialog();
            if (frm.IsCompleted)
            {
                MessageBox.Show("Đã tải xong toàn bộ hóa đơn!");
            }
        }
        // ==============================
        // Download thông báo (chỉ 1 thông báo - kể cả gộp và không gộp)
        // ==============================
        private void btnFrmHomeOnlyMakhInput_Click(object sender, EventArgs e)
        {
            var acc = GetAccountFromInput();
            if (acc == null)
                return;

            frmDownload frm = new frmDownload(acc.MaKH, accountService, true);//true là chỉ down 1 maKH được nhập ở txt
            frm.ShowDialog();
            if (frm.IsCompleted)
            {
                MessageBox.Show("Done!");
            }
        }
        // ==============================
        // Download những thông báo được thả trong DataGridView
        // ==============================
        //private async void btnFrmHomeMultiDownload_Click(object sender, EventArgs e)
        //{
        //    if (dgvFrmHome.Rows.Count == 0)
        //    {
        //        MessageBox.Show("Không có dữ liệu để download");
        //        return;
        //    }

        //    foreach (DataGridViewRow row in dgvFrmHome.Rows)
        //    {   //Trong DataGridView của WinForms luôn có 1 dòng cuối cùng để nhập thêm dữ liệu (dòng trống có dấu * bên trái).Dòng đó chính là New Row
        //        //row.IsNewRow == true → đây là dòng trống để user nhập
        //        //row.IsNewRow == false → là dữ liệu thật
        //        //Ý nghĩa đoạn code
        //        //Nếu gặp dòng trống(dòng nhập mới) → bỏ qua, không xử lý
        //        if (row.IsNewRow)
        //            continue;//ngay khi chạy lệnh này, toàn bộ đoạn code ở dưới sẽ bỏ qua, và trở về vòng lặp tiếp theo

        //        string maKH = row.Cells[5].Value?.ToString(); // cột Mã KH
        //        string tenDangNhap = row.Cells[2].Value?.ToString(); // cột Tên đăng nhập
        //        string gopMa = row.Cells[4].Value?.ToString(); // cột gộp mã của Mã KH

        //        if (string.IsNullOrWhiteSpace(maKH))
        //            continue;

        //        maKH = NormalizeMaKH(maKH);

        //        using (frmDownload frm = new frmDownload(maKH, accountService, false))
        //        {
        //            frm.ShowDialog(); // chờ download xong
        //        }

        //        await Task.Delay(1000); // nghỉ 1s tránh EVN block
        //    }
        //    MessageBox.Show("Done");
        //}
        /////////////////////////////////////////////////////////////////////////////////////////////////
        //phía trên là đoạn download tất cả các rows trong table datagridview mình đưa vào, nhưng duyệt từng row → mở frmDownload từng cái → không tối ưu do là ở những mã gộp thì nó sẽ download trùng lại. Và dưới đây là đoạn gom nhóm theo gopMa, sau đó mỗi nhóm → lấy list maKH → truyền vào frmDownload
        /////////////////////////////////////////////////////////////////////////////////////////////////
        private async void btnFrmHomeMultiDownload_Click(object sender, EventArgs e)
        {
            if (dgvFrmHome.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để download");
                return;
            }

            // 1. Gom nhóm theo gopMa
            var groups = dgvFrmHome.Rows
                .Cast<DataGridViewRow>()
                .Where(r => !r.IsNewRow)
                .Select(r => new
                {
                    MaKH = r.Cells[5].Value?.ToString(),
                    TenDangNhap = r.Cells[2].Value?.ToString(),
                    GopMa = r.Cells[4].Value?.ToString()
                })
                .Where(x => !string.IsNullOrWhiteSpace(x.MaKH))
                .GroupBy(x => x.GopMa);

            // 2. Duyệt từng group
            foreach (var group in groups)
            {
                // Lấy danh sách maKH cùng gopMa
                var listMaKH = group.Select(x => NormalizeMaKH(x.MaKH)).Distinct().ToList();

                // Gọi 1 lần cho cả nhóm
                using (frmDownload frm = new frmDownload(listMaKH, accountService, false))
                {
                    frm.ShowDialog();
                }

                await Task.Delay(1000); // tránh bị block
            }

            MessageBox.Show("Done");
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
                    dgvFrmHome.DataSource = table.DefaultView.ToTable(false, table.Columns[0].ColumnName, table.Columns[4].ColumnName, table.Columns[5].ColumnName, table.Columns[6].ColumnName, table.Columns[7].ColumnName, table.Columns[8].ColumnName, table.Columns[10].ColumnName);//chỉ hiện thị các column cần thiết, bỏ qua các cột đã hide, nếu unhide toàn bộ sheet thì STT là cols thứ 0, và Bên phụ trách là cols thứ 4, tương tự về sau
                    dgvFrmHome.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                }
            }
        }
        // ==============================
        // Bắn file excel source trong Data vào DataGridView để tra cho nhanh khỏi phải mở riêng 1 file excel để tra mã
        // ==============================
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
        // ==============================
        // Tạo acc mới
        // ==============================
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