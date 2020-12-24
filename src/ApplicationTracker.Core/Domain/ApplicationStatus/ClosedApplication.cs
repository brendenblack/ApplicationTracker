using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Domain
{
    public class ClosedApplication : StatusWrapper
    {
        public ClosedApplication(JobApplication application)
            : base(application)
        {
            SupportedStatuses = new List<ApplicationStatuses> { ApplicationStatuses.CLOSED };
        }
    }
}
