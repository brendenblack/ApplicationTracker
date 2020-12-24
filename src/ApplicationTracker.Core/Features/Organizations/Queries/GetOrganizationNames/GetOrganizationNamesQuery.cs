using ApplicationTracker.Core.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Features.Organizations.Queries.GetOrganizationNames
{
    public class GetOrganizationNamesQuery : IRequest<OrganizationNamesVm>
    {
    }

    public class GetOrganizationNamesHandler : IRequestHandler<GetOrganizationNamesQuery, OrganizationNamesVm>
    {
        private readonly IApplicationDbContext _context;

        public GetOrganizationNamesHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrganizationNamesVm> Handle(GetOrganizationNamesQuery request, CancellationToken cancellationToken)
        {
            var names = await _context.Applications
                .Select(a => a.OrganizationName)
                .Distinct()
                .ToListAsync();

            return new OrganizationNamesVm { Names = names };
        }
    }
}
