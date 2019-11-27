using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConnectorTests
{
    [TestClass]
    public class DevConnector
    {


        public DevConnector()
        {
            cred = new DevOpsTaskConnector.ConnectorCreds
            {
                DevOpsURL = "https://dev.azure.com/<name>",
                pat = "<your key>"
            };
        }

        private TestContext m_testContext;

        private DevOpsTaskConnector.ConnectorCreds cred { get; set; }

        public TestContext TestContext

        {

            get { return m_testContext; }

            set { m_testContext = value; }

        }

        [TestMethod]
        public void CheckConnection()
        {
            
            var connector = new DevOpsTaskConnector.DevOpsTaskConnector(cred);

           if(! connector.CheckConnection().Result)
           {
                Assert.Fail("Connection Failed");
           }

        }

        [TestMethod]
        public void GetTask()
        {
           

            var connector = new DevOpsTaskConnector.DevOpsTaskConnector(cred);

            var task = connector.GetTask(67).Result;



            TestContext.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(task));
        }

        [TestMethod]
        public void CreateTask()
        {
            

            var connector = new DevOpsTaskConnector.DevOpsTaskConnector(cred);

            var projectList = connector.GetProjects().Result;

            var task = connector.CreateTask(new DevOpsTaskConnector.TaskDetails
            {
                ProjectId = projectList[0].ProjectId,
                Title = "New task",
                TaskType = "Task",
                Description = "Details about...",
                Tags = "c#,Library",
                AttachmentFile = @"C:\applications\Readme.txt"
            }).Result;

            TestContext.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(task));
        }

        [TestMethod]
        public void GetProjects()
        {
            

            var connector = new DevOpsTaskConnector.DevOpsTaskConnector(cred);

            var projectList = connector.GetProjects().Result;

            TestContext.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(projectList));

        }

    }
}
