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

namespace ApplicationTracker.Core.Features.JobSearches.Queries.GetJobSearch
{
    public class GetJobSearchQuery : IRequest<JobSearchDto>
    {
        public int Id { get; set; }
    }

    public class GetJobSearchHandler : IRequestHandler<GetJobSearchQuery, JobSearchDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetJobSearchHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<JobSearchDto> Handle(GetJobSearchQuery request, CancellationToken cancellationToken)
        {
            return await _context.JobSearches
                .Where(s => s.Id == request.Id)
                .Include(s => s.Applications)
                .ProjectTo<JobSearchDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }
    }
}
