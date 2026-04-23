using R34Downloader.Models;
using System;
using System.Windows.Forms;

namespace R34Downloader.Forms
{
    /// <summary>
    /// Settings form.
    /// </summary>
    public partial class SettingsForm : Form
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SettingsForm class.
        /// </summary>
        public SettingsForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Handlers

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            if (SettingsModel.IsApi)
            {
                radioButton1.Checked = true;
            }
            else
            {
                radioButton2.Checked = true;
            }

            textBox1.Text = SettingsModel.UserId;
            textBox2.Text = SettingsModel.ApiKey;
        }

        private void radioButton_MouseClick(object sender, MouseEventArgs e)
        {
            SettingsModel.IsApi = radioButton1.Checked;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            SettingsModel.UserId = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            SettingsModel.ApiKey = textBox2.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.IsApi = SettingsModel.IsApi;
            Properties.Settings.Default.UserId = SettingsModel.UserId;
            Properties.Settings.Default.ApiKey = SettingsModel.ApiKey;
            Properties.Settings.Default.Save();
            Close();
        }

        #endregion
    }
}
