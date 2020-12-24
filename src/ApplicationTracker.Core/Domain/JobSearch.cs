using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationTracker.Core.Domain
{
    public class JobSearch : AuditableEntity
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public virtual ICollection<JobApplication> Applications { get; set; }
    }
}
