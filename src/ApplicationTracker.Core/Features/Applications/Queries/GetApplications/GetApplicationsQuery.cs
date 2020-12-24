using ApplicationTracker.Core.Persistence;
using ApplicationTracker.Core.Domain;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Features.Applications.Queries.GetApplications
{
    public class GetApplicationsQuery : IRequest<GetApplicationsVm>
    {
        public int ParentSearchId { get; set; }
    }

    public class GetApplicationsHandler : IRequestHandler<GetApplicationsQuery, GetApplicationsVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetApplicationsHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetApplicationsVm> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
        {
            var applications = await _context.Applications
                .Where(a => a.ParentSearchId == request.ParentSearchId)
                .Include(a => a.Transitions)
                .ProjectTo<ApplicationSummaryDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new GetApplicationsVm
            {
                JobSearchId = request.ParentSearchId,
                Applications = applications
            };
        }
    }
}
