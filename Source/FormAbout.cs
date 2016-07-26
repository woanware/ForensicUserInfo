using System.Windows.Forms;

namespace woanware
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FormAbout : Form
    {
        /// <summary>
        /// 
        /// </summary>
        public FormAbout()
        {
            InitializeComponent();

            kryptonHeader.Text = Application.ProductName;
            kryptonHeader.Values.Description = "v" + Application.ProductVersion;

            this.Text = Application.ProductName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkEmail_LinkClicked(object sender, System.EventArgs e)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.RedirectStandardOutput = false;
            process.StartInfo.FileName = "mailto:" + linkEmail.Text;
            process.StartInfo.UseShellExecute = true;
            process.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkWeb_LinkClicked(object sender, System.EventArgs e)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.RedirectStandardOutput = false;
            process.StartInfo.FileName = "http://" + linkEmail.Text;
            process.StartInfo.UseShellExecute = true;
            process.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
