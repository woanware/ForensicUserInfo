using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FormMain : Form
    {
        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public FormMain()
        {
            InitializeComponent();
        }
        #endregion

        #region Menu Event Handlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFileExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFileOpen_Click(object sender, EventArgs e)
        {
            this.ParseFile();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuHelpAbout_Click(object sender, EventArgs e)
        {
            using (FormAbout formAbout = new FormAbout())
            {
                formAbout.ShowDialog(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFileExportCsv_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "CSV Files|*.csv|All Files|*.*";
            saveFileDialog.FileName = "ForensicUserInfo.csv";
            if (saveFileDialog.ShowDialog(this) == DialogResult.Cancel)
            {
                return;
            }

            string csv = UserInterface.ExportListviewToCsv(listUserAccounts, true);

            IO.WriteTextToFile(csv, saveFileDialog.FileName, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFileExportHtml_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "HTML Files|*.html|All Files|*.*";
            saveFileDialog.FileName = "ForensicUserInfo.html";
            if (saveFileDialog.ShowDialog(this) == DialogResult.Cancel)
            {
                return;
            }

            string html = UserInterface.ExportListviewToHtml(listUserAccounts);

            IO.WriteTextToFile(html, saveFileDialog.FileName, false);
        }
        #endregion

        #region Toolbar Button Event Handlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolBtnOpen_Click(object sender, EventArgs e)
        {
            this.ParseFile();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        private void ParseFile()
        {
            string samFile = string.Empty;
            openFileDialog.Title = "Select the SAM registry file";
            if (openFileDialog.ShowDialog(this) == DialogResult.Cancel)
            {
                return;
            }
            samFile = openFileDialog.FileName;

            string softwareFile = string.Empty;
            openFileDialog.Title = "Select the SOFTWARE registry file";
            if (openFileDialog.ShowDialog(this) == DialogResult.Cancel)
            {
                return;
            }
            softwareFile = openFileDialog.FileName;

            string systemFile = string.Empty;
            openFileDialog.Title = "Select the SYSTEM registry file";
            if (openFileDialog.ShowDialog(this) == DialogResult.Cancel)
            {
                return;
            }
            systemFile = openFileDialog.FileName;

            SamParser samParser = new SamParser(samFile, softwareFile, systemFile);
            List<UserAccount> userAccounts = samParser.Parse();
            if (userAccounts == null)
            {
                UserInterface.DisplayMessageBox(this, "No users identified", MessageBoxIcon.Exclamation);
                return;
            }

            if (userAccounts.Count == 0)
            {
                UserInterface.DisplayMessageBox(this, "No users identified", MessageBoxIcon.Exclamation);
                return;
            }

            listUserAccounts.Items.Clear();
            for (int index = 0; index < userAccounts.Count; index++)
            {
                ListViewItem lvwItem = new ListViewItem();
                lvwItem.Text = userAccounts[index].Rid.ToString();
                lvwItem.SubItems.Add(userAccounts[index].LoginName);
                lvwItem.SubItems.Add(userAccounts[index].Name);
                lvwItem.SubItems.Add(userAccounts[index].Description);
                lvwItem.SubItems.Add(userAccounts[index].UserComment);

                if (userAccounts[index].NtHash == samParser.EmptyNt)
                {
                    lvwItem.SubItems.Add("Password not required");
                }
                else
                {
                    if (userAccounts[index].NtHash.Trim().Length == 0)
                    {
                        lvwItem.SubItems.Add("Password not required");
                    }
                    else
                    {
                        lvwItem.SubItems.Add("Password required");
                    }
                }

                if (userAccounts[index].IsDisabled == true)
                {
                    lvwItem.SubItems.Add("Disabled");
                }
                else
                {
                    lvwItem.SubItems.Add("Active");
                }

                lvwItem.SubItems.Add(userAccounts[index].LastLoginDate != "00:00 01/01/1601" ? userAccounts[index].LastLoginDate : string.Empty);
                lvwItem.SubItems.Add(userAccounts[index].PasswordResetDate != "00:00 01/01/1601" ? userAccounts[index].PasswordResetDate : string.Empty);
                lvwItem.SubItems.Add(userAccounts[index].AccountExpiryDate != "00:00 01/01/1601" ? userAccounts[index].AccountExpiryDate : string.Empty);
                lvwItem.SubItems.Add(userAccounts[index].LoginFailedDate != "00:00 01/01/1601" ? userAccounts[index].LoginFailedDate : string.Empty);
                lvwItem.SubItems.Add(userAccounts[index].LoginCount.ToString());
                lvwItem.SubItems.Add(userAccounts[index].FailedLogins.ToString());
                lvwItem.SubItems.Add(userAccounts[index].ProfilePath);
                lvwItem.SubItems.Add(userAccounts[index].Groups);
                lvwItem.SubItems.Add(userAccounts[index].LmHash);
                lvwItem.SubItems.Add(userAccounts[index].NtHash);

                listUserAccounts.Items.Add(lvwItem);
            }

            if (listUserAccounts.Items.Count > 0)
            {
                listUserAccounts.Items[0].Selected = true;
            }

            UserInterface.AutoSizeListViewColumns(listUserAccounts);
        }
        #endregion
    }
}
