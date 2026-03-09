namespace Tamphan_BBP_EVN_WF
{
    partial class EVNSPC_WEB_LOGIN
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
            this.weblogin = new CefSharp.WinForms.ChromiumWebBrowser();
            this.SuspendLayout();
            // 
            // weblogin
            // 
            this.weblogin.ActivateBrowserOnCreation = false;
            this.weblogin.Location = new System.Drawing.Point(0, 0);
            this.weblogin.Name = "weblogin";
            this.weblogin.Size = new System.Drawing.Size(1900, 1000);
            this.weblogin.TabIndex = 0;
            // 
            // EVNSPC_WEB_LOGIN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.weblogin);
            this.Name = "EVNSPC_WEB_LOGIN";
            this.Text = "EVNSPC_WEB_LOGIN";
            this.ResumeLayout(false);

        }

        #endregion

        private CefSharp.WinForms.ChromiumWebBrowser weblogin;
    }
}