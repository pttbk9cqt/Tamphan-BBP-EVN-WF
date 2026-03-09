namespace Tamphan_BBP_EVN_WF
{
    partial class EVNSPC_DownloadThongbao
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
            this.evndownload = new CefSharp.WinForms.ChromiumWebBrowser();
            this.btn_exporttable = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // evndownload
            // 
            this.evndownload.ActivateBrowserOnCreation = false;
            this.evndownload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.evndownload.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.evndownload.Location = new System.Drawing.Point(0, 0);
            this.evndownload.Name = "evndownload";
            this.evndownload.Size = new System.Drawing.Size(800, 450);
            this.evndownload.TabIndex = 0;
            // 
            // btn_exporttable
            // 
            this.btn_exporttable.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_exporttable.Location = new System.Drawing.Point(27, 30);
            this.btn_exporttable.Name = "btn_exporttable";
            this.btn_exporttable.Size = new System.Drawing.Size(75, 23);
            this.btn_exporttable.TabIndex = 1;
            this.btn_exporttable.Text = "Xuất bảng";
            this.btn_exporttable.UseVisualStyleBackColor = false;
            this.btn_exporttable.Click += new System.EventHandler(this.btn_exporttable_Click);
            // 
            // EVNSPC_DownloadThongbao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_exporttable);
            this.Controls.Add(this.evndownload);
            this.Name = "EVNSPC_DownloadThongbao";
            this.Text = "EVNSPC_DownloadThongbao";
            this.ResumeLayout(false);

        }

        #endregion

        private CefSharp.WinForms.ChromiumWebBrowser evndownload;
        private System.Windows.Forms.Button btn_exporttable;
    }
}