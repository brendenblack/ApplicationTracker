using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Domain
{
    public class UnderConsiderationApplication : StatusWrapper
    {
        public UnderConsiderationApplication(JobApplication application)
            : base(application) 
        {
            //SupportedStatus = ApplicationStatuses.UNDER_CONSIDERATION;
        }

        public Result<ClosedApplication> Reject(DateTime timestamp)
        {

            var result = Application.Transition(ApplicationStatuses.CLOSED, timestamp, ApplicationResolution.REJECTED);
            if (result.IsSuccess)
            {
                return Result.Ok(new ClosedApplication(Application));
            }

            return Result.Fail(GENERIC_ERROR);
        }
    }
}
