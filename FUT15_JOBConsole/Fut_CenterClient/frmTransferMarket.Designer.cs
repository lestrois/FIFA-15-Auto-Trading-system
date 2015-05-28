namespace FUT_MonitorCenter
{
    partial class frmTransferMarket
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
            this.button2 = new System.Windows.Forms.Button();
            this.lstLog1 = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cTradeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cResourceID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cFace = new System.Windows.Forms.DataGridViewImageColumn();
            this.cCurrentBid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLastSalePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cStartingBid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cBuyNowPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.AddWatch = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgPlayers)).BeginInit();
            this.SuspendLayout();
            // 
            // dgPlayers
            // 
            this.dgPlayers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgPlayers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cTradeId,
            this.cResourceID,
            this.cName,
            this.cFace,
            this.cCurrentBid,
            this.cLastSalePrice,
            this.cStartingBid,
            this.cBuyNowPrice,
            this.Column2,
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
            this.cFitness,
            this.AddWatch,
            this.Column1});
            this.dgPlayers.Location = new System.Drawing.Point(12, 59);
            this.dgPlayers.Name = "dgPlayers";
            this.dgPlayers.Size = new System.Drawing.Size(1050, 960);
            this.dgPlayers.TabIndex = 31;
            this.dgPlayers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgPlayers_CellContentClick);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(1113, 31);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 78;
            this.button4.Text = "<";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1194, 31);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 77;
            this.button3.Text = ">";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(953, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 76;
            this.button1.Text = "Reset";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(566, 33);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(31, 13);
            this.label10.TabIndex = 75;
            this.label10.Text = "Club:";
            // 
            // comboBox5
            // 
            this.comboBox5.FormattingEnabled = true;
            this.comboBox5.Location = new System.Drawing.Point(609, 29);
            this.comboBox5.Name = "comboBox5";
            this.comboBox5.Size = new System.Drawing.Size(121, 21);
            this.comboBox5.TabIndex = 74;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(548, 7);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 13);
            this.label9.TabIndex = 73;
            this.label9.Text = "Nationality:";
            // 
            // comboBox4
            // 
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(609, 3);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(121, 21);
            this.comboBox4.TabIndex = 72;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(736, 7);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 13);
            this.label8.TabIndex = 71;
            this.label8.Text = "Chemistry Style:";
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(813, 3);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(121, 21);
            this.comboBox3.TabIndex = 70;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(360, 33);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 13);
            this.label7.TabIndex = 69;
            this.label7.Text = "Position:";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(421, 26);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 68;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(360, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 13);
            this.label6.TabIndex = 67;
            this.label6.Text = "Quality:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(421, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 66;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(176, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 65;
            this.label4.Text = "Buy Now Max:";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(252, 31);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(100, 20);
            this.textBox4.TabIndex = 64;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(176, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 63;
            this.label5.Text = "Buy Now Min:";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(252, 5);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(100, 20);
            this.textBox5.TabIndex = 62;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 61;
            this.label3.Text = "Max Price:";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(68, 33);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 60;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 59;
            this.label2.Text = "Min Price:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(68, 7);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 58;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(737, 33);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(70, 13);
            this.label11.TabIndex = 57;
            this.label11.Text = "Player Name:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(813, 27);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(121, 20);
            this.textBox1.TabIndex = 56;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(953, 26);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 55;
            this.button2.Text = "Retrieve";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // lstLog1
            // 
            this.lstLog1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.lstLog1.GridLines = true;
            this.lstLog1.Location = new System.Drawing.Point(1068, 59);
            this.lstLog1.Name = "lstLog1";
            this.lstLog1.Size = new System.Drawing.Size(602, 960);
            this.lstLog1.TabIndex = 79;
            this.lstLog1.UseCompatibleStateImageBehavior = false;
            this.lstLog1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "BID LOG";
            this.columnHeader2.Width = 577;
            // 
            // cTradeId
            // 
            this.cTradeId.HeaderText = "TradeId";
            this.cTradeId.Name = "cTradeId";
            // 
            // cResourceID
            // 
            this.cResourceID.HeaderText = "ResourceID";
            this.cResourceID.Name = "cResourceID";
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
            this.cFace.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cFace.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cFace.Width = 90;
            // 
            // cCurrentBid
            // 
            this.cCurrentBid.HeaderText = "CurrentBid";
            this.cCurrentBid.Name = "cCurrentBid";
            this.cCurrentBid.Width = 80;
            // 
            // cLastSalePrice
            // 
            this.cLastSalePrice.HeaderText = "LastSalePrice";
            this.cLastSalePrice.Name = "cLastSalePrice";
            this.cLastSalePrice.Visible = false;
            this.cLastSalePrice.Width = 80;
            // 
            // cStartingBid
            // 
            this.cStartingBid.HeaderText = "StartingBid";
            this.cStartingBid.Name = "cStartingBid";
            this.cStartingBid.Width = 80;
            // 
            // cBuyNowPrice
            // 
            this.cBuyNowPrice.HeaderText = "BuyNowPrice";
            this.cBuyNowPrice.Name = "cBuyNowPrice";
            this.cBuyNowPrice.Width = 80;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Expires";
            this.Column2.Name = "Column2";
            this.Column2.Width = 50;
            // 
            // cPreferredPosition
            // 
            this.cPreferredPosition.HeaderText = "Position";
            this.cPreferredPosition.Name = "cPreferredPosition";
            this.cPreferredPosition.Width = 50;
            // 
            // cRating
            // 
            this.cRating.HeaderText = "Rating";
            this.cRating.Name = "cRating";
            this.cRating.Width = 50;
            // 
            // cRareFlag
            // 
            this.cRareFlag.HeaderText = "RareFlag";
            this.cRareFlag.Name = "cRareFlag";
            this.cRareFlag.Width = 60;
            // 
            // cPAC
            // 
            this.cPAC.HeaderText = "PAC";
            this.cPAC.Name = "cPAC";
            this.cPAC.Visible = false;
            this.cPAC.Width = 50;
            // 
            // cSHO
            // 
            this.cSHO.HeaderText = "SHO";
            this.cSHO.Name = "cSHO";
            this.cSHO.Visible = false;
            this.cSHO.Width = 50;
            // 
            // cPAS
            // 
            this.cPAS.HeaderText = "PAS";
            this.cPAS.Name = "cPAS";
            this.cPAS.Width = 50;
            // 
            // cDRI
            // 
            this.cDRI.HeaderText = "DRI";
            this.cDRI.Name = "cDRI";
            this.cDRI.Visible = false;
            this.cDRI.Width = 50;
            // 
            // cDEF
            // 
            this.cDEF.HeaderText = "DEF";
            this.cDEF.Name = "cDEF";
            this.cDEF.Visible = false;
            this.cDEF.Width = 50;
            // 
            // cPHY
            // 
            this.cPHY.HeaderText = "PHY";
            this.cPHY.Name = "cPHY";
            this.cPHY.Visible = false;
            this.cPHY.Width = 50;
            // 
            // cContract
            // 
            this.cContract.HeaderText = "Contract";
            this.cContract.Name = "cContract";
            this.cContract.Visible = false;
            this.cContract.Width = 50;
            // 
            // cFitness
            // 
            this.cFitness.HeaderText = "Fitness";
            this.cFitness.Name = "cFitness";
            this.cFitness.Visible = false;
            this.cFitness.Width = 50;
            // 
            // AddWatch
            // 
            this.AddWatch.HeaderText = "Add To Watchlist";
            this.AddWatch.Name = "AddWatch";
            this.AddWatch.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.AddWatch.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Auto Bid";
            this.Column1.Name = "Column1";
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.Text = "Auto Bid";
            this.Column1.ToolTipText = "Auto Bid";
            this.Column1.Width = 80;
            // 
            // frmTransferMarket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1682, 1031);
            this.Controls.Add(this.lstLog1);
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
            this.Name = "frmTransferMarket";
            this.Text = "frmTransferMarket";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dgPlayers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

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
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListView lstLog1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.DataGridViewTextBoxColumn cTradeId;
        private System.Windows.Forms.DataGridViewTextBoxColumn cResourceID;
        private System.Windows.Forms.DataGridViewTextBoxColumn cName;
        private System.Windows.Forms.DataGridViewImageColumn cFace;
        private System.Windows.Forms.DataGridViewTextBoxColumn cCurrentBid;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLastSalePrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn cStartingBid;
        private System.Windows.Forms.DataGridViewTextBoxColumn cBuyNowPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
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
        private System.Windows.Forms.DataGridViewButtonColumn AddWatch;
        private System.Windows.Forms.DataGridViewButtonColumn Column1;
    }
}