namespace Tamphan_BBP_EVN_WF
{
    partial class Auto_Download
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
            this.webhidden = new CefSharp.WinForms.ChromiumWebBrowser();
            this.SuspendLayout();
            // 
            // webhidden
            // 
            this.webhidden.ActivateBrowserOnCreation = false;
            this.webhidden.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webhidden.Location = new System.Drawing.Point(0, 0);
            this.webhidden.Name = "webhidden";
            this.webhidden.Size = new System.Drawing.Size(729, 465);
            this.webhidden.TabIndex = 0;
            // 
            // EVNSPC_WEB_Hidden
            // 
            this.ClientSize = new System.Drawing.Size(729, 465);
            this.Controls.Add(this.webhidden);
            this.Name = "EVNSPC_WEB_Hidden";
            this.ResumeLayout(false);

        }

        #endregion
        private CefSharp.WinForms.ChromiumWebBrowser webhidden;
    }
}