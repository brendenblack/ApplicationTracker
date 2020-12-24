using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ApplicationTracker.Core.Domain
{
    public class JobApplicationNote : AuditableEntity
    {
        protected JobApplicationNote() { }

        public JobApplicationNote(JobApplication application)
        {
            this.Application = application;
            this.ApplicationId = application.Id;
        }

        public int Id { get; private set; }

        public int ApplicationId { get; private set; }

        public virtual JobApplication Application { get; private set; }

        public DateTime Timestamp { get; set; }

        public string Subject { get; set; }

        public string Author { get; set; }

        public string Contents { get; set; }
    }
}
