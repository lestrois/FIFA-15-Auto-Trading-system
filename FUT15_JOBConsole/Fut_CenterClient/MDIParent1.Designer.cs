namespace FUT_MonitorCenter
{
    partial class MDIParent1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MDIParent1));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.listAllAccountsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loginBySessionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quickReloginToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.playerNameUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.targetBidUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.viewHistoryPricesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fUT15ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchPlayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transferTargetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transferListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.myClubPlayersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.myClubSquadsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.autoQuerySinglePlayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoQueryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.autoBidToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.marketToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.marketReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.cascadeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileVerticalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileHorizontalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.arrangeIconsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.toolsMenu,
            this.fUT15ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.marketToolStripMenuItem,
            this.windowsMenu,
            this.helpMenu});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.MdiWindowListItem = this.windowsMenu;
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1149, 25);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "MenuStrip";
            this.menuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip_ItemClicked);
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.listAllAccountsToolStripMenuItem,
            this.newToolStripMenuItem,
            this.loginBySessionToolStripMenuItem,
            this.quickReloginToolStripMenuItem1,
            this.toolStripSeparator4,
            this.exitToolStripMenuItem});
            this.fileMenu.ImageTransparentColor = System.Drawing.SystemColors.ActiveBorder;
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(93, 21);
            this.fileMenu.Text = "Center Panel";
            // 
            // listAllAccountsToolStripMenuItem
            // 
            this.listAllAccountsToolStripMenuItem.Name = "listAllAccountsToolStripMenuItem";
            this.listAllAccountsToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.listAllAccountsToolStripMenuItem.Text = "&List All Accounts";
            this.listAllAccountsToolStripMenuItem.Click += new System.EventHandler(this.listAllAccountsToolStripMenuItem_Click);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Enabled = false;
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Black;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.newToolStripMenuItem.Text = "&Login";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.ShowNewForm);
            // 
            // loginBySessionToolStripMenuItem
            // 
            this.loginBySessionToolStripMenuItem.Enabled = false;
            this.loginBySessionToolStripMenuItem.Name = "loginBySessionToolStripMenuItem";
            this.loginBySessionToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.loginBySessionToolStripMenuItem.Text = "Login by &Session";
            this.loginBySessionToolStripMenuItem.Click += new System.EventHandler(this.loginBySessionToolStripMenuItem_Click);
            // 
            // quickReloginToolStripMenuItem1
            // 
            this.quickReloginToolStripMenuItem1.Enabled = false;
            this.quickReloginToolStripMenuItem1.Name = "quickReloginToolStripMenuItem1";
            this.quickReloginToolStripMenuItem1.Size = new System.Drawing.Size(174, 22);
            this.quickReloginToolStripMenuItem1.Text = "&Quick Relogin";
            this.quickReloginToolStripMenuItem1.Click += new System.EventHandler(this.quickReloginToolStripMenuItem1_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(171, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolsStripMenuItem_Click);
            // 
            // toolsMenu
            // 
            this.toolsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playerNameUpdateToolStripMenuItem,
            this.targetBidUpdateToolStripMenuItem,
            this.toolStripSeparator3,
            this.viewHistoryPricesToolStripMenuItem});
            this.toolsMenu.Name = "toolsMenu";
            this.toolsMenu.Size = new System.Drawing.Size(89, 21);
            this.toolsMenu.Text = "&Data Center";
            // 
            // playerNameUpdateToolStripMenuItem
            // 
            this.playerNameUpdateToolStripMenuItem.Name = "playerNameUpdateToolStripMenuItem";
            this.playerNameUpdateToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.playerNameUpdateToolStripMenuItem.Text = "&Player Name Update";
            this.playerNameUpdateToolStripMenuItem.Click += new System.EventHandler(this.playerNameUpdateToolStripMenuItem_Click);
            // 
            // targetBidUpdateToolStripMenuItem
            // 
            this.targetBidUpdateToolStripMenuItem.Name = "targetBidUpdateToolStripMenuItem";
            this.targetBidUpdateToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.targetBidUpdateToolStripMenuItem.Text = "&Target Bid Update";
            this.targetBidUpdateToolStripMenuItem.Click += new System.EventHandler(this.targetBidUpdateToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(194, 6);
            // 
            // viewHistoryPricesToolStripMenuItem
            // 
            this.viewHistoryPricesToolStripMenuItem.Name = "viewHistoryPricesToolStripMenuItem";
            this.viewHistoryPricesToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.viewHistoryPricesToolStripMenuItem.Text = "&View History Prices";
            this.viewHistoryPricesToolStripMenuItem.Click += new System.EventHandler(this.viewHistoryPricesToolStripMenuItem_Click_1);
            // 
            // fUT15ToolStripMenuItem
            // 
            this.fUT15ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchPlayerToolStripMenuItem,
            this.transferTargetsToolStripMenuItem,
            this.transferListToolStripMenuItem,
            this.toolStripSeparator2,
            this.myClubPlayersToolStripMenuItem,
            this.myClubSquadsToolStripMenuItem});
            this.fUT15ToolStripMenuItem.Name = "fUT15ToolStripMenuItem";
            this.fUT15ToolStripMenuItem.Size = new System.Drawing.Size(56, 21);
            this.fUT15ToolStripMenuItem.Text = "&FUT15";
            // 
            // searchPlayerToolStripMenuItem
            // 
            this.searchPlayerToolStripMenuItem.Name = "searchPlayerToolStripMenuItem";
            this.searchPlayerToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.searchPlayerToolStripMenuItem.Text = "Transfer &Market";
            this.searchPlayerToolStripMenuItem.Click += new System.EventHandler(this.searchPlayerToolStripMenuItem_Click);
            // 
            // transferTargetsToolStripMenuItem
            // 
            this.transferTargetsToolStripMenuItem.Name = "transferTargetsToolStripMenuItem";
            this.transferTargetsToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.transferTargetsToolStripMenuItem.Text = "&Transfer Targets";
            this.transferTargetsToolStripMenuItem.Click += new System.EventHandler(this.transferTargetsToolStripMenuItem_Click);
            // 
            // transferListToolStripMenuItem
            // 
            this.transferListToolStripMenuItem.Name = "transferListToolStripMenuItem";
            this.transferListToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.transferListToolStripMenuItem.Text = "Transfer &List";
            this.transferListToolStripMenuItem.Click += new System.EventHandler(this.transferListToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(169, 6);
            // 
            // myClubPlayersToolStripMenuItem
            // 
            this.myClubPlayersToolStripMenuItem.Name = "myClubPlayersToolStripMenuItem";
            this.myClubPlayersToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.myClubPlayersToolStripMenuItem.Text = "&My Club Players";
            this.myClubPlayersToolStripMenuItem.Click += new System.EventHandler(this.myClubPlayersToolStripMenuItem_Click);
            // 
            // myClubSquadsToolStripMenuItem
            // 
            this.myClubSquadsToolStripMenuItem.Name = "myClubSquadsToolStripMenuItem";
            this.myClubSquadsToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.myClubSquadsToolStripMenuItem.Text = "My &Club Squads";
            this.myClubSquadsToolStripMenuItem.Click += new System.EventHandler(this.myClubSquadsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoQuerySinglePlayerToolStripMenuItem,
            this.autoQueryToolStripMenuItem,
            this.toolStripSeparator1,
            this.autoBidToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(50, 21);
            this.toolStripMenuItem1.Text = "&JOBS";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // autoQuerySinglePlayerToolStripMenuItem
            // 
            this.autoQuerySinglePlayerToolStripMenuItem.Enabled = false;
            this.autoQuerySinglePlayerToolStripMenuItem.Name = "autoQuerySinglePlayerToolStripMenuItem";
            this.autoQuerySinglePlayerToolStripMenuItem.Size = new System.Drawing.Size(265, 22);
            this.autoQuerySinglePlayerToolStripMenuItem.Text = "Auto Query &Single Player && Save";
            this.autoQuerySinglePlayerToolStripMenuItem.Click += new System.EventHandler(this.autoQuerySinglePlayerToolStripMenuItem_Click);
            // 
            // autoQueryToolStripMenuItem
            // 
            this.autoQueryToolStripMenuItem.Enabled = false;
            this.autoQueryToolStripMenuItem.Name = "autoQueryToolStripMenuItem";
            this.autoQueryToolStripMenuItem.Size = new System.Drawing.Size(265, 22);
            this.autoQueryToolStripMenuItem.Text = "Auto &Query && Save";
            this.autoQueryToolStripMenuItem.Visible = false;
            this.autoQueryToolStripMenuItem.Click += new System.EventHandler(this.autoQueryToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(262, 6);
            // 
            // autoBidToolStripMenuItem
            // 
            this.autoBidToolStripMenuItem.Name = "autoBidToolStripMenuItem";
            this.autoBidToolStripMenuItem.Size = new System.Drawing.Size(265, 22);
            this.autoBidToolStripMenuItem.Text = "JOB &Monitor";
            this.autoBidToolStripMenuItem.Click += new System.EventHandler(this.autoBidToolStripMenuItem_Click);
            // 
            // marketToolStripMenuItem
            // 
            this.marketToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.marketReportToolStripMenuItem});
            this.marketToolStripMenuItem.Name = "marketToolStripMenuItem";
            this.marketToolStripMenuItem.Size = new System.Drawing.Size(62, 21);
            this.marketToolStripMenuItem.Text = "&Market";
            // 
            // marketReportToolStripMenuItem
            // 
            this.marketReportToolStripMenuItem.Name = "marketReportToolStripMenuItem";
            this.marketReportToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.marketReportToolStripMenuItem.Text = "&Market Report";
            this.marketReportToolStripMenuItem.Click += new System.EventHandler(this.marketReportToolStripMenuItem_Click);
            // 
            // windowsMenu
            // 
            this.windowsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cascadeToolStripMenuItem,
            this.tileVerticalToolStripMenuItem,
            this.tileHorizontalToolStripMenuItem,
            this.closeAllToolStripMenuItem,
            this.arrangeIconsToolStripMenuItem});
            this.windowsMenu.Name = "windowsMenu";
            this.windowsMenu.Size = new System.Drawing.Size(73, 21);
            this.windowsMenu.Text = "&Windows";
            // 
            // cascadeToolStripMenuItem
            // 
            this.cascadeToolStripMenuItem.Name = "cascadeToolStripMenuItem";
            this.cascadeToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.cascadeToolStripMenuItem.Text = "&Cascade";
            this.cascadeToolStripMenuItem.Click += new System.EventHandler(this.CascadeToolStripMenuItem_Click);
            // 
            // tileVerticalToolStripMenuItem
            // 
            this.tileVerticalToolStripMenuItem.Name = "tileVerticalToolStripMenuItem";
            this.tileVerticalToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.tileVerticalToolStripMenuItem.Text = "Tile &Vertical";
            this.tileVerticalToolStripMenuItem.Click += new System.EventHandler(this.TileVerticalToolStripMenuItem_Click);
            // 
            // tileHorizontalToolStripMenuItem
            // 
            this.tileHorizontalToolStripMenuItem.Name = "tileHorizontalToolStripMenuItem";
            this.tileHorizontalToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.tileHorizontalToolStripMenuItem.Text = "Tile &Horizontal";
            this.tileHorizontalToolStripMenuItem.Click += new System.EventHandler(this.TileHorizontalToolStripMenuItem_Click);
            // 
            // closeAllToolStripMenuItem
            // 
            this.closeAllToolStripMenuItem.Name = "closeAllToolStripMenuItem";
            this.closeAllToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.closeAllToolStripMenuItem.Text = "C&lose All";
            this.closeAllToolStripMenuItem.Click += new System.EventHandler(this.CloseAllToolStripMenuItem_Click);
            // 
            // arrangeIconsToolStripMenuItem
            // 
            this.arrangeIconsToolStripMenuItem.Name = "arrangeIconsToolStripMenuItem";
            this.arrangeIconsToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.arrangeIconsToolStripMenuItem.Text = "&Arrange Icons";
            this.arrangeIconsToolStripMenuItem.Click += new System.EventHandler(this.ArrangeIconsToolStripMenuItem_Click);
            // 
            // helpMenu
            // 
            this.helpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator8,
            this.aboutToolStripMenuItem});
            this.helpMenu.Name = "helpMenu";
            this.helpMenu.Size = new System.Drawing.Size(47, 21);
            this.helpMenu.Text = "&Help";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(134, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.aboutToolStripMenuItem.Text = "&About ... ...";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 665);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1149, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "StatusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // MDIParent1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1149, 687);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MDIParent1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FUT15 Trade House";
            this.Load += new System.EventHandler(this.MDIParent1_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tileHorizontalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsMenu;
        private System.Windows.Forms.ToolStripMenuItem windowsMenu;
        private System.Windows.Forms.ToolStripMenuItem cascadeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tileVerticalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem arrangeIconsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem autoBidToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loginBySessionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem playerNameUpdateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem targetBidUpdateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fUT15ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchPlayerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transferTargetsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transferListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem myClubPlayersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem myClubSquadsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoQueryToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem autoQuerySinglePlayerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem viewHistoryPricesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quickReloginToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem listAllAccountsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem marketToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem marketReportToolStripMenuItem;
    }
}



