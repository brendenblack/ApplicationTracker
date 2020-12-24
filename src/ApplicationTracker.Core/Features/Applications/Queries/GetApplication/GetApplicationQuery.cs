using ApplicationTracker.Core.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Features.Applications.Queries.GetApplication
{
    public class GetApplicationQuery : IRequest<ApplicationDto>
    {
        public int Id { get; set; }
    }

    public class GetApplicationHandler : IRequestHandler<GetApplicationQuery, ApplicationDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetApplicationHandler> _logger;

        public GetApplicationHandler(IApplicationDbContext context, IMapper mapper, ILogger<GetApplicationHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApplicationDto> Handle(GetApplicationQuery request, CancellationToken cancellationToken)
        {
            return await _context.Applications
                .Where(a => a.Id == request.Id)
                .Include(a => a.Notes)
                .Include(a => a.Transitions)
                .ProjectTo<ApplicationDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
