namespace Tamphan_BBP_EVN_WF
{
    partial class frmDownloadFullYear
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
            this.chromiumdownloadfullyear = new CefSharp.WinForms.ChromiumWebBrowser();
            this.SuspendLayout();
            // 
            // chromiumdownloadfullyear
            // 
            this.chromiumdownloadfullyear.ActivateBrowserOnCreation = false;
            this.chromiumdownloadfullyear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chromiumdownloadfullyear.Location = new System.Drawing.Point(0, 0);
            this.chromiumdownloadfullyear.Name = "chromiumdownloadfullyear";
            this.chromiumdownloadfullyear.Size = new System.Drawing.Size(800, 450);
            this.chromiumdownloadfullyear.TabIndex = 0;
            // 
            // frmDownloadFullYear
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.chromiumdownloadfullyear);
            this.Name = "frmDownloadFullYear";
            this.Text = "frmDownloadFullYear";
            this.ResumeLayout(false);

        }

        #endregion

        private CefSharp.WinForms.ChromiumWebBrowser chromiumdownloadfullyear;
    }
}