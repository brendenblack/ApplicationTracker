using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Common.Interfaces
{
    public interface IDateTimeService
    {
        DateTime Now { get; } 
    }
}
