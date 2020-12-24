using ApplicationTracker.Core.Common.Mappings;
using ApplicationTracker.Core.Domain;

namespace ApplicationTracker.Core.Features.JobSearches.Queries.GetJobSearch
{
    public class JobSearchDto : IMapFrom<JobSearch>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}