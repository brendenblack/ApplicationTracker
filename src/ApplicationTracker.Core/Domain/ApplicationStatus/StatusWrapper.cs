using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Domain
{
    public abstract class StatusWrapper
    {
        public JobApplication Application { get; protected set; }

        protected readonly string GENERIC_ERROR;

        public IReadOnlyCollection<ApplicationStatuses> SupportedStatuses { get; protected set; }

        public StatusWrapper(JobApplication application)
        {
            Application = application;
            GENERIC_ERROR = $"Unable to perform this action while the application is in {Application.CurrentStatus}";
        }

        public virtual bool CanTransition()
        {
            return SupportedStatuses.Contains(Application.CurrentStatus);
        }
    }
}
