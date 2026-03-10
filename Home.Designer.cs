using System;

namespace Tamphan_BBP_EVN_WF
{
    partial class Home
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
            this.panel_account_lẻ = new System.Windows.Forms.Panel();
            this.button_Login_account_riêng_lẻ = new System.Windows.Forms.Button();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_nhập_mã_khách_hàng = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_evn_download = new System.Windows.Forms.Button();
            this.btn_multidownload = new System.Windows.Forms.Button();
            this.panelDropExcel = new System.Windows.Forms.Panel();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.panel_account_lẻ.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_account_lẻ
            // 
            this.panel_account_lẻ.Controls.Add(this.button_Login_account_riêng_lẻ);
            this.panel_account_lẻ.Controls.Add(this.textBox_password);
            this.panel_account_lẻ.Controls.Add(this.label2);
            this.panel_account_lẻ.Controls.Add(this.textBox_nhập_mã_khách_hàng);
            this.panel_account_lẻ.Controls.Add(this.label1);
            this.panel_account_lẻ.Location = new System.Drawing.Point(47, 12);
            this.panel_account_lẻ.Name = "panel_account_lẻ";
            this.panel_account_lẻ.Size = new System.Drawing.Size(515, 60);
            this.panel_account_lẻ.TabIndex = 4;
            // 
            // button_Login_account_riêng_lẻ
            // 
            this.button_Login_account_riêng_lẻ.Location = new System.Drawing.Point(421, 20);
            this.button_Login_account_riêng_lẻ.Name = "button_Login_account_riêng_lẻ";
            this.button_Login_account_riêng_lẻ.Size = new System.Drawing.Size(64, 21);
            this.button_Login_account_riêng_lẻ.TabIndex = 4;
            this.button_Login_account_riêng_lẻ.Text = "Login";
            this.button_Login_account_riêng_lẻ.UseVisualStyleBackColor = true;
            this.button_Login_account_riêng_lẻ.Click += new System.EventHandler(this.Btn_Login_account_riêng_lẻ_Click);
            // 
            // textBox_password
            // 
            this.textBox_password.Location = new System.Drawing.Point(273, 20);
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.Size = new System.Drawing.Size(121, 20);
            this.textBox_password.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(237, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Pass";
            // 
            // textBox_nhập_mã_khách_hàng
            // 
            this.textBox_nhập_mã_khách_hàng.Location = new System.Drawing.Point(57, 19);
            this.textBox_nhập_mã_khách_hàng.Name = "textBox_nhập_mã_khách_hàng";
            this.textBox_nhập_mã_khách_hàng.Size = new System.Drawing.Size(169, 20);
            this.textBox_nhập_mã_khách_hàng.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mã KH";
            // 
            // btn_evn_download
            // 
            this.btn_evn_download.Location = new System.Drawing.Point(199, 89);
            this.btn_evn_download.Name = "btn_evn_download";
            this.btn_evn_download.Size = new System.Drawing.Size(253, 31);
            this.btn_evn_download.TabIndex = 10;
            this.btn_evn_download.Text = "Download lẻ thông báo điện theo mã KH";
            this.btn_evn_download.UseVisualStyleBackColor = true;
            this.btn_evn_download.Click += new System.EventHandler(this.btn_evn_download_Click);
            // 
            // btn_multidownload
            // 
            this.btn_multidownload.Location = new System.Drawing.Point(248, 541);
            this.btn_multidownload.Name = "btn_multidownload";
            this.btn_multidownload.Size = new System.Drawing.Size(105, 23);
            this.btn_multidownload.TabIndex = 11;
            this.btn_multidownload.Text = "Multiple Download";
            this.btn_multidownload.UseVisualStyleBackColor = true;
            // 
            // panelDropExcel
            // 
            this.panelDropExcel.AllowDrop = true;
            this.panelDropExcel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDropExcel.Location = new System.Drawing.Point(201, 163);
            this.panelDropExcel.Name = "panelDropExcel";
            this.panelDropExcel.Size = new System.Drawing.Size(247, 29);
            this.panelDropExcel.TabIndex = 12;
            this.panelDropExcel.DragDrop += new System.Windows.Forms.DragEventHandler(this.panelDropExcel_DragDrop);
            this.panelDropExcel.DragEnter += new System.Windows.Forms.DragEventHandler(this.panelDropExcel_DragEnter);
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(42, 210);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(520, 312);
            this.dataGridView.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(75, 172);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Thả file excel vào đây";
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 576);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.panelDropExcel);
            this.Controls.Add(this.btn_multidownload);
            this.Controls.Add(this.btn_evn_download);
            this.Controls.Add(this.panel_account_lẻ);
            this.Name = "Home";
            this.Text = "Tamphan";
            this.panel_account_lẻ.ResumeLayout(false);
            this.panel_account_lẻ.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion
        private System.Windows.Forms.Panel panel_account_lẻ;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_nhập_mã_khách_hàng;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.Button button_Login_account_riêng_lẻ;
        private System.Windows.Forms.Button btn_evn_download;
        private System.Windows.Forms.Button btn_multidownload;
        private System.Windows.Forms.Panel panelDropExcel;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Label label3;
    }
}

