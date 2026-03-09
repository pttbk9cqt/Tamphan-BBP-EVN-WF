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
            this.btn_login_download_no_UI = new System.Windows.Forms.Button();
            this.btn_evn_download = new System.Windows.Forms.Button();
            this.panel_account_lẻ.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_account_lẻ
            // 
            this.panel_account_lẻ.Controls.Add(this.button_Login_account_riêng_lẻ);
            this.panel_account_lẻ.Controls.Add(this.textBox_password);
            this.panel_account_lẻ.Controls.Add(this.label2);
            this.panel_account_lẻ.Controls.Add(this.textBox_nhập_mã_khách_hàng);
            this.panel_account_lẻ.Controls.Add(this.label1);
            this.panel_account_lẻ.Location = new System.Drawing.Point(58, 38);
            this.panel_account_lẻ.Name = "panel_account_lẻ";
            this.panel_account_lẻ.Size = new System.Drawing.Size(251, 60);
            this.panel_account_lẻ.TabIndex = 4;
            // 
            // button_Login_account_riêng_lẻ
            // 
            this.button_Login_account_riêng_lẻ.Location = new System.Drawing.Point(162, 30);
            this.button_Login_account_riêng_lẻ.Name = "button_Login_account_riêng_lẻ";
            this.button_Login_account_riêng_lẻ.Size = new System.Drawing.Size(64, 21);
            this.button_Login_account_riêng_lẻ.TabIndex = 4;
            this.button_Login_account_riêng_lẻ.Text = "Login";
            this.button_Login_account_riêng_lẻ.UseVisualStyleBackColor = true;
            this.button_Login_account_riêng_lẻ.Click += new System.EventHandler(this.Btn_Login_account_riêng_lẻ_Click);
            // 
            // textBox_password
            // 
            this.textBox_password.Location = new System.Drawing.Point(58, 31);
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.Size = new System.Drawing.Size(100, 20);
            this.textBox_password.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Pass";
            // 
            // textBox_nhập_mã_khách_hàng
            // 
            this.textBox_nhập_mã_khách_hàng.Location = new System.Drawing.Point(57, 5);
            this.textBox_nhập_mã_khách_hàng.Name = "textBox_nhập_mã_khách_hàng";
            this.textBox_nhập_mã_khách_hàng.Size = new System.Drawing.Size(169, 20);
            this.textBox_nhập_mã_khách_hàng.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mã KH";
            // 
            // btn_login_download_no_UI
            // 
            this.btn_login_download_no_UI.Location = new System.Drawing.Point(58, 119);
            this.btn_login_download_no_UI.Name = "btn_login_download_no_UI";
            this.btn_login_download_no_UI.Size = new System.Drawing.Size(251, 28);
            this.btn_login_download_no_UI.TabIndex = 5;
            this.btn_login_download_no_UI.Text = "Download no UI";
            this.btn_login_download_no_UI.UseVisualStyleBackColor = true;
            this.btn_login_download_no_UI.Click += new System.EventHandler(this.Btn_login_download_no_UI_Click);
            // 
            // btn_evn_download
            // 
            this.btn_evn_download.Location = new System.Drawing.Point(60, 153);
            this.btn_evn_download.Name = "btn_evn_download";
            this.btn_evn_download.Size = new System.Drawing.Size(249, 31);
            this.btn_evn_download.TabIndex = 10;
            this.btn_evn_download.Text = "Download thông báo";
            this.btn_evn_download.UseVisualStyleBackColor = true;
            this.btn_evn_download.Click += new System.EventHandler(this.btn_evn_download_Click);
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 279);
            this.Controls.Add(this.btn_evn_download);
            this.Controls.Add(this.btn_login_download_no_UI);
            this.Controls.Add(this.panel_account_lẻ);
            this.Name = "Home";
            this.Text = "Tamphan";
            this.panel_account_lẻ.ResumeLayout(false);
            this.panel_account_lẻ.PerformLayout();
            this.ResumeLayout(false);

        }


        #endregion
        private System.Windows.Forms.Panel panel_account_lẻ;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_nhập_mã_khách_hàng;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.Button button_Login_account_riêng_lẻ;
        private System.Windows.Forms.Button btn_login_download_no_UI;
        private System.Windows.Forms.Button btn_evn_download;
    }
}

