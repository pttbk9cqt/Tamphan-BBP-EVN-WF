namespace Tamphan_BBP_EVN_WF
{
    partial class frmCreAcc
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
            this.chromiumnewaccount = new CefSharp.WinForms.ChromiumWebBrowser();
            this.btn_creaccount_fillinfo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chromiumnewaccount
            // 
            this.chromiumnewaccount.ActivateBrowserOnCreation = false;
            this.chromiumnewaccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chromiumnewaccount.Location = new System.Drawing.Point(0, 0);
            this.chromiumnewaccount.Name = "chromiumnewaccount";
            this.chromiumnewaccount.Size = new System.Drawing.Size(800, 450);
            this.chromiumnewaccount.TabIndex = 0;
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
            // frmCreAcc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_creaccount_fillinfo);
            this.Controls.Add(this.chromiumnewaccount);
            this.Name = "frmCreAcc";
            this.Text = "TamphanTools";
            this.ResumeLayout(false);

        }

        #endregion

        private CefSharp.WinForms.ChromiumWebBrowser chromiumnewaccount;
        private System.Windows.Forms.Button btn_creaccount_fillinfo;
    }
}