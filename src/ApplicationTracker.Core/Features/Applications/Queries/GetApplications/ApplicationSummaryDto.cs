using ApplicationTracker.Core.Domain;
using ApplicationTracker.Core.Common.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace ApplicationTracker.Core.Features.Applications.Queries.GetApplications
{
    public class ApplicationSummaryDto : IMapFrom<JobApplication>
    {
        public int Id { get; set; }
        public string OrganizationName { get; set; }

        public DateTime Created { get; set; }

        public DateTime? LastModified { get; set; }

        public string JobTitle { get; set; }

        public ApplicationStatuses CurrentStatus { get; set; }

        public ApplicationResolution Resolution { get; set; }

        public bool HasResume { get; set; }

        public bool HasCoverLetter { get; set; }

        public bool IsRemote { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<JobApplication, ApplicationSummaryDto>()
                .ForMember(m => m.HasResume, opt => opt.MapFrom(s => !string.IsNullOrWhiteSpace(s.Resume)))
                .ForMember(m => m.HasCoverLetter, opt => opt.MapFrom(s => !string.IsNullOrWhiteSpace(s.CoverLetter)))
                .ForMember(m => m.IsRemote, opt => opt.MapFrom(s => s.Location != null && s.Location.CityName.Equals("remote", StringComparison.InvariantCultureIgnoreCase)));
        }
    }
}
