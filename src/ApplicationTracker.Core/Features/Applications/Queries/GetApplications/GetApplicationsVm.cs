using System.Collections.Generic;

namespace ApplicationTracker.Core.Features.Applications.Queries.GetApplications
{
    public class GetApplicationsVm
    {
        public int JobSearchId { get; set; }
        public List<ApplicationSummaryDto> Applications { get; set; }
    }
}