using ApplicationTracker.Core.Domain;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApplicationTracker.Core.UnitTests.Domain.JobApplicationTests
{
    public class CurrentStatus
    {
        [Fact]
        public void NewlyCreated_ShouldReturnINPROGRESS()
        {
            var search = new JobSearch();
            var app = new JobApplication(search, "", "");

            app.CurrentStatus.ShouldBe(ApplicationStatuses.INPROGRESS);
        }
    }
}
