using ApplicationTracker.Core.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Features.JobSearches.Queries.GetJobSearches
{
    public class GetJobSearchesQuery : IRequest<JobSearchesVm>
    {
        public int? Limit { get; set; }
    }

    public class GetJobSearchesHandler : IRequestHandler<GetJobSearchesQuery, JobSearchesVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetJobSearchesHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<JobSearchesVm> Handle(GetJobSearchesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.JobSearches
                .Include(s => s.Applications)
                .ProjectTo<JobSearchSummaryDto>(_mapper.ConfigurationProvider);

            if (request.Limit.HasValue)
            {
                query = query.Take(request.Limit.Value);
            }

            var dtos = await query.ToListAsync();

            return new JobSearchesVm
            {
                Searches = dtos
            };
        }
    }
}
