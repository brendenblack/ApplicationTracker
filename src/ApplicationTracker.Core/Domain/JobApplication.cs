using ApplicationTracker.Core.Common.Errors;
using ApplicationTracker.Core.Common.ValueObjects;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ApplicationTracker.Core.Domain
{
    public enum Formats
    {
        PLAINTEXT,
        HTML,
        MARKDOWN
    }

    public class JobApplication : AuditableEntity
    {
        private JobApplication() { }

        public JobApplication(JobSearch parentSearch, string organization, string title)
        {
            ParentSearchId = parentSearch.Id;
            ParentSearch = parentSearch;
            OrganizationName = organization;
            JobTitle = title;

            var creation = new StatusTransition(this, ApplicationStatuses.CREATED, ApplicationStatuses.INPROGRESS, DateTime.Now);
            ((List<StatusTransition>)Transitions).Add(creation);
        }

        #region Basic details
        public int Id { get; set; }

        public int ParentSearchId { get; private set; }

        public virtual JobSearch ParentSearch { get; private set; }

        public Location Location { get; set; }

        public string OrganizationName { get; set; }

        public string JobTitle { get; set; }

        public string Resume { get; set; }

        public Formats ResumeFormat = Formats.PLAINTEXT;

        public string CoverLetter { get; set; }

        /// <summary>
        /// A detailed description of the duties and responsibilities of the position, as well as the requirements for the role.
        /// </summary>
        public string JobDescription { get; set; }

        public Formats JobDescriptionFormat = Formats.PLAINTEXT;

        /// <summary>
        /// Where this job opportunity was discovered.
        /// </summary>
        public string Source { get; set; }
        #endregion

        #region Status & notes
        public ApplicationStatuses CurrentStatus
        {
            get
            {
                var latest = Transitions
                    .OrderByDescending(t => t.Timestamp)
                    .FirstOrDefault();

                return latest?.TransitionTo ?? ApplicationStatuses.UNKNOWN;
            }
        }

        public ApplicationResolution Resolution
        {
            get
            {
                var latest = Transitions
                    .OrderByDescending(t => t.Timestamp)
                    .FirstOrDefault();

                return latest?.Resolution ?? ApplicationResolution.UNRESOLVED;
            }
        }

        public bool IsClosed => CurrentStatus == ApplicationStatuses.CLOSED;

        public virtual IReadOnlyCollection<StatusTransition> Transitions { get; } = new List<StatusTransition>();

        /// <summary>
        /// Transition this application from one status to another.
        /// </summary>
        /// <remarks>
        /// This method is agnostic about workflows, and will only enforce that you cannot transition from one statu to the same status.
        /// Use an appropriate class that derives from <see cref="StatusWrapper"/> (e.g. <see cref="SubmittedApplication"/>) as helpers to more
        /// effective manage the status.
        /// </remarks>
        /// <param name="transitionTo"></param>
        /// <param name="effectiveAsOf"></param>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public Result<StatusTransition> Transition(ApplicationStatuses transitionTo, DateTime effectiveAsOf, ApplicationResolution resolution = ApplicationResolution.UNRESOLVED)
        {
            // CAUTION: if the inner logic of this method changes, you may have to revisit the first transition created
            // in the ctor

            if (CurrentStatus == transitionTo)
            {
                return Result.Fail("Unable to transition to the current status.");
            }

            if (transitionTo != ApplicationStatuses.CLOSED)
            {
                resolution = ApplicationResolution.UNRESOLVED;
            }

            var transition = new StatusTransition(this, CurrentStatus, transitionTo, effectiveAsOf, resolution);
            ((List<StatusTransition>)Transitions).Add(transition);
            return Result.Ok(transition);
        }



        /// <summary>
        /// Adds an interview record to this job application.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="interviewMedium">What medium the interview took place over. See <see cref="InterviewTypes"/> for standard mediums.</param>
        /// <param name="interviewers"></param>
        public Result<JobApplicationNote> ScheduleInterview(DateTime start, string interviewMedium, IEnumerable<string> interviewers, TimeSpan duration, string description = "")
        {
            var interview = new JobApplicationNote(this)
            {
                Timestamp = start,
                Author = OrganizationName,
                Contents = $"Interview<br/>Start time: ${start}<br />Duration: ${duration}<br />Medium: ${interviewMedium}<br />Interviewers: ${string.Join(',', interviewers)}",
            };
            
            if (!string.IsNullOrWhiteSpace(description))
            {
                interview.Contents += $"<br />{description}";
            }

            ((List<JobApplicationNote>)Notes).Add(interview);

            return Result.Ok(interview);
        }
        #endregion

        public bool HasOffer => Offer != null;
        public virtual Offer Offer { get; private set; }

        public Result<Offer> AddOffer(DateTime extended, string details)
        {
            Offer = new Offer(this)
            {
                Extended = extended,
                Expires = extended.AddDays(30),
                Details = details,
            };

            return Result.Ok(Offer);
        }

        public Result RemoveNote(int id)
        {
            var applicationEvent = Notes.FirstOrDefault(e => e.Id == id);
            if (applicationEvent != null)
            {
                return RemoveNote(applicationEvent);
            }

            return Result.Fail("");
        }

        public Result RemoveNote(JobApplicationNote applicationEvent)
        {
            var removed = ((List<JobApplicationNote>)Notes).Remove(applicationEvent);
            return Result.FailIf(!removed, "");
        }

        /// <summary>
        /// Records a note about this application.
        /// </summary>
        /// <param name="contents"></param>
        /// <returns></returns>
        public Result<JobApplicationNote> AddNote(string contents)
        {
            var note = new JobApplicationNote(this)
            {
                Author = "self",
                Timestamp = DateTime.Now,
                Contents = contents
            };

            ((List<JobApplicationNote>)Notes).Add(note);

            return Result.Ok(note);
        }

        /// <summary>
        /// Creates a record of contact from the potential employer.
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="author"></param>
        /// <param name="received">When this contact took place.</param>
        /// <returns></returns>
        public Result<JobApplicationNote> RecordEmployerContact(string contents, string author = "", string subject = "", DateTime? received = null)
        {
            var note = new JobApplicationNote(this)
            {
                Author = (string.IsNullOrWhiteSpace(author)) ? OrganizationName : author,
                Timestamp = received ?? DateTime.Now,
                Contents = contents
            };

            ((List<JobApplicationNote>)Notes).Add(note);

            return Result.Ok(note);
        }
    
        public IReadOnlyCollection<JobApplicationNote> Notes { get; private set; } = new List<JobApplicationNote>();

    }

    public enum ApplicationStatuses
    {
        UNKNOWN,
        /// <summary>
        /// A newly created application that is not yet in progress.
        /// </summary>
        CREATED,
        /// <summary>
        /// Indicates that the application is still be constructed, and has not yet been submitted to the employer.
        /// </summary>
        INPROGRESS,
        /// <summary>
        /// The application has been submitted to the employer, but there has been no feedback from them about next steps.
        /// </summary>
        SUBMITTED,
        /// <summary>
        /// The application has been closed and is no longer under consideration.
        /// </summary>
        CLOSED
    }

    public enum ApplicationResolution
    {
        /// <summary>
        /// The application is still in progress.
        /// </summary>
        UNRESOLVED,
        WITHDRAWN,
        CANCELLED,
        REJECTED,
        OFFER_DECLINED,
        OFFER_ACCEPTED
    }
}
