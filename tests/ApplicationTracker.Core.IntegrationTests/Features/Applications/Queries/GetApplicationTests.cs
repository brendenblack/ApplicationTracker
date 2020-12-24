using ApplicationTracker.Core.Domain;
using ApplicationTracker.Core.Features.Applications.Queries.GetApplication;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ApplicationTracker.Core.IntegrationTests.Features.Applications.Queries
{
    public class GetApplicationTests : TestBase
    {
        public GetApplicationTests(ITestOutputHelper output) : base(output)
        { }

        [Fact]
        public async Task ShouldReturn()
        {
            var search = new JobSearch()
            {
                Title = "Test search"
            };
            await AddAsync(search);
            var app = new JobApplication(search, "Microsoft", "Junior Developer");
            await AddAsync(app);

            var query = new GetApplicationQuery { Id = app.Id };

            var result = await SendAsync(query);

            result.ShouldNotBeNull();
            result.OrganizationName.ShouldBe("Microsoft");
            result.JobTitle.ShouldBe("Junior Developer");
        }
    }
}
