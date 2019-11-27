namespace OutlookTasks.Sharp
{
    partial class TaskCreator : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public TaskCreator()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.btnSettings = this.Factory.CreateRibbonButton();
            this.CreateTask = this.Factory.CreateRibbonButton();
            this.Button2 = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.group1);
            this.tab1.Label = "Task Creator";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.Items.Add(this.btnSettings);
            this.group1.Items.Add(this.CreateTask);
            this.group1.Items.Add(this.Button2);
            this.group1.Label = "Options";
            this.group1.Name = "group1";
            // 
            // btnSettings
            // 
            this.btnSettings.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnSettings.Image = global::OutlookTasks.Sharp.Properties.Resources.settings;
            this.btnSettings.Label = "Settings";
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.ShowImage = true;
            this.btnSettings.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnSettings_Click);
            // 
            // CreateTask
            // 
            this.CreateTask.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.CreateTask.Image = global::OutlookTasks.Sharp.Properties.Resources.createTask;
            this.CreateTask.Label = "Create Task";
            this.CreateTask.Name = "CreateTask";
            this.CreateTask.ShowImage = true;
            this.CreateTask.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.CreateTask_Click);
            // 
            // Button2
            // 
            this.Button2.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.Button2.Image = global::OutlookTasks.Sharp.Properties.Resources.updateTask;
            this.Button2.Label = "Update Task";
            this.Button2.Name = "Button2";
            this.Button2.ShowImage = true;
            this.Button2.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.Button2_Click);
            // 
            // TaskCreator
            // 
            this.Name = "TaskCreator";
            this.RibbonType = "Microsoft.Outlook.Explorer, Microsoft.Outlook.Mail.Read";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.TaskCreator_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnSettings;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton CreateTask;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton Button2;
    }

    partial class ThisRibbonCollection
    {
        internal TaskCreator TaskCreator
        {
            get { return this.GetRibbon<TaskCreator>(); }
        }
    }
}
