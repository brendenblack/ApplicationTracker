using ApplicationTracker.Core.Domain;
using ApplicationTracker.Core.Features.Applications.Commands.UpsertApplication;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ApplicationTracker.Core.IntegrationTests.Features.Applications.Commands
{
    public class UpsertApplicationTests : TestBase
    {
        public UpsertApplicationTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async Task ShouldCreateWhenNoResumeIdSpecified()
        {
            var command = new UpsertApplicationCommand
            {
                Description = "A bland description",
                JobTitle = "Worker",
                AppliedOn = DateTime.Now,
                OrganizationName = "Company Co."
            };

            var createdId = await SendAsync(command);

            var app = await FindAsync<JobApplication>(createdId);
            app.JobDescription.ShouldBe("A bland description");
            app.JobTitle.ShouldBe("Worker");
            app.OrganizationName.ShouldBe("Company Co.");
        }

        [Fact]
        public async Task ShouldReturnUpdatedId()
        {
            var search = new JobSearch
            {
                Title = "Test Search"
            };
            await AddAsync(search);
            var app = new JobApplication(search, "Organization", "Job Title");
            await AddAsync(app);

            var command = new UpsertApplicationCommand
            {
                Id = app.Id,
                OrganizationName = "Organization 2",
                JobTitle = "Updated Job Title",
                Description = "Adding a description."
            };

            var updatedId = await SendAsync(command);

            updatedId.ShouldBe(app.Id);
            var updatedApp = await FindAsync<JobApplication>(updatedId);
            updatedApp.ShouldNotBeNull();
        }

        [Fact]
        public async Task WhenExists_ShouldUpdateFields()
        {
            var search = new JobSearch
            {
                Title = "Test Search"
            };
            await AddAsync(search);
            var app = new JobApplication(search, "Organization", "Job Title");
            await AddAsync(app);

            var command = new UpsertApplicationCommand
            {
                Id = app.Id,
                OrganizationName = "Organization 2",
                JobTitle = "Updated Job Title",
                Description = "Adding a description."
            };

            var updatedId = await SendAsync(command);

            updatedId.ShouldBe(app.Id);
            var updatedApp = await FindAsync<JobApplication>(updatedId);

            updatedApp.OrganizationName.ShouldBe("Organization 2");
            updatedApp.JobTitle = "Updated Job Title";
            updatedApp.JobDescription.ShouldBe("Adding a description.");
        }
    }
}
