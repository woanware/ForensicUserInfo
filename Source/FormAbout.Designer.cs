namespace woanware
{
    partial class FormAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
            this.kryptonHeader = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.linkWeb = new ComponentFactory.Krypton.Toolkit.KryptonLinkLabel();
            this.linkEmail = new ComponentFactory.Krypton.Toolkit.KryptonLinkLabel();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // kryptonHeader
            // 
            this.kryptonHeader.AutoSize = false;
            this.kryptonHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonHeader.Location = new System.Drawing.Point(0, 0);
            this.kryptonHeader.Name = "kryptonHeader";
            this.kryptonHeader.Size = new System.Drawing.Size(376, 84);
            this.kryptonHeader.StateCommon.Content.Padding = new System.Windows.Forms.Padding(13, 3, 10, -1);
            this.kryptonHeader.TabIndex = 0;
            this.kryptonHeader.TabStop = false;
            this.kryptonHeader.Values.Description = "v1.0.0";
            this.kryptonHeader.Values.Heading = "  ForensicUserInfo";
            this.kryptonHeader.Values.Image = ((System.Drawing.Image)(resources.GetObject("kryptonHeader.Values.Image")));
            // 
            // linkWeb
            // 
            this.linkWeb.Location = new System.Drawing.Point(111, 94);
            this.linkWeb.Name = "linkWeb";
            this.linkWeb.Size = new System.Drawing.Size(120, 19);
            this.linkWeb.TabIndex = 1;
            this.linkWeb.TabStop = false;
            this.linkWeb.Values.Text = "www.woanware.co.uk";
            this.linkWeb.LinkClicked += new System.EventHandler(this.linkWeb_LinkClicked);
            // 
            // linkEmail
            // 
            this.linkEmail.Location = new System.Drawing.Point(111, 117);
            this.linkEmail.Name = "linkEmail";
            this.linkEmail.Size = new System.Drawing.Size(128, 19);
            this.linkEmail.TabIndex = 2;
            this.linkEmail.TabStop = false;
            this.linkEmail.Values.Text = "markwoan@gmail.com";
            this.linkEmail.LinkClicked += new System.EventHandler(this.linkEmail_LinkClicked);
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(79, 95);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(34, 19);
            this.kryptonLabel1.TabIndex = 3;
            this.kryptonLabel1.TabStop = false;
            this.kryptonLabel1.Values.Text = "Web";
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(75, 118);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(38, 19);
            this.kryptonLabel2.TabIndex = 4;
            this.kryptonLabel2.TabStop = false;
            this.kryptonLabel2.Values.Text = "Email";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(295, 149);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 29);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FormAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(376, 183);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.kryptonLabel2);
            this.Controls.Add(this.kryptonLabel1);
            this.Controls.Add(this.linkEmail);
            this.Controls.Add(this.linkWeb);
            this.Controls.Add(this.kryptonHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAbout";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About ForensicUserInfo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader;
        private ComponentFactory.Krypton.Toolkit.KryptonLinkLabel linkWeb;
        private ComponentFactory.Krypton.Toolkit.KryptonLinkLabel linkEmail;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private System.Windows.Forms.Button btnClose;
    }
}