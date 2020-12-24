using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Common.ValueObjects
{
    public class InterviewRecord
    {
        public DateTime ScheduledTime { get; set; }

        public TimeSpan ScheduledDuration { get; set; }

        public List<string> Interviewers { get; set; }
    }
}
