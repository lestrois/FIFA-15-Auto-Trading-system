namespace FUT_MonitorCenter
{
    partial class frmLoginSession
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
            this.gbLogin = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtSecondToken = new System.Windows.Forms.TextBox();
            this.txtSecondSessionID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnSessionLogin = new System.Windows.Forms.Button();
            this.txtToken = new System.Windows.Forms.TextBox();
            this.txtSessionID = new System.Windows.Forms.TextBox();
            this.lblToken = new System.Windows.Forms.Label();
            this.lblSessionID = new System.Windows.Forms.Label();
            this.gbLogin.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbLogin
            // 
            this.gbLogin.Controls.Add(this.button1);
            this.gbLogin.Controls.Add(this.txtSecondToken);
            this.gbLogin.Controls.Add(this.txtSecondSessionID);
            this.gbLogin.Controls.Add(this.label1);
            this.gbLogin.Controls.Add(this.label2);
            this.gbLogin.Controls.Add(this.lblStatus);
            this.gbLogin.Controls.Add(this.btnSessionLogin);
            this.gbLogin.Controls.Add(this.txtToken);
            this.gbLogin.Controls.Add(this.txtSessionID);
            this.gbLogin.Controls.Add(this.lblToken);
            this.gbLogin.Controls.Add(this.lblSessionID);
            this.gbLogin.Location = new System.Drawing.Point(1, 2);
            this.gbLogin.Name = "gbLogin";
            this.gbLogin.Size = new System.Drawing.Size(368, 100);
            this.gbLogin.TabIndex = 14;
            this.gbLogin.TabStop = false;
            this.gbLogin.Text = "Session Sign In";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(20, 161);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(151, 26);
            this.button1.TabIndex = 20;
            this.button1.Text = "Log In by second SessionID";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtSecondToken
            // 
            this.txtSecondToken.Location = new System.Drawing.Point(102, 135);
            this.txtSecondToken.Name = "txtSecondToken";
            this.txtSecondToken.Size = new System.Drawing.Size(247, 20);
            this.txtSecondToken.TabIndex = 19;
            this.txtSecondToken.Visible = false;
            // 
            // txtSecondSessionID
            // 
            this.txtSecondSessionID.Location = new System.Drawing.Point(102, 114);
            this.txtSecondSessionID.Name = "txtSecondSessionID";
            this.txtSecondSessionID.Size = new System.Drawing.Size(247, 20);
            this.txtSecondSessionID.TabIndex = 18;
            this.txtSecondSessionID.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 135);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Phishing Token:";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Session ID:";
            this.label2.Visible = false;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(162, 67);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 15;
            // 
            // btnSessionLogin
            // 
            this.btnSessionLogin.Location = new System.Drawing.Point(20, 66);
            this.btnSessionLogin.Name = "btnSessionLogin";
            this.btnSessionLogin.Size = new System.Drawing.Size(117, 26);
            this.btnSessionLogin.TabIndex = 14;
            this.btnSessionLogin.Text = "Log In by SessionID";
            this.btnSessionLogin.UseVisualStyleBackColor = true;
            this.btnSessionLogin.Click += new System.EventHandler(this.btnSessionLogin_Click);
            // 
            // txtToken
            // 
            this.txtToken.Location = new System.Drawing.Point(102, 40);
            this.txtToken.Name = "txtToken";
            this.txtToken.Size = new System.Drawing.Size(247, 20);
            this.txtToken.TabIndex = 13;
            // 
            // txtSessionID
            // 
            this.txtSessionID.Location = new System.Drawing.Point(102, 19);
            this.txtSessionID.Name = "txtSessionID";
            this.txtSessionID.Size = new System.Drawing.Size(247, 20);
            this.txtSessionID.TabIndex = 12;
            // 
            // lblToken
            // 
            this.lblToken.AutoSize = true;
            this.lblToken.Location = new System.Drawing.Point(17, 40);
            this.lblToken.Name = "lblToken";
            this.lblToken.Size = new System.Drawing.Size(84, 13);
            this.lblToken.TabIndex = 2;
            this.lblToken.Text = "Phishing Token:";
            // 
            // lblSessionID
            // 
            this.lblSessionID.AutoSize = true;
            this.lblSessionID.Location = new System.Drawing.Point(17, 23);
            this.lblSessionID.Name = "lblSessionID";
            this.lblSessionID.Size = new System.Drawing.Size(61, 13);
            this.lblSessionID.TabIndex = 1;
            this.lblSessionID.Text = "Session ID:";
            // 
            // frmLoginSession
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 106);
            this.Controls.Add(this.gbLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmLoginSession";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmLoginSession";
            this.Load += new System.EventHandler(this.frmLoginSession_Load);
            this.gbLogin.ResumeLayout(false);
            this.gbLogin.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbLogin;
        private System.Windows.Forms.Button btnSessionLogin;
        private System.Windows.Forms.TextBox txtToken;
        private System.Windows.Forms.TextBox txtSessionID;
        private System.Windows.Forms.Label lblToken;
        private System.Windows.Forms.Label lblSessionID;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtSecondToken;
        private System.Windows.Forms.TextBox txtSecondSessionID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}