namespace Tamphan_BBP_EVN_WF
{
    partial class frmDownload
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
            this.chromiumdownload = new CefSharp.WinForms.ChromiumWebBrowser();
            this.btnExporttable = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chromiumdownload
            // 
            this.chromiumdownload.ActivateBrowserOnCreation = false;
            this.chromiumdownload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chromiumdownload.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chromiumdownload.Location = new System.Drawing.Point(0, 0);
            this.chromiumdownload.Name = "chromiumdownload";
            this.chromiumdownload.Size = new System.Drawing.Size(800, 450);
            this.chromiumdownload.TabIndex = 0;
            // 
            // btnExporttable
            // 
            this.btnExporttable.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnExporttable.Location = new System.Drawing.Point(27, 30);
            this.btnExporttable.Name = "btnExporttable";
            this.btnExporttable.Size = new System.Drawing.Size(75, 23);
            this.btnExporttable.TabIndex = 1;
            this.btnExporttable.Text = "Xuất bảng";
            this.btnExporttable.UseVisualStyleBackColor = false;
            this.btnExporttable.Click += new System.EventHandler(this.btnExporttable_Click);
            // 
            // frmDownload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnExporttable);
            this.Controls.Add(this.chromiumdownload);
            this.Name = "frmDownload";
            this.Text = "TamphanTools";
            this.ResumeLayout(false);

        }

        #endregion

        private CefSharp.WinForms.ChromiumWebBrowser chromiumdownload;
        private System.Windows.Forms.Button btnExporttable;
    }
}