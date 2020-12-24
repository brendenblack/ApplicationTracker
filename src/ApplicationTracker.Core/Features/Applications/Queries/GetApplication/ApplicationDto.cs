using ApplicationTracker.Core.Domain;
using ApplicationTracker.Core.Common.Mappings;
using System;
using System.Collections.Generic;

namespace ApplicationTracker.Core.Features.Applications.Queries.GetApplication
{
    public class ApplicationDto : IMapFrom<JobApplication>
    {
        public int Id { get; set; }

        public int ParentSearchId { get; set; }

        public string ParentSearchTitle { get; set; }

        public string OrganizationName { get; set; }

        public DateTime SubmittedOn { get; set; }

        public DateTime Created { get; set; }

        public DateTime? LastModified { get; set; }

        public ApplicationStatuses CurrentStatus { get; set; }

        public ApplicationResolution Resolution { get; set; }

        public string LocationCityName { get; set; }

        public string LocationProvince { get; set; }

        public string Resume { get; set; }

        public string CoverLetter { get; set; }

        public string JobTitle { get; set; }

        public bool IsClosed { get; set; }

        public string JobDescription { get; set; }

        public List<NoteDto> Notes { get; set; }

        public List<StatusTransitionDto> Transitions { get; set; }
    }
}