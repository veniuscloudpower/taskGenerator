using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttachmentReference = Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.AttachmentReference;
using WorkItem = Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem;

namespace DevOpsTaskConnector
{
    public class DevOpsTaskConnector
    {
        private ConnectorCreds credentials { get; set; }

        public DevOpsTaskConnector(ConnectorCreds creds)
        {
            credentials = creds;
        }

        public async  Task<bool> CheckConnection()
        {
            VssCredentials creds = new VssBasicCredential(string.Empty, credentials.pat);

            // Connect to Azure DevOps Services
            VssConnection connection = new VssConnection(new Uri(credentials.DevOpsURL), creds);


           await connection.ConnectAsync();

            return connection.HasAuthenticated;
        }

        public async Task<List<TeamProject>> GetProjects()
        {
            VssCredentials creds = new VssBasicCredential(string.Empty, credentials.pat);

            // Connect to Azure DevOps Services
            VssConnection connection = new VssConnection(new Uri(credentials.DevOpsURL), creds);

            ProjectHttpClient projectClient = connection.GetClient<ProjectHttpClient>();

            // Call to get the list of projects

            IEnumerable<TeamProjectReference> projects = await projectClient.GetProjects();

            var items = new List<TeamProject>();

            foreach (var item in projects)
            {
                items.Add(new TeamProject
                {
                     ProjectId = item.Id,
                      ProjectName = item.Name
                });
            }

            

            return items;
        }

        public async Task<WorkItem> CreateTask(TaskDetails task)
        {
            VssCredentials creds = new VssBasicCredential(string.Empty, credentials.pat);

            // Connect to Azure DevOps Services
            VssConnection connection = new VssConnection(new Uri(credentials.DevOpsURL), creds);

            // Construct the object containing field values required for the new work item
            JsonPatchDocument patchDocument = new JsonPatchDocument();

            patchDocument.Add(
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/fields/System.Title",
                    Value = task.Title
                }
            );

            if (!String.IsNullOrWhiteSpace(task.Description))
            {
                patchDocument.Add(
                   new JsonPatchOperation()
                   {
                       Operation = Operation.Add,
                       Path = "/fields/System.Description",
                       Value = task.Description
                   }
               );
            }

            if (!String.IsNullOrWhiteSpace(task.Tags))
            {

                patchDocument.Add(
                     new JsonPatchOperation()
                     {
                         Operation = Operation.Add,
                         Path = "/fields/System.Tags",
                         Value = task.Tags
                     }
                 );
            }

            
            WorkItemTrackingHttpClient workItemTrackingClient = connection.GetClient<WorkItemTrackingHttpClient>();

            // Create the new work item
            WorkItem newWorkItem = workItemTrackingClient.CreateWorkItemAsync(patchDocument, task.ProjectId, "Task").Result;

            // Construct the object containing field values required for the new work item
             patchDocument = new JsonPatchDocument();

            workItemTrackingClient = connection.GetClient<WorkItemTrackingHttpClient>();

            if (!String.IsNullOrWhiteSpace(task.AttachmentFile))
            {
                if (System.IO.File.Exists(task.AttachmentFile))
                {
                    // upload attachment to store and get a reference to that file
                    AttachmentReference attachmentReference = await workItemTrackingClient.CreateAttachmentAsync(task.AttachmentFile);

                    var attatchmentURL = attachmentReference.Url.Split('?')[0] + "?filename=" + System.IO.Path.GetFileName(task.AttachmentFile);

                    patchDocument.Add(
                        new JsonPatchOperation()
                        {
                            Operation = Operation.Add,
                            Path = "/fields/System.History",
                            Value = "Attach file"
                        }
                    );

                    patchDocument.Add(
                      new JsonPatchOperation()
                      {
                          Operation = Operation.Add,
                          Path = "/relations/-",
                          Value = new
                          {
                              rel = "AttachedFile",
                              url = attatchmentURL,

                              attributes = new { comment = "Details :" + task.Title }
                          }
                      }
                    );
                }
            }





            // Construct the object containing field values required for the new work item
            //   JsonPatchDocument patchAssignmentDocument = new JsonPatchDocument();

            patchDocument.Add(new JsonPatchOperation
            {
                Operation = Operation.Replace,
                Path = "/fields/System.AssignedTo",
                Value = newWorkItem.Fields["System.CreatedBy"]
            });

            WorkItem assignedItem = workItemTrackingClient.UpdateWorkItemAsync(patchDocument, newWorkItem.Id ?? 0).Result;

            return assignedItem;
        }


        public async Task<TaskDetails> GetTask(int Id)
        {
            VssCredentials creds = new VssBasicCredential(string.Empty, credentials.pat);

            // Connect to Azure DevOps Services
            VssConnection connection = new VssConnection(new Uri(credentials.DevOpsURL), creds);
            WorkItemTrackingHttpClient workItemTrackingClient = connection.GetClient<WorkItemTrackingHttpClient>();
            try
            {
                var workitem = await workItemTrackingClient.GetWorkItemAsync(Id, expand: WorkItemExpand.All);





                var taglist = string.Empty;

                try
                {
                    if (workitem.Fields["System.Tags"] != null)
                    {
                        taglist = workitem.Fields["System.Tags"].ToString();
                    }
                }
                catch (Exception)
                {

                    taglist = string.Empty;
                }



                var taskDetails = new TaskDetails
                {
                    Id = workitem.Id ?? 0,
                    Title = workitem.Fields["System.Title"].ToString(),
                    Description = workitem.Fields["System.Description"].ToString(),

                    Tags = taglist,
                    TaskType = workitem.Fields["System.WorkItemType"].ToString()
                };


                return taskDetails;
            }
            catch (Exception)
            {

                return new TaskDetails {
                    Id = -1
                };
            }




        }

        public async Task<WorkItem> UpdateTask(TaskDetails task)
        {
            VssCredentials creds = new VssBasicCredential(string.Empty, credentials.pat);

            // Connect to Azure DevOps Services
            VssConnection connection = new VssConnection(new Uri(credentials.DevOpsURL), creds);

            // Construct the object containing field values required for the new work item
            JsonPatchDocument patchDocument = new JsonPatchDocument();

            if (!String.IsNullOrWhiteSpace(task.Description))
            {
                patchDocument.Add(
                   new JsonPatchOperation()
                   {
                       Operation = Operation.Replace,
                       Path = "/fields/System.Description",
                       Value = task.Description
                   }
               );
            }

            WorkItemTrackingHttpClient workItemTrackingClient = connection.GetClient<WorkItemTrackingHttpClient>();


            if (!String.IsNullOrWhiteSpace(task.AttachmentFile))
            {
                if (System.IO.File.Exists(task.AttachmentFile))
                {
                    // upload attachment to store and get a reference to that file
                    AttachmentReference attachmentReference = await workItemTrackingClient.CreateAttachmentAsync(task.AttachmentFile);

                    var attatchmentURL = attachmentReference.Url.Split('?')[0] + "?filename=" + System.IO.Path.GetFileName(task.AttachmentFile);

                    patchDocument.Add(
                        new JsonPatchOperation()
                        {
                            Operation = Operation.Add,
                            Path = "/fields/System.History",
                            Value = "Attach file"
                        }
                    );

                    patchDocument.Add(
                      new JsonPatchOperation()
                      {
                         Operation = Operation.Add,
                         Path = "/relations/-",
                         Value = new
                      {
                      rel = "AttachedFile",
                      url = attatchmentURL,
                      
                      attributes = new { comment = "Details :" + task.Title }
                      }
                      }
                    );
                }
            }
            WorkItem result = await workItemTrackingClient.UpdateWorkItemAsync(patchDocument, task.Id);

            return result;
        }

    }
}
