using ApplicationTracker.Core.Domain;
using ApplicationTracker.Core.Features.Applications.Commands;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ApplicationTracker.Core.IntegrationTests.Features.Applications.Commands
{
    public class SubmitApplicationTests : TestBase
    {
        public SubmitApplicationTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async Task ResultShouldBeSuccess()
        {
            var search = new JobSearch();
            var app = new JobApplication(search, "Microsoft", "Developer");
            await AddAsync(app);
            var command = new SubmitApplicationCommand { ApplicationId = app.Id, SubmittedOn = DateTime.Now };

            var response = await SendAsync(command);

            response.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task ShouldSubmit()
        {
            var search = new JobSearch();
            var app = new JobApplication(search, "Microsoft", "Developer");
            await AddAsync(app);
            var command = new SubmitApplicationCommand { ApplicationId = app.Id, SubmittedOn = DateTime.Now };

            await SendAsync(command);

            var submittedApp = await FindAsync<JobApplication>(app.Id, collectionIncludes: new string[] { nameof(JobApplication.Transitions) });

            submittedApp.CurrentStatus.ShouldBe(ApplicationStatuses.SUBMITTED);
        }
    }
}
