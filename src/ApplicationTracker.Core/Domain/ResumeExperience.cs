using ApplicationTracker.Core.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Domain
{
    public class ResumeExperience
    {
        public string OrganizationName { get; set; }

        public string JobTitle { get; set; }

        public Location Location { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndTime { get; set; }

        public bool IsCurrent { get; set; }

        public ICollection<string> Details { get; set; } = new List<string>();
    }
}
