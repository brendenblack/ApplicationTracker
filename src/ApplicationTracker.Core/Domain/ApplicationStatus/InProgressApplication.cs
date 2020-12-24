using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Domain
{
    /// <summary>
    /// Provides facilities to manage the status of an application that is <see cref="ApplicationStatuses.INPROGRESS"/>.
    /// </summary>
    public class InProgressApplication : StatusWrapper
    {
        public InProgressApplication(JobApplication application)
            : base(application)
        {
            SupportedStatuses = new List<ApplicationStatuses> { ApplicationStatuses.INPROGRESS };
        }

        /// <summary>
        /// Marks the application as <see cref="ApplicationStatuses.SUBMITTED"/>, indicating that it has been
        /// submitted to the prospective employer.
        /// </summary>
        /// <returns></returns>
        public Result<SubmittedApplication> Submit()
        {
            return Submit(DateTime.Now);
        }

        /// <summary>
        /// Marks the application as <see cref="ApplicationStatuses.SUBMITTED"/>, indicating that it has been
        /// submitted to the prospective employer.
        /// </summary>
        /// <param name="submittedAt"></param>
        /// <returns></returns>
        public Result<SubmittedApplication> Submit(DateTime submittedAt)
        {
            if (CanTransition())
            {
                var result = Application.Transition(ApplicationStatuses.SUBMITTED, submittedAt);
                if (result.IsSuccess)
                {
                    return Result.Ok(new SubmittedApplication(Application));
                }
            }

            return Result.Fail(GENERIC_ERROR);            
        }

        /// <summary>
        /// Marks the application as <see cref="ApplicationStatuses.CLOSED"/> without ever having been submitted
        /// to the prospective employer.
        /// </summary>
        /// <returns></returns>
        public Result Cancel()
        {
            return Cancel(DateTime.Now);
        }

        /// <summary>
        /// Marks the application as <see cref="ApplicationStatuses.CLOSED"/> without ever having been submitted
        /// to the prospective employer.
        /// </summary>
        /// <param name="effectiveAsOf"></param>
        /// <returns></returns>
        public Result Cancel(DateTime effectiveAsOf)
        {
            return Application.Transition(ApplicationStatuses.CLOSED, effectiveAsOf, ApplicationResolution.CANCELLED);
        }


    }
}
