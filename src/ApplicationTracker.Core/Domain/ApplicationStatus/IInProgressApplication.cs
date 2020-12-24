using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Domain.ApplicationStatus
{
    public interface IApplication
    {
        Result<IClosedApplication> Cancel();

        Result<IClosedApplication> Cancel(DateTime effective);
    }

    public interface IInProgressApplication : IApplication
    {
        Result<ISubmittedApplication> Submit();

        Result<ISubmittedApplication> Submit(DateTime effective);
    }

    public interface ISubmittedApplication : IApplication
    {

    }

    public interface IClosedApplication : IApplication
    {

    }
}
