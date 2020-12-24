using ApplicationTracker.Core.Domain;
using ApplicationTracker.Core.Features.Applications.Queries.GetApplications;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ApplicationTracker.Core.IntegrationTests.Features.Applications.Queries
{
    public class GetApplicationsTests : TestBase
    {
        [Fact]
        public async Task ShouldReturnResults()
        {
            var search = new JobSearch
            {
                Title = "Search test"
            };
            //await AddAsync(search);
            var a1 = new JobApplication(search, "Microsoft", "CEO");
            var a2 = new JobApplication(search, "Salesforce", "Janitor");
            await AddAsync(a1);
            await AddAsync(a2);

            var query = new GetApplicationsQuery { ParentSearchId = search.Id };

            var result = await SendAsync(query);

            result.Applications.Count.ShouldBe(2);
            result.Applications.Where(a => a.OrganizationName == "Microsoft").Any().ShouldBeTrue();
            result.Applications.Where(a => a.OrganizationName == "Salesforce").Any().ShouldBeTrue();
        }
    }
}
