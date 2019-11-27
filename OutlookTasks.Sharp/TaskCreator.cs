using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Tools.Ribbon;

namespace OutlookTasks.Sharp
{
    public partial class TaskCreator
    {
        private void TaskCreator_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void btnSettings_Click(object sender, RibbonControlEventArgs e)
        {
            var frm = new frmSettings();
            frm.Show();
        }

        private void CreateTask_Click(object sender, RibbonControlEventArgs e)
        {
            var frm = new frmCreateTask();
            var myOlApp = new Microsoft.Office.Interop.Outlook.Application();
            Microsoft.Office.Interop.Outlook.Explorer myOlExp = myOlApp.ActiveExplorer();
            var SelectedObject = myOlExp.Selection[1];

            var MailItem = (Microsoft.Office.Interop.Outlook.MailItem)SelectedObject;
            if (MailItem!= null)
            {
                frm.msg = MailItem;
                frm.Show();
            }             
            else
            {
                MessageBox.Show("Please select a mail to create a task");
            }
               
          
     
        }

        private void Button2_Click(object sender, RibbonControlEventArgs e)
        {
            var frm = new frmUpdateTask();
       var myOlApp = new Microsoft.Office.Interop.Outlook.Application();
            var myOlExp = myOlApp.ActiveExplorer();
            var SelectedObject = myOlExp.Selection[1];

            var MailItem = (Microsoft.Office.Interop.Outlook.MailItem)SelectedObject;
            if (MailItem != null)
            {
                frm.msg = MailItem;
                frm.Show();
            }
            else
            {
                MessageBox.Show("Please select a mail to create a task");
            }
        }
    }
}
