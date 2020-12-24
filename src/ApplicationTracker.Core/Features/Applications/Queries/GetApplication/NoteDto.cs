using ApplicationTracker.Core.Domain;
using ApplicationTracker.Core.Common.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Features.Applications.Queries.GetApplication
{
    public class NoteDto : IMapFrom<JobApplicationNote>
    {
        public int Id { get; private set; }

        //public int ApplicationId { get; private set; }

        public DateTime Created { get; set; }
        
        public DateTime LastModified { get; set; }

        public DateTime Timestamp { get; set; }

        public string Author { get; set; }

        public string Contents { get; set; }
    }
}
