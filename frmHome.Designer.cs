using System;

namespace Tamphan_BBP_EVN_WF
{
    partial class frmHome
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlFrmHomeLogin = new System.Windows.Forms.Panel();
            this.btnFrmHomeLogin = new System.Windows.Forms.Button();
            this.txtFrmHomePassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFrmHomeMaKH = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnFrmHomeOneMakhALLDownload = new System.Windows.Forms.Button();
            this.btnFrmHomeMultiDownload = new System.Windows.Forms.Button();
            this.pnlDropExcel = new System.Windows.Forms.Panel();
            this.dgvFrmHome = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.btnImportExcelSource = new System.Windows.Forms.Button();
            this.btnFrmHomeCreAccount = new System.Windows.Forms.Button();
            this.btnFrmHomeOnlyMakhInput = new System.Windows.Forms.Button();
            this.btn_downloadfullyear = new System.Windows.Forms.Button();
            this.pnlFrmHomeLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFrmHome)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFrmHomeLogin
            // 
            this.pnlFrmHomeLogin.Controls.Add(this.btnFrmHomeLogin);
            this.pnlFrmHomeLogin.Controls.Add(this.txtFrmHomePassword);
            this.pnlFrmHomeLogin.Controls.Add(this.label2);
            this.pnlFrmHomeLogin.Controls.Add(this.txtFrmHomeMaKH);
            this.pnlFrmHomeLogin.Controls.Add(this.label1);
            this.pnlFrmHomeLogin.Location = new System.Drawing.Point(207, 15);
            this.pnlFrmHomeLogin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlFrmHomeLogin.Name = "pnlFrmHomeLogin";
            this.pnlFrmHomeLogin.Size = new System.Drawing.Size(732, 74);
            this.pnlFrmHomeLogin.TabIndex = 4;
            // 
            // btnFrmHomeLogin
            // 
            this.btnFrmHomeLogin.Location = new System.Drawing.Point(608, 28);
            this.btnFrmHomeLogin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnFrmHomeLogin.Name = "btnFrmHomeLogin";
            this.btnFrmHomeLogin.Size = new System.Drawing.Size(85, 26);
            this.btnFrmHomeLogin.TabIndex = 4;
            this.btnFrmHomeLogin.Text = "Login";
            this.btnFrmHomeLogin.UseVisualStyleBackColor = true;
            this.btnFrmHomeLogin.Click += new System.EventHandler(this.btnFrmHomeLogin_Click);
            // 
            // txtFrmHomePassword
            // 
            this.txtFrmHomePassword.Location = new System.Drawing.Point(417, 28);
            this.txtFrmHomePassword.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtFrmHomePassword.Name = "txtFrmHomePassword";
            this.txtFrmHomePassword.Size = new System.Drawing.Size(160, 22);
            this.txtFrmHomePassword.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(369, 32);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Pass";
            // 
            // txtFrmHomeMaKH
            // 
            this.txtFrmHomeMaKH.Location = new System.Drawing.Point(104, 27);
            this.txtFrmHomeMaKH.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtFrmHomeMaKH.Name = "txtFrmHomeMaKH";
            this.txtFrmHomeMaKH.Size = new System.Drawing.Size(224, 22);
            this.txtFrmHomeMaKH.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 31);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mã KH";
            // 
            // btnFrmHomeOneMakhALLDownload
            // 
            this.btnFrmHomeOneMakhALLDownload.Location = new System.Drawing.Point(63, 116);
            this.btnFrmHomeOneMakhALLDownload.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnFrmHomeOneMakhALLDownload.Name = "btnFrmHomeOneMakhALLDownload";
            this.btnFrmHomeOneMakhALLDownload.Size = new System.Drawing.Size(436, 38);
            this.btnFrmHomeOneMakhALLDownload.TabIndex = 10;
            this.btnFrmHomeOneMakhALLDownload.Text = "Download tất cả thông báo điện/hóa đơn nhìn thấy sau Login";
            this.btnFrmHomeOneMakhALLDownload.UseVisualStyleBackColor = true;
            this.btnFrmHomeOneMakhALLDownload.Click += new System.EventHandler(this.btnFrmHomeOneMakhALLDownload_Click);
            // 
            // btnFrmHomeMultiDownload
            // 
            this.btnFrmHomeMultiDownload.Location = new System.Drawing.Point(532, 666);
            this.btnFrmHomeMultiDownload.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnFrmHomeMultiDownload.Name = "btnFrmHomeMultiDownload";
            this.btnFrmHomeMultiDownload.Size = new System.Drawing.Size(140, 28);
            this.btnFrmHomeMultiDownload.TabIndex = 11;
            this.btnFrmHomeMultiDownload.Text = "Multiple Download";
            this.btnFrmHomeMultiDownload.UseVisualStyleBackColor = true;
            this.btnFrmHomeMultiDownload.Click += new System.EventHandler(this.btnFrmHomeMultiDownload_Click);
            // 
            // pnlDropExcel
            // 
            this.pnlDropExcel.AllowDrop = true;
            this.pnlDropExcel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDropExcel.Location = new System.Drawing.Point(1009, 199);
            this.pnlDropExcel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlDropExcel.Name = "pnlDropExcel";
            this.pnlDropExcel.Size = new System.Drawing.Size(182, 35);
            this.pnlDropExcel.TabIndex = 12;
            this.pnlDropExcel.DragDrop += new System.Windows.Forms.DragEventHandler(this.pnlDropExcel_DragDrop);
            this.pnlDropExcel.DragEnter += new System.Windows.Forms.DragEventHandler(this.pnlDropExcel_DragEnter);
            // 
            // dgvFrmHome
            // 
            this.dgvFrmHome.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFrmHome.Location = new System.Drawing.Point(56, 258);
            this.dgvFrmHome.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvFrmHome.Name = "dgvFrmHome";
            this.dgvFrmHome.RowHeadersWidth = 51;
            this.dgvFrmHome.Size = new System.Drawing.Size(1136, 384);
            this.dgvFrmHome.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(836, 209);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "Import file excel vào đây";
            // 
            // btnImportExcelSource
            // 
            this.btnImportExcelSource.Location = new System.Drawing.Point(63, 199);
            this.btnImportExcelSource.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnImportExcelSource.Name = "btnImportExcelSource";
            this.btnImportExcelSource.Size = new System.Drawing.Size(239, 28);
            this.btnImportExcelSource.TabIndex = 14;
            this.btnImportExcelSource.Text = "Nhập từ file dữ liệu excel gốc";
            this.btnImportExcelSource.UseVisualStyleBackColor = true;
            this.btnImportExcelSource.Click += new System.EventHandler(this.btnImportExcelSource_Click);
            // 
            // btnFrmHomeCreAccount
            // 
            this.btnFrmHomeCreAccount.BackColor = System.Drawing.SystemColors.Window;
            this.btnFrmHomeCreAccount.Location = new System.Drawing.Point(1009, 43);
            this.btnFrmHomeCreAccount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnFrmHomeCreAccount.Name = "btnFrmHomeCreAccount";
            this.btnFrmHomeCreAccount.Size = new System.Drawing.Size(143, 28);
            this.btnFrmHomeCreAccount.TabIndex = 15;
            this.btnFrmHomeCreAccount.Text = "New account";
            this.btnFrmHomeCreAccount.UseVisualStyleBackColor = false;
            this.btnFrmHomeCreAccount.Click += new System.EventHandler(this.btnFrmHomeCreAccount_Click);
            // 
            // btnFrmHomeOnlyMakhInput
            // 
            this.btnFrmHomeOnlyMakhInput.Location = new System.Drawing.Point(532, 116);
            this.btnFrmHomeOnlyMakhInput.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnFrmHomeOnlyMakhInput.Name = "btnFrmHomeOnlyMakhInput";
            this.btnFrmHomeOnlyMakhInput.Size = new System.Drawing.Size(383, 38);
            this.btnFrmHomeOnlyMakhInput.TabIndex = 16;
            this.btnFrmHomeOnlyMakhInput.Text = "Download đúng 1 thông báo/hóa đơn được nhập";
            this.btnFrmHomeOnlyMakhInput.UseVisualStyleBackColor = true;
            this.btnFrmHomeOnlyMakhInput.Click += new System.EventHandler(this.btnFrmHomeOnlyMakhInput_Click);
            // 
            // btn_downloadfullyear
            // 
            this.btn_downloadfullyear.Location = new System.Drawing.Point(975, 116);
            this.btn_downloadfullyear.Name = "btn_downloadfullyear";
            this.btn_downloadfullyear.Size = new System.Drawing.Size(177, 39);
            this.btn_downloadfullyear.TabIndex = 17;
            this.btn_downloadfullyear.Text = "downloadfullyear";
            this.btn_downloadfullyear.UseVisualStyleBackColor = true;
            this.btn_downloadfullyear.Click += new System.EventHandler(this.btn_downloadfullyear_Click);
            // 
            // frmHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1247, 709);
            this.Controls.Add(this.btn_downloadfullyear);
            this.Controls.Add(this.btnFrmHomeOnlyMakhInput);
            this.Controls.Add(this.btnFrmHomeCreAccount);
            this.Controls.Add(this.btnImportExcelSource);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dgvFrmHome);
            this.Controls.Add(this.pnlDropExcel);
            this.Controls.Add(this.btnFrmHomeMultiDownload);
            this.Controls.Add(this.btnFrmHomeOneMakhALLDownload);
            this.Controls.Add(this.pnlFrmHomeLogin);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmHome";
            this.Text = "TamphanTools-Main";
            this.pnlFrmHomeLogin.ResumeLayout(false);
            this.pnlFrmHomeLogin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFrmHome)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion
        private System.Windows.Forms.Panel pnlFrmHomeLogin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFrmHomeMaKH;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFrmHomePassword;
        private System.Windows.Forms.Button btnFrmHomeLogin;
        private System.Windows.Forms.Button btnFrmHomeOneMakhALLDownload;
        private System.Windows.Forms.Button btnFrmHomeMultiDownload;
        private System.Windows.Forms.Panel pnlDropExcel;
        private System.Windows.Forms.DataGridView dgvFrmHome;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnImportExcelSource;
        private System.Windows.Forms.Button btnFrmHomeCreAccount;
        private System.Windows.Forms.Button btnFrmHomeOnlyMakhInput;
        private System.Windows.Forms.Button btn_downloadfullyear;
    }
}

