namespace Tamphan_BBP_EVN_WF
{
    partial class Login
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_loginfrm_username = new System.Windows.Forms.TextBox();
            this.textBox_loginfrm_password = new System.Windows.Forms.TextBox();
            this.btn_loginfrm_login = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(53, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Password";
            // 
            // textBox_loginfrm_username
            // 
            this.textBox_loginfrm_username.Location = new System.Drawing.Point(111, 56);
            this.textBox_loginfrm_username.Name = "textBox_loginfrm_username";
            this.textBox_loginfrm_username.Size = new System.Drawing.Size(208, 20);
            this.textBox_loginfrm_username.TabIndex = 2;
            // 
            // textBox_loginfrm_password
            // 
            this.textBox_loginfrm_password.Location = new System.Drawing.Point(112, 91);
            this.textBox_loginfrm_password.Name = "textBox_loginfrm_password";
            this.textBox_loginfrm_password.Size = new System.Drawing.Size(208, 20);
            this.textBox_loginfrm_password.TabIndex = 3;
            // 
            // btn_loginfrm_login
            // 
            this.btn_loginfrm_login.Location = new System.Drawing.Point(165, 135);
            this.btn_loginfrm_login.Name = "btn_loginfrm_login";
            this.btn_loginfrm_login.Size = new System.Drawing.Size(75, 23);
            this.btn_loginfrm_login.TabIndex = 4;
            this.btn_loginfrm_login.Text = "Login";
            this.btn_loginfrm_login.UseVisualStyleBackColor = true;
            this.btn_loginfrm_login.Click += new System.EventHandler(this.btn_loginfrm_login_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 215);
            this.Controls.Add(this.btn_loginfrm_login);
            this.Controls.Add(this.textBox_loginfrm_password);
            this.Controls.Add(this.textBox_loginfrm_username);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Login";
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_loginfrm_username;
        private System.Windows.Forms.TextBox textBox_loginfrm_password;
        private System.Windows.Forms.Button btn_loginfrm_login;
    }
}