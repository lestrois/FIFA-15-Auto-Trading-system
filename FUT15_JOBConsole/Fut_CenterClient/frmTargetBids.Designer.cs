namespace FUT_MonitorCenter
{
    partial class frmTargetBids
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
            this.button2 = new System.Windows.Forms.Button();
            this.dgPlayers = new System.Windows.Forms.DataGridView();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cResourceID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cFace = new System.Windows.Forms.DataGridViewImageColumn();
            this.cRating = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cRareFlag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TargetSale = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.txtResourceID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgPlayers)).BeginInit();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(197, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(106, 23);
            this.button2.TabIndex = 12;
            this.button2.Text = "Retrieve";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dgPlayers
            // 
            this.dgPlayers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgPlayers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column5,
            this.cResourceID,
            this.cName,
            this.cFace,
            this.cRating,
            this.cRareFlag,
            this.Column8,
            this.Column4,
            this.Column1,
            this.Column6,
            this.TargetSale,
            this.Column3,
            this.Column7,
            this.Column9,
            this.Column2});
            this.dgPlayers.Location = new System.Drawing.Point(12, 33);
            this.dgPlayers.Name = "dgPlayers";
            this.dgPlayers.Size = new System.Drawing.Size(1282, 756);
            this.dgPlayers.TabIndex = 11;
            this.dgPlayers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgPlayers_CellContentClick);
            // 
            // Column5
            // 
            this.Column5.HeaderText = "id";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Visible = false;
            // 
            // cResourceID
            // 
            this.cResourceID.HeaderText = "ResourceID";
            this.cResourceID.Name = "cResourceID";
            this.cResourceID.ReadOnly = true;
            // 
            // cName
            // 
            this.cName.HeaderText = "Name";
            this.cName.Name = "cName";
            // 
            // cFace
            // 
            this.cFace.HeaderText = "Face";
            this.cFace.Name = "cFace";
            this.cFace.ReadOnly = true;
            this.cFace.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cFace.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cFace.Width = 90;
            // 
            // cRating
            // 
            this.cRating.HeaderText = "Rating";
            this.cRating.Name = "cRating";
            this.cRating.ReadOnly = true;
            this.cRating.Width = 50;
            // 
            // cRareFlag
            // 
            this.cRareFlag.HeaderText = "RareFlag";
            this.cRareFlag.Name = "cRareFlag";
            this.cRareFlag.ReadOnly = true;
            this.cRareFlag.Width = 70;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Updatable";
            this.Column8.Name = "Column8";
            this.Column8.Width = 60;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Biddable";
            this.Column4.Name = "Column4";
            this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column4.Width = 60;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "TargetBuyAt";
            this.Column1.Name = "Column1";
            // 
            // Column6
            // 
            this.Column6.HeaderText = "TargetBuyAmount";
            this.Column6.Name = "Column6";
            // 
            // TargetSale
            // 
            this.TargetSale.HeaderText = "TargetSaleMin";
            this.TargetSale.Name = "TargetSale";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "TargetSaleMax";
            this.Column3.Name = "Column3";
            // 
            // Column7
            // 
            this.Column7.HeaderText = "TargetSaleMargin";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Count";
            this.Column9.Name = "Column9";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Save";
            this.Column2.Name = "Column2";
            this.Column2.Text = "Update";
            this.Column2.UseColumnTextForButtonValue = true;
            this.Column2.Width = 75;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(421, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Auto Calculation";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(309, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(106, 23);
            this.button3.TabIndex = 14;
            this.button3.Text = "Save All";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // txtResourceID
            // 
            this.txtResourceID.Location = new System.Drawing.Point(83, 6);
            this.txtResourceID.Name = "txtResourceID";
            this.txtResourceID.Size = new System.Drawing.Size(100, 20);
            this.txtResourceID.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "ResourceID";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(672, 3);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 17;
            this.btnTest.Text = "test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Visible = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // frmTargetBids
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1542, 816);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtResourceID);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.dgPlayers);
            this.Name = "frmTargetBids";
            this.Text = "frmTargetBids";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dgPlayers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dgPlayers;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn cResourceID;
        private System.Windows.Forms.DataGridViewTextBoxColumn cName;
        private System.Windows.Forms.DataGridViewImageColumn cFace;
        private System.Windows.Forms.DataGridViewTextBoxColumn cRating;
        private System.Windows.Forms.DataGridViewTextBoxColumn cRareFlag;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column8;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn TargetSale;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewButtonColumn Column2;
        private System.Windows.Forms.TextBox txtResourceID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnTest;
    }
}