namespace Tamphan_BBP_EVN_WF
{
    partial class frmEVNLogin
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
            this.chromiumlogin = new CefSharp.WinForms.ChromiumWebBrowser();
            this.btn_changepassword = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chromiumlogin
            // 
            this.chromiumlogin.ActivateBrowserOnCreation = false;
            this.chromiumlogin.Location = new System.Drawing.Point(0, 0);
            this.chromiumlogin.Name = "chromiumlogin";
            this.chromiumlogin.Size = new System.Drawing.Size(1900, 1000);
            this.chromiumlogin.TabIndex = 0;
            // 
            // btn_changepassword
            // 
            this.btn_changepassword.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btn_changepassword.Location = new System.Drawing.Point(21, 24);
            this.btn_changepassword.Name = "btn_changepassword";
            this.btn_changepassword.Size = new System.Drawing.Size(107, 23);
            this.btn_changepassword.TabIndex = 1;
            this.btn_changepassword.Text = "Change password";
            this.btn_changepassword.UseVisualStyleBackColor = false;
            this.btn_changepassword.Click += new System.EventHandler(this.btn_changepassword_Click);
            // 
            // frmEVNLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_changepassword);
            this.Controls.Add(this.chromiumlogin);
            this.Name = "frmEVNLogin";
            this.Text = "TamphanTools";
            this.ResumeLayout(false);

        }

        #endregion

        private CefSharp.WinForms.ChromiumWebBrowser chromiumlogin;
        private System.Windows.Forms.Button btn_changepassword;
    }
}