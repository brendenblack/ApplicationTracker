using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Domain
{
    public class SubmittedApplication : StatusWrapper
    {
        public SubmittedApplication(JobApplication application)
            : base(application) 
        {
            SupportedStatuses = new List<ApplicationStatuses> 
            { 
                ApplicationStatuses.INPROGRESS, 
                ApplicationStatuses.SUBMITTED
            };
        }

        public Result RecordOffer()
        {
            if (!CanTransition())
            {
                return Result.Fail(GENERIC_ERROR);
            }

            throw new NotImplementedException();
        }


    }
}
