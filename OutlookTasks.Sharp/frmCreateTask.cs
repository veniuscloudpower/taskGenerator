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
    public partial class frmCreateTask : Form
    {

        public Microsoft.Office.Interop.Outlook.MailItem msg { get; set; }

     

        public DevOpsTaskConnector.DevOpsTaskConnector connector { get; set; }


        public frmCreateTask()
        {
            InitializeComponent();
        }

        private void frmCreateTask_Load(object sender, EventArgs e)
        {
            txtTitle.Text = msg.Subject;
            connector = new DevOpsTaskConnector.DevOpsTaskConnector(new DevOpsTaskConnector.ConnectorCreds { DevOpsURL = OutlookTasks.Sharp.Properties.Settings.Default.DevOpsUrl, pat = OutlookTasks.Sharp.Properties.Settings.Default.pat });
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (cmbProjects.SelectedValue.ToString() != "" )
            {
              if ( CheckBox1.Checked = false && String.IsNullOrWhiteSpace(RichTextBox1.Text) )
              {
                    MessageBox.Show("Please add description or check Include mail body  to continue.");
              }
              else
              {
                    if (string.IsNullOrWhiteSpace(cmbType.Text))
                    {
                        MessageBox.Show("Please select type.");
                    }
                    else
                    {
                        CreateTask();
                    }
              }
            }
      

     
        }

        private void CreateTask()
        {
            var task = new DevOpsTaskConnector.TaskDetails();
            task.ProjectId = Guid.Parse(cmbProjects.SelectedValue.ToString());
            task.Title = txtTitle.Text;
            task.Description = RichTextBox1.Text;
            if (CheckBox1.Checked) {
                task.Description = task.Description + Environment.NewLine + msg.Body;
            }
            task.TaskType = cmbType.Text;
            task.Tags = txtTags.Text;
            var fileName = OutlookTasks.Sharp.Properties.Settings.Default.tmploc + Guid.NewGuid().ToString() + ".msg";
            if (!System.IO.Directory.Exists(OutlookTasks.Sharp.Properties.Settings.Default.tmploc)) {
                System.IO.Directory.CreateDirectory(OutlookTasks.Sharp.Properties.Settings.Default.tmploc);
            }
            msg.SaveAs(fileName);
            task.AttachmentFile = fileName;



            var onlinetask = connector.CreateTask(task).Result;



            System.IO.File.Delete(fileName);
            MessageBox.Show("Task created with ID:" + onlinetask.Id.ToString());

        Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            var projectList = connector.GetProjects().Result;
            cmbProjects.DataSource = projectList;
            cmbProjects.DisplayMember = "ProjectName";
            cmbProjects.ValueMember = "ProjectId";
        }
    }
}
