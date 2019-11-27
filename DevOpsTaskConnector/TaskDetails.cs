using System;

namespace DevOpsTaskConnector
{
    public class TaskDetails
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string TaskType { get; set; }

        public string Tags { get; set; }

        public string AttachmentFile { get; set; }

        public Guid ProjectId { get; set; }
    }

}