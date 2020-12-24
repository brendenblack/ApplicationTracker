using ApplicationTracker.Core.Common.Mappings;
using ApplicationTracker.Core.Domain;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Features.JobSearches.Queries.GetJobSearches
{
    public class JobSearchSummaryDto : IMapFrom<JobSearch>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime Created { get; set; }

        public int ApplicationsCount { get; set; }

        //public void MapFrom(Profile profile)
        //{
        //    profile.CreateMap<JobSearch, JobSearchSummaryDto>()
        //        .ForMember(s => s.ApplicationsCount)
        //}
    }
}
