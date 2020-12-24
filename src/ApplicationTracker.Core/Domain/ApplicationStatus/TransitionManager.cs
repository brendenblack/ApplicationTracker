using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Domain
{
    /// <summary>
    /// Provides meaningful facilities to manage the status of an Application and enforcing workflow rules.
    /// </summary>
    public class TransitionManager
    {
        public JobApplication Application { get; private set; }

        private string GENERIC_ERROR => $"Unable to perform this action while the application is in {Application.CurrentStatus}";
        private ApplicationStatuses initialStatus;

        public TransitionManager(JobApplication application)
        {
            Application = application;
            initialStatus = application.CurrentStatus;
        }

        public Result<StatusTransition> Submit(DateTime? submittedAt = null)
        {
            if (Application.CurrentStatus >= ApplicationStatuses.SUBMITTED)
            {
                return Result.Fail("");
            }

            var result = Application.Transition(ApplicationStatuses.SUBMITTED, submittedAt ?? DateTime.Now);
            return HandleTransitionResult(result);
        }


        [Obsolete]
        public Result<StatusTransition> Cancel(DateTime? effective = null)
        {
            var result = Application.Transition(ApplicationStatuses.CLOSED, effective ?? DateTime.Now, ApplicationResolution.CANCELLED);
            return HandleTransitionResult(result);
        }

        [Obsolete]
        public Result<StatusTransition> Rejected(DateTime? effective = null)
        {
            var result = Application.Transition(ApplicationStatuses.CLOSED, effective ?? DateTime.Now, ApplicationResolution.REJECTED);
            return HandleTransitionResult(result);
        }

        public Result<StatusTransition> Close(ApplicationResolution resolution, DateTime? effective = null)
        {
            var result = Application.Transition(ApplicationStatuses.CLOSED, effective ?? DateTime.Now, resolution);
            return HandleTransitionResult(result);
        }

        public Result<StatusTransition> Reopen()
        {
            if (Application.CurrentStatus != ApplicationStatuses.CLOSED)
            {
                return Result.Fail("Cannot reopen an application that is not closed.");
            }

            var result = Application.Transition(ApplicationStatuses.SUBMITTED, DateTime.Now, ApplicationResolution.UNRESOLVED);
            return HandleTransitionResult(result);
        }

        private Result<StatusTransition> HandleTransitionResult(Result<StatusTransition> result)
        {
            if (result.IsSuccess)
            {
                return Result.Ok(result.Value);
            }

            return Result.Fail(GENERIC_ERROR);
        }

        public bool HasTransitioned => initialStatus != Application.CurrentStatus;


    }
}
