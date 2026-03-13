namespace Tamphan_BBP_EVN_WF
{
    partial class EVN_NewAccount
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
            this.chrome_newaccount = new CefSharp.WinForms.ChromiumWebBrowser();
            this.btn_creaccount_fillinfo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chrome_newaccount
            // 
            this.chrome_newaccount.ActivateBrowserOnCreation = false;
            this.chrome_newaccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chrome_newaccount.Location = new System.Drawing.Point(0, 0);
            this.chrome_newaccount.Name = "chrome_newaccount";
            this.chrome_newaccount.Size = new System.Drawing.Size(800, 450);
            this.chrome_newaccount.TabIndex = 0;
            // 
            // btn_creaccount_fillinfo
            // 
            this.btn_creaccount_fillinfo.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btn_creaccount_fillinfo.Location = new System.Drawing.Point(28, 24);
            this.btn_creaccount_fillinfo.Name = "btn_creaccount_fillinfo";
            this.btn_creaccount_fillinfo.Size = new System.Drawing.Size(75, 23);
            this.btn_creaccount_fillinfo.TabIndex = 1;
            this.btn_creaccount_fillinfo.Text = "Fill Info Account";
            this.btn_creaccount_fillinfo.UseVisualStyleBackColor = false;
            this.btn_creaccount_fillinfo.Click += new System.EventHandler(this.btn_creaccount_fillinfo_Click);
            // 
            // EVN_NewAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_creaccount_fillinfo);
            this.Controls.Add(this.chrome_newaccount);
            this.Name = "EVN_NewAccount";
            this.Text = "EVN_NewAccount";
            this.ResumeLayout(false);

        }

        #endregion

        private CefSharp.WinForms.ChromiumWebBrowser chrome_newaccount;
        private System.Windows.Forms.Button btn_creaccount_fillinfo;
    }
}