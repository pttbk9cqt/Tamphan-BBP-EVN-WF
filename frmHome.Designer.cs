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
            this.btnFrmHomeOneDownload = new System.Windows.Forms.Button();
            this.btnFrmHomeMultiDownload = new System.Windows.Forms.Button();
            this.pnlDropExcel = new System.Windows.Forms.Panel();
            this.dgvFrmHome = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.btnImportExcelSource = new System.Windows.Forms.Button();
            this.btnFrmHomeCreAccount = new System.Windows.Forms.Button();
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
            this.pnlFrmHomeLogin.Location = new System.Drawing.Point(155, 12);
            this.pnlFrmHomeLogin.Name = "pnlFrmHomeLogin";
            this.pnlFrmHomeLogin.Size = new System.Drawing.Size(549, 60);
            this.pnlFrmHomeLogin.TabIndex = 4;
            // 
            // btnFrmHomeLogin
            // 
            this.btnFrmHomeLogin.Location = new System.Drawing.Point(456, 23);
            this.btnFrmHomeLogin.Name = "btnFrmHomeLogin";
            this.btnFrmHomeLogin.Size = new System.Drawing.Size(64, 21);
            this.btnFrmHomeLogin.TabIndex = 4;
            this.btnFrmHomeLogin.Text = "Login";
            this.btnFrmHomeLogin.UseVisualStyleBackColor = true;
            this.btnFrmHomeLogin.Click += new System.EventHandler(this.btnFrmHomeLogin_Click);
            // 
            // txtFrmHomePassword
            // 
            this.txtFrmHomePassword.Location = new System.Drawing.Point(313, 23);
            this.txtFrmHomePassword.Name = "txtFrmHomePassword";
            this.txtFrmHomePassword.Size = new System.Drawing.Size(121, 20);
            this.txtFrmHomePassword.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(277, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Pass";
            // 
            // txtFrmHomeMaKH
            // 
            this.txtFrmHomeMaKH.Location = new System.Drawing.Point(78, 22);
            this.txtFrmHomeMaKH.Name = "txtFrmHomeMaKH";
            this.txtFrmHomeMaKH.Size = new System.Drawing.Size(169, 20);
            this.txtFrmHomeMaKH.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mã KH";
            // 
            // btnFrmHomeOneDownload
            // 
            this.btnFrmHomeOneDownload.Location = new System.Drawing.Point(155, 88);
            this.btnFrmHomeOneDownload.Name = "btnFrmHomeOneDownload";
            this.btnFrmHomeOneDownload.Size = new System.Drawing.Size(253, 31);
            this.btnFrmHomeOneDownload.TabIndex = 10;
            this.btnFrmHomeOneDownload.Text = "Download lẻ thông báo điện theo mã KH";
            this.btnFrmHomeOneDownload.UseVisualStyleBackColor = true;
            this.btnFrmHomeOneDownload.Click += new System.EventHandler(this.btnFrmHomeOneDownload_Click);
            // 
            // btnFrmHomeMultiDownload
            // 
            this.btnFrmHomeMultiDownload.Location = new System.Drawing.Point(399, 541);
            this.btnFrmHomeMultiDownload.Name = "btnFrmHomeMultiDownload";
            this.btnFrmHomeMultiDownload.Size = new System.Drawing.Size(105, 23);
            this.btnFrmHomeMultiDownload.TabIndex = 11;
            this.btnFrmHomeMultiDownload.Text = "Multiple Download";
            this.btnFrmHomeMultiDownload.UseVisualStyleBackColor = true;
            this.btnFrmHomeMultiDownload.Click += new System.EventHandler(this.btnFrmHomeMultiDownload_Click);
            // 
            // pnlDropExcel
            // 
            this.pnlDropExcel.AllowDrop = true;
            this.pnlDropExcel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDropExcel.Location = new System.Drawing.Point(757, 162);
            this.pnlDropExcel.Name = "pnlDropExcel";
            this.pnlDropExcel.Size = new System.Drawing.Size(137, 29);
            this.pnlDropExcel.TabIndex = 12;
            this.pnlDropExcel.DragDrop += new System.Windows.Forms.DragEventHandler(this.pnlDropExcel_DragDrop);
            this.pnlDropExcel.DragEnter += new System.Windows.Forms.DragEventHandler(this.pnlDropExcel_DragEnter);
            // 
            // dgvFrmHome
            // 
            this.dgvFrmHome.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFrmHome.Location = new System.Drawing.Point(42, 210);
            this.dgvFrmHome.Name = "dgvFrmHome";
            this.dgvFrmHome.Size = new System.Drawing.Size(852, 312);
            this.dgvFrmHome.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(627, 170);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Import file excel vào đây";
            // 
            // btnImportExcelSource
            // 
            this.btnImportExcelSource.Location = new System.Drawing.Point(47, 162);
            this.btnImportExcelSource.Name = "btnImportExcelSource";
            this.btnImportExcelSource.Size = new System.Drawing.Size(179, 23);
            this.btnImportExcelSource.TabIndex = 14;
            this.btnImportExcelSource.Text = "Nhập từ file dữ liệu excel gốc";
            this.btnImportExcelSource.UseVisualStyleBackColor = true;
            this.btnImportExcelSource.Click += new System.EventHandler(this.btnImportExcelSource_Click);
            // 
            // btnFrmHomeCreAccount
            // 
            this.btnFrmHomeCreAccount.BackColor = System.Drawing.SystemColors.Window;
            this.btnFrmHomeCreAccount.Location = new System.Drawing.Point(468, 92);
            this.btnFrmHomeCreAccount.Name = "btnFrmHomeCreAccount";
            this.btnFrmHomeCreAccount.Size = new System.Drawing.Size(107, 23);
            this.btnFrmHomeCreAccount.TabIndex = 15;
            this.btnFrmHomeCreAccount.Text = "New account";
            this.btnFrmHomeCreAccount.UseVisualStyleBackColor = false;
            this.btnFrmHomeCreAccount.Click += new System.EventHandler(this.btnFrmHomeCreAccount_Click);
            // 
            // frmHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(935, 576);
            this.Controls.Add(this.btnFrmHomeCreAccount);
            this.Controls.Add(this.btnImportExcelSource);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dgvFrmHome);
            this.Controls.Add(this.pnlDropExcel);
            this.Controls.Add(this.btnFrmHomeMultiDownload);
            this.Controls.Add(this.btnFrmHomeOneDownload);
            this.Controls.Add(this.pnlFrmHomeLogin);
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
        private System.Windows.Forms.Button btnFrmHomeOneDownload;
        private System.Windows.Forms.Button btnFrmHomeMultiDownload;
        private System.Windows.Forms.Panel pnlDropExcel;
        private System.Windows.Forms.DataGridView dgvFrmHome;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnImportExcelSource;
        private System.Windows.Forms.Button btnFrmHomeCreAccount;
    }
}

