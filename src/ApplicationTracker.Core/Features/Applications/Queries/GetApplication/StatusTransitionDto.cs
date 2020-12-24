using ApplicationTracker.Core.Common.Mappings;
using ApplicationTracker.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationTracker.Core.Features.Applications.Queries.GetApplication
{
    public class StatusTransitionDto : IMapFrom<StatusTransition>
    {
        public int Id { get; set; }

        public DateTime Timestamp { get; private set; }

        public ApplicationStatuses TransitionTo { get; private set; }

        public ApplicationStatuses TransitionFrom { get; private set; }

        public ApplicationResolution Resolution { get; private set; }
    }
}
