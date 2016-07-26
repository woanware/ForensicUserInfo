namespace woanware
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileSepOne = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolBtnOpen = new System.Windows.Forms.ToolStripButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.listUserAccounts = new System.Windows.Forms.ListView();
            this.colRid = new System.Windows.Forms.ColumnHeader();
            this.colLoginName = new System.Windows.Forms.ColumnHeader();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.colDescription = new System.Windows.Forms.ColumnHeader();
            this.colUserComment = new System.Windows.Forms.ColumnHeader();
            this.colLastLoginDate = new System.Windows.Forms.ColumnHeader();
            this.colPasswordResetDate = new System.Windows.Forms.ColumnHeader();
            this.colAccountExpiryDate = new System.Windows.Forms.ColumnHeader();
            this.colLoginFailedDate = new System.Windows.Forms.ColumnHeader();
            this.colLoginCount = new System.Windows.Forms.ColumnHeader();
            this.colFailedLogins = new System.Windows.Forms.ColumnHeader();
            this.colProfilePath = new System.Windows.Forms.ColumnHeader();
            this.colGroups = new System.Windows.Forms.ColumnHeader();
            this.colNtHash = new System.Windows.Forms.ColumnHeader();
            this.colLmHash = new System.Windows.Forms.ColumnHeader();
            this.colPasswordRequired = new System.Windows.Forms.ColumnHeader();
            this.colAccountStatus = new System.Windows.Forms.ColumnHeader();
            this.menuFileExport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.menuFileExportCsv = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileExportHtml = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMain.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuHelp});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuMain.Size = new System.Drawing.Size(602, 24);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFileOpen,
            this.menuFileSepOne,
            this.menuFileExport,
            this.toolStripMenuItem1,
            this.menuFileExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "&File";
            // 
            // menuFileOpen
            // 
            this.menuFileOpen.Name = "menuFileOpen";
            this.menuFileOpen.Size = new System.Drawing.Size(152, 22);
            this.menuFileOpen.Text = "Open";
            this.menuFileOpen.Click += new System.EventHandler(this.menuFileOpen_Click);
            // 
            // menuFileSepOne
            // 
            this.menuFileSepOne.Name = "menuFileSepOne";
            this.menuFileSepOne.Size = new System.Drawing.Size(149, 6);
            // 
            // menuFileExit
            // 
            this.menuFileExit.Name = "menuFileExit";
            this.menuFileExit.Size = new System.Drawing.Size(152, 22);
            this.menuFileExit.Text = "Exit";
            this.menuFileExit.Click += new System.EventHandler(this.menuFileExit_Click);
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuHelpAbout});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(44, 20);
            this.menuHelp.Text = "&Help";
            // 
            // menuHelpAbout
            // 
            this.menuHelpAbout.Name = "menuHelpAbout";
            this.menuHelpAbout.Size = new System.Drawing.Size(152, 22);
            this.menuHelpAbout.Text = "&About";
            this.menuHelpAbout.Click += new System.EventHandler(this.menuHelpAbout_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolBtnOpen});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(602, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolBtnOpen
            // 
            this.toolBtnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolBtnOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolBtnOpen.Image")));
            this.toolBtnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolBtnOpen.Name = "toolBtnOpen";
            this.toolBtnOpen.Size = new System.Drawing.Size(23, 22);
            this.toolBtnOpen.Text = "toolStripButton1";
            this.toolBtnOpen.Click += new System.EventHandler(this.toolBtnOpen_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "All Files|*.*";
            // 
            // listUserAccounts
            // 
            this.listUserAccounts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colRid,
            this.colLoginName,
            this.colName,
            this.colDescription,
            this.colUserComment,
            this.colPasswordRequired,
            this.colAccountStatus,
            this.colLastLoginDate,
            this.colPasswordResetDate,
            this.colAccountExpiryDate,
            this.colLoginFailedDate,
            this.colLoginCount,
            this.colFailedLogins,
            this.colProfilePath,
            this.colGroups,
            this.colLmHash,
            this.colNtHash});
            this.listUserAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listUserAccounts.FullRowSelect = true;
            this.listUserAccounts.HideSelection = false;
            this.listUserAccounts.Location = new System.Drawing.Point(0, 49);
            this.listUserAccounts.Name = "listUserAccounts";
            this.listUserAccounts.Size = new System.Drawing.Size(602, 204);
            this.listUserAccounts.TabIndex = 2;
            this.listUserAccounts.UseCompatibleStateImageBehavior = false;
            this.listUserAccounts.View = System.Windows.Forms.View.Details;
            // 
            // colRid
            // 
            this.colRid.Text = "RID";
            // 
            // colLoginName
            // 
            this.colLoginName.Text = "Login Name";
            // 
            // colName
            // 
            this.colName.Text = "Name";
            // 
            // colDescription
            // 
            this.colDescription.Text = "Description";
            // 
            // colUserComment
            // 
            this.colUserComment.Text = "User Comment";
            // 
            // colLastLoginDate
            // 
            this.colLastLoginDate.Text = "Last Login Date";
            // 
            // colPasswordResetDate
            // 
            this.colPasswordResetDate.Text = "Password Reset Date";
            // 
            // colAccountExpiryDate
            // 
            this.colAccountExpiryDate.Text = "Account Expiry Date";
            // 
            // colLoginFailedDate
            // 
            this.colLoginFailedDate.Text = "Login Failed Date";
            // 
            // colLoginCount
            // 
            this.colLoginCount.Text = "Login Count";
            // 
            // colFailedLogins
            // 
            this.colFailedLogins.Text = "Failed Logins";
            // 
            // colProfilePath
            // 
            this.colProfilePath.Text = "Profile Path";
            // 
            // colGroups
            // 
            this.colGroups.Text = "Groups";
            // 
            // colNtHash
            // 
            this.colNtHash.Text = "NT Hash";
            // 
            // colLmHash
            // 
            this.colLmHash.Text = "LM Hash";
            // 
            // colPasswordRequired
            // 
            this.colPasswordRequired.Text = "Password Required";
            // 
            // colAccountStatus
            // 
            this.colAccountStatus.Text = "Account Status";
            // 
            // menuFileExport
            // 
            this.menuFileExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFileExportCsv,
            this.menuFileExportHtml});
            this.menuFileExport.Name = "menuFileExport";
            this.menuFileExport.Size = new System.Drawing.Size(152, 22);
            this.menuFileExport.Text = "Export";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "*.csv";
            this.saveFileDialog.FileName = "ForensicUserInfo.csv";
            this.saveFileDialog.Filter = "CSV|*.csv|All Files|*.*";
            this.saveFileDialog.Title = "Select the export file";
            // 
            // menuFileExportCsv
            // 
            this.menuFileExportCsv.Name = "menuFileExportCsv";
            this.menuFileExportCsv.Size = new System.Drawing.Size(152, 22);
            this.menuFileExportCsv.Text = "CSV";
            this.menuFileExportCsv.Click += new System.EventHandler(this.menuFileExportCsv_Click);
            // 
            // menuFileExportHtml
            // 
            this.menuFileExportHtml.Name = "menuFileExportHtml";
            this.menuFileExportHtml.Size = new System.Drawing.Size(152, 22);
            this.menuFileExportHtml.Text = "HTML";
            this.menuFileExportHtml.Click += new System.EventHandler(this.menuFileExportHtml_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 253);
            this.Controls.Add(this.listUserAccounts);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ForensicUserInfo";
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuFileOpen;
        private System.Windows.Forms.ToolStripSeparator menuFileSepOne;
        private System.Windows.Forms.ToolStripMenuItem menuFileExit;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuHelpAbout;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolBtnOpen;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ListView listUserAccounts;
        private System.Windows.Forms.ColumnHeader colRid;
        private System.Windows.Forms.ColumnHeader colLoginName;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colDescription;
        private System.Windows.Forms.ColumnHeader colUserComment;
        private System.Windows.Forms.ColumnHeader colPasswordRequired;
        private System.Windows.Forms.ColumnHeader colLastLoginDate;
        private System.Windows.Forms.ColumnHeader colPasswordResetDate;
        private System.Windows.Forms.ColumnHeader colAccountExpiryDate;
        private System.Windows.Forms.ColumnHeader colLoginFailedDate;
        private System.Windows.Forms.ColumnHeader colLoginCount;
        private System.Windows.Forms.ColumnHeader colFailedLogins;
        private System.Windows.Forms.ColumnHeader colProfilePath;
        private System.Windows.Forms.ColumnHeader colGroups;
        private System.Windows.Forms.ColumnHeader colLmHash;
        private System.Windows.Forms.ColumnHeader colNtHash;
        private System.Windows.Forms.ColumnHeader colAccountStatus;
        private System.Windows.Forms.ToolStripMenuItem menuFileExport;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem menuFileExportCsv;
        private System.Windows.Forms.ToolStripMenuItem menuFileExportHtml;
    }
}

