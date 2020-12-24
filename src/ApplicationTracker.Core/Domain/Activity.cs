using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Domain
{
    public class Activity
    {
        /// <summary>
        /// This activity can only be recorded when the parent is in one of the listed statuses.
        /// </summary>
        public ICollection<ApplicationStatuses> ValidStatuses { get; }
    }
}
