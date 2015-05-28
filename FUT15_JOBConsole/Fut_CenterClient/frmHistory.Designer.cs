namespace FUT_MonitorCenter
{
    partial class frmHistory
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
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBox5 = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.dgTG = new System.Windows.Forms.DataGridView();
            this.cTradeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cResourceID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCurrentBid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLastSalePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cStartingBid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cBuyNowPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPreferredPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cRating = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cRareFlag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPAC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cSHO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPAS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDRI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDEF = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPHY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cContract = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cFitness = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cFace = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TargetSale = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewButtonColumn1 = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgPlayers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgTG)).BeginInit();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1092, 27);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "Retrieve";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dgPlayers
            // 
            this.dgPlayers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgPlayers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cTradeId,
            this.cResourceID,
            this.cName,
            this.Column2,
            this.cCurrentBid,
            this.cLastSalePrice,
            this.cStartingBid,
            this.cBuyNowPrice,
            this.cPreferredPosition,
            this.cRating,
            this.cRareFlag,
            this.cPAC,
            this.cSHO,
            this.cPAS,
            this.cDRI,
            this.cDEF,
            this.cPHY,
            this.cContract,
            this.cFitness});
            this.dgPlayers.Location = new System.Drawing.Point(12, 61);
            this.dgPlayers.Name = "dgPlayers";
            this.dgPlayers.ReadOnly = true;
            this.dgPlayers.Size = new System.Drawing.Size(1243, 839);
            this.dgPlayers.TabIndex = 9;
            this.dgPlayers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgPlayers_CellContentClick);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(1180, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 54;
            this.button4.Text = "<";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1261, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 53;
            this.button3.Text = ">";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1173, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 52;
            this.button1.Text = "Reset";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(573, 35);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(31, 13);
            this.label10.TabIndex = 51;
            this.label10.Text = "Club:";
            // 
            // comboBox5
            // 
            this.comboBox5.FormattingEnabled = true;
            this.comboBox5.Location = new System.Drawing.Point(616, 31);
            this.comboBox5.Name = "comboBox5";
            this.comboBox5.Size = new System.Drawing.Size(121, 21);
            this.comboBox5.TabIndex = 50;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(555, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 13);
            this.label9.TabIndex = 49;
            this.label9.Text = "Nationality:";
            // 
            // comboBox4
            // 
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(616, 5);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(121, 21);
            this.comboBox4.TabIndex = 48;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(743, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 13);
            this.label8.TabIndex = 47;
            this.label8.Text = "Chemistry Style:";
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(820, 5);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(121, 21);
            this.comboBox3.TabIndex = 46;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(367, 35);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 13);
            this.label7.TabIndex = 45;
            this.label7.Text = "Position:";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(428, 28);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 44;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(367, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 13);
            this.label6.TabIndex = 43;
            this.label6.Text = "Quality:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(428, 5);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 42;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(183, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 41;
            this.label4.Text = "Buy Now Max:";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(259, 33);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(100, 20);
            this.textBox4.TabIndex = 40;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(183, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 39;
            this.label5.Text = "Buy Now Min:";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(259, 7);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(100, 20);
            this.textBox5.TabIndex = 38;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 37;
            this.label3.Text = "Max Price:";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(75, 35);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 36;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 35;
            this.label2.Text = "Min Price:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(75, 9);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 34;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(744, 35);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(70, 13);
            this.label11.TabIndex = 33;
            this.label11.Text = "Player Name:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(820, 29);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(121, 20);
            this.textBox1.TabIndex = 32;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(937, 28);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(27, 23);
            this.button5.TabIndex = 55;
            this.button5.Text = "...";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(965, 30);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(121, 20);
            this.textBox6.TabIndex = 56;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(965, 9);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(76, 17);
            this.checkBox1.TabIndex = 57;
            this.checkBox1.Text = "Recent 50";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // dgTG
            // 
            this.dgTG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgTG.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cFace,
            this.Column8,
            this.Column4,
            this.dataGridViewTextBoxColumn5,
            this.TargetSale,
            this.Column3,
            this.Column7,
            this.Column9,
            this.dataGridViewButtonColumn1});
            this.dgTG.Location = new System.Drawing.Point(1261, 61);
            this.dgTG.Name = "dgTG";
            this.dgTG.Size = new System.Drawing.Size(593, 261);
            this.dgTG.TabIndex = 58;
            // 
            // cTradeId
            // 
            this.cTradeId.HeaderText = "TradeId";
            this.cTradeId.Name = "cTradeId";
            this.cTradeId.ReadOnly = true;
            this.cTradeId.Width = 80;
            // 
            // cResourceID
            // 
            this.cResourceID.HeaderText = "ResourceID";
            this.cResourceID.Name = "cResourceID";
            this.cResourceID.ReadOnly = true;
            this.cResourceID.Width = 80;
            // 
            // cName
            // 
            this.cName.HeaderText = "Name";
            this.cName.Name = "cName";
            this.cName.ReadOnly = true;
            this.cName.Width = 80;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "BidState";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 70;
            // 
            // cCurrentBid
            // 
            this.cCurrentBid.HeaderText = "CurrentBid";
            this.cCurrentBid.Name = "cCurrentBid";
            this.cCurrentBid.ReadOnly = true;
            this.cCurrentBid.Width = 70;
            // 
            // cLastSalePrice
            // 
            this.cLastSalePrice.HeaderText = "LastSalePrice";
            this.cLastSalePrice.Name = "cLastSalePrice";
            this.cLastSalePrice.ReadOnly = true;
            this.cLastSalePrice.Width = 80;
            // 
            // cStartingBid
            // 
            this.cStartingBid.HeaderText = "StartingBid";
            this.cStartingBid.Name = "cStartingBid";
            this.cStartingBid.ReadOnly = true;
            this.cStartingBid.Width = 80;
            // 
            // cBuyNowPrice
            // 
            this.cBuyNowPrice.HeaderText = "BuyNowPrice";
            this.cBuyNowPrice.Name = "cBuyNowPrice";
            this.cBuyNowPrice.ReadOnly = true;
            this.cBuyNowPrice.Width = 80;
            // 
            // cPreferredPosition
            // 
            this.cPreferredPosition.HeaderText = "Position";
            this.cPreferredPosition.Name = "cPreferredPosition";
            this.cPreferredPosition.ReadOnly = true;
            this.cPreferredPosition.Width = 50;
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
            this.cRareFlag.Width = 60;
            // 
            // cPAC
            // 
            this.cPAC.HeaderText = "PAC";
            this.cPAC.Name = "cPAC";
            this.cPAC.ReadOnly = true;
            this.cPAC.Width = 50;
            // 
            // cSHO
            // 
            this.cSHO.HeaderText = "SHO";
            this.cSHO.Name = "cSHO";
            this.cSHO.ReadOnly = true;
            this.cSHO.Width = 50;
            // 
            // cPAS
            // 
            this.cPAS.HeaderText = "PAS";
            this.cPAS.Name = "cPAS";
            this.cPAS.ReadOnly = true;
            this.cPAS.Width = 50;
            // 
            // cDRI
            // 
            this.cDRI.HeaderText = "DRI";
            this.cDRI.Name = "cDRI";
            this.cDRI.ReadOnly = true;
            this.cDRI.Width = 50;
            // 
            // cDEF
            // 
            this.cDEF.HeaderText = "DEF";
            this.cDEF.Name = "cDEF";
            this.cDEF.ReadOnly = true;
            this.cDEF.Width = 50;
            // 
            // cPHY
            // 
            this.cPHY.HeaderText = "PHY";
            this.cPHY.Name = "cPHY";
            this.cPHY.ReadOnly = true;
            this.cPHY.Width = 50;
            // 
            // cContract
            // 
            this.cContract.HeaderText = "Contract";
            this.cContract.Name = "cContract";
            this.cContract.ReadOnly = true;
            this.cContract.Width = 50;
            // 
            // cFitness
            // 
            this.cFitness.HeaderText = "Fitness";
            this.cFitness.Name = "cFitness";
            this.cFitness.ReadOnly = true;
            this.cFitness.Width = 50;
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
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "TgtBuyAt";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 80;
            // 
            // TargetSale
            // 
            this.TargetSale.HeaderText = "TgtSaleMin";
            this.TargetSale.Name = "TargetSale";
            this.TargetSale.Width = 80;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "TgtSaleMax";
            this.Column3.Name = "Column3";
            this.Column3.Width = 80;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "TgtSaleMargin";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Width = 80;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Count";
            this.Column9.Name = "Column9";
            this.Column9.Width = 10;
            // 
            // dataGridViewButtonColumn1
            // 
            this.dataGridViewButtonColumn1.HeaderText = "Save";
            this.dataGridViewButtonColumn1.Name = "dataGridViewButtonColumn1";
            this.dataGridViewButtonColumn1.Text = "Update";
            this.dataGridViewButtonColumn1.UseColumnTextForButtonValue = true;
            this.dataGridViewButtonColumn1.Width = 75;
            // 
            // frmHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1866, 912);
            this.Controls.Add(this.dgTG);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.comboBox5);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.comboBox4);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.dgPlayers);
            this.Name = "frmHistory";
            this.Text = "frmHistory";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmHistory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgPlayers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgTG)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dgPlayers;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboBox5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.DataGridView dgTG;
        private System.Windows.Forms.DataGridViewTextBoxColumn cTradeId;
        private System.Windows.Forms.DataGridViewTextBoxColumn cResourceID;
        private System.Windows.Forms.DataGridViewTextBoxColumn cName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn cCurrentBid;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLastSalePrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn cStartingBid;
        private System.Windows.Forms.DataGridViewTextBoxColumn cBuyNowPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPreferredPosition;
        private System.Windows.Forms.DataGridViewTextBoxColumn cRating;
        private System.Windows.Forms.DataGridViewTextBoxColumn cRareFlag;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPAC;
        private System.Windows.Forms.DataGridViewTextBoxColumn cSHO;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPAS;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDRI;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDEF;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPHY;
        private System.Windows.Forms.DataGridViewTextBoxColumn cContract;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFitness;
        private System.Windows.Forms.DataGridViewImageColumn cFace;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column8;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn TargetSale;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewButtonColumn dataGridViewButtonColumn1;
    }
}