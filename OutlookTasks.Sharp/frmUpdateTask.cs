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
    public partial class frmUpdateTask : Form
    {

        public Microsoft.Office.Interop.Outlook.MailItem msg { get; set; }

        public DevOpsTaskConnector.TaskDetails task { get; set; }

        public DevOpsTaskConnector.DevOpsTaskConnector connector { get; set; }



        public frmUpdateTask()
        {
            InitializeComponent();
        }

        private void TextBox1_Leave(object sender, EventArgs e)
        {
            var tid = Convert.ToInt32(TextBox1.Text);
            task = connector.GetTask(tid).Result;
            label2.Text = task.Description;
        }

        private void frmUpdateTask_Load(object sender, EventArgs e)
        {
            connector = new DevOpsTaskConnector.DevOpsTaskConnector(new DevOpsTaskConnector.ConnectorCreds { DevOpsURL = OutlookTasks.Sharp.Properties.Settings.Default.DevOpsUrl, pat = OutlookTasks.Sharp.Properties.Settings.Default.pat });
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (task.Id > 0) {

                if (CheckBox1.Checked) {
                    task.Description = msg.Body + Environment.NewLine + task.Description;
                }
                else
                {
                    task.Description = String.Empty;
                }



                var fileName = OutlookTasks.Sharp.Properties.Settings.Default.tmploc + Guid.NewGuid().ToString() + ".msg";
                if (!System.IO.Directory.Exists(OutlookTasks.Sharp.Properties.Settings.Default.tmploc)) {
                    System.IO.Directory.CreateDirectory(OutlookTasks.Sharp.Properties.Settings.Default.tmploc);
                }
                msg.SaveAs(fileName);
                task.AttachmentFile = fileName;

            var onlinetask = connector.UpdateTask(task).Result;

                System.IO.File.Delete(fileName);
                MessageBox.Show("Task with ID:" + onlinetask.Id.ToString() + " updated.");


                Close();


        }
        else
        {
            MessageBox.Show("Please select a task to continue");
        }
        }
    }
}
