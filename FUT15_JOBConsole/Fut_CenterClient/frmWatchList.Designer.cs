namespace FUT_MonitorCenter
{
    partial class frmWatchList
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
            this.button1 = new System.Windows.Forms.Button();
            this.btnGetPrice = new System.Windows.Forms.Button();
            this.lstLog1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.dgPlayers = new System.Windows.Forms.DataGridView();
            this.cTradeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cResourceID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cFace = new System.Windows.Forms.DataGridViewImageColumn();
            this.BidState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Expires = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cCurrentBid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cStartingBid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cBuyNowPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPreferredPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cRating = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cRareFlag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cContract = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cFitness = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgPlayers)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1118, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Retrieve";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnGetPrice
            // 
            this.btnGetPrice.Location = new System.Drawing.Point(1117, 37);
            this.btnGetPrice.Name = "btnGetPrice";
            this.btnGetPrice.Size = new System.Drawing.Size(113, 23);
            this.btnGetPrice.TabIndex = 8;
            this.btnGetPrice.Text = "Save Price && Clear";
            this.btnGetPrice.UseVisualStyleBackColor = true;
            this.btnGetPrice.Click += new System.EventHandler(this.btnGetPrice_Click);
            // 
            // lstLog1
            // 
            this.lstLog1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lstLog1.GridLines = true;
            this.lstLog1.Location = new System.Drawing.Point(1117, 66);
            this.lstLog1.Name = "lstLog1";
            this.lstLog1.Size = new System.Drawing.Size(555, 776);
            this.lstLog1.TabIndex = 7;
            this.lstLog1.UseCompatibleStateImageBehavior = false;
            this.lstLog1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Saving Log";
            this.columnHeader1.Width = 544;
            // 
            // dgPlayers
            // 
            this.dgPlayers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgPlayers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cTradeId,
            this.cResourceID,
            this.cName,
            this.cFace,
            this.BidState,
            this.Expires,
            this.cCurrentBid,
            this.cStartingBid,
            this.cBuyNowPrice,
            this.cPreferredPosition,
            this.cRating,
            this.cRareFlag,
            this.cContract,
            this.cFitness,
            this.Column1});
            this.dgPlayers.Location = new System.Drawing.Point(3, 9);
            this.dgPlayers.Name = "dgPlayers";
            this.dgPlayers.ReadOnly = true;
            this.dgPlayers.Size = new System.Drawing.Size(1111, 829);
            this.dgPlayers.TabIndex = 33;
            // 
            // cTradeId
            // 
            this.cTradeId.HeaderText = "TradeId";
            this.cTradeId.Name = "cTradeId";
            this.cTradeId.ReadOnly = true;
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
            this.cName.ReadOnly = true;
            // 
            // cFace
            // 
            this.cFace.HeaderText = "Face";
            this.cFace.Name = "cFace";
            this.cFace.ReadOnly = true;
            this.cFace.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cFace.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cFace.Width = 80;
            // 
            // BidState
            // 
            this.BidState.HeaderText = "BidState";
            this.BidState.Name = "BidState";
            this.BidState.ReadOnly = true;
            this.BidState.Width = 60;
            // 
            // Expires
            // 
            this.Expires.HeaderText = "Expires";
            this.Expires.Name = "Expires";
            this.Expires.ReadOnly = true;
            this.Expires.Width = 50;
            // 
            // cCurrentBid
            // 
            this.cCurrentBid.HeaderText = "CurrentBid";
            this.cCurrentBid.Name = "cCurrentBid";
            this.cCurrentBid.ReadOnly = true;
            this.cCurrentBid.Width = 50;
            // 
            // cStartingBid
            // 
            this.cStartingBid.HeaderText = "StartingBid";
            this.cStartingBid.Name = "cStartingBid";
            this.cStartingBid.ReadOnly = true;
            this.cStartingBid.Width = 70;
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
            this.cRareFlag.Width = 70;
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
            // Column1
            // 
            this.Column1.HeaderText = "Bid";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 80;
            // 
            // frmWatchList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1680, 854);
            this.Controls.Add(this.dgPlayers);
            this.Controls.Add(this.btnGetPrice);
            this.Controls.Add(this.lstLog1);
            this.Controls.Add(this.button1);
            this.Name = "frmWatchList";
            this.Text = "frmWatchList";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dgPlayers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnGetPrice;
        private System.Windows.Forms.ListView lstLog1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.DataGridView dgPlayers;
        private System.Windows.Forms.DataGridViewTextBoxColumn cTradeId;
        private System.Windows.Forms.DataGridViewTextBoxColumn cResourceID;
        private System.Windows.Forms.DataGridViewTextBoxColumn cName;
        private System.Windows.Forms.DataGridViewImageColumn cFace;
        private System.Windows.Forms.DataGridViewTextBoxColumn BidState;
        private System.Windows.Forms.DataGridViewTextBoxColumn Expires;
        private System.Windows.Forms.DataGridViewTextBoxColumn cCurrentBid;
        private System.Windows.Forms.DataGridViewTextBoxColumn cStartingBid;
        private System.Windows.Forms.DataGridViewTextBoxColumn cBuyNowPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPreferredPosition;
        private System.Windows.Forms.DataGridViewTextBoxColumn cRating;
        private System.Windows.Forms.DataGridViewTextBoxColumn cRareFlag;
        private System.Windows.Forms.DataGridViewTextBoxColumn cContract;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFitness;
        private System.Windows.Forms.DataGridViewButtonColumn Column1;
    }
}