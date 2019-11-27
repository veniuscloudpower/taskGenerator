using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OutlookTasks.Sharp
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var connector = new DevOpsTaskConnector.DevOpsTaskConnector(new DevOpsTaskConnector.ConnectorCreds { DevOpsURL = txtDevOpsURL.Text, pat = txtUserName.Text });
            var isConnected = connector.CheckConnection().Result;
            if (isConnected) {
               MessageBox.Show("Connection ok");
            }
            else
            {
               MessageBox.Show("Connection Failed");
            }
     
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            OutlookTasks.Sharp.Properties.Settings.Default.DevOpsUrl = txtDevOpsURL.Text;
            OutlookTasks.Sharp.Properties.Settings.Default.pat = txtUserName.Text;
            OutlookTasks.Sharp.Properties.Settings.Default.tmploc = TextBox1.Text;
            OutlookTasks.Sharp.Properties.Settings.Default.Save();
            Close();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            txtDevOpsURL.Text = OutlookTasks.Sharp.Properties.Settings.Default.DevOpsUrl;
            txtUserName.Text = OutlookTasks.Sharp.Properties.Settings.Default.pat;
            TextBox1.Text = OutlookTasks.Sharp.Properties.Settings.Default.tmploc;
        }
    }
}
