using ApplicationTracker.Core.Domain;
using ApplicationTracker.Core.Persistence;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Features.Applications.Commands
{
    public class CancelApplicationCommand : IRequest<Result>
    {
        public int ApplicationId { get; set; }

        public DateTime EffectiveTime { get; set; }
    }

    public class CancelApplicationHandler : IRequestHandler<CancelApplicationCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public CancelApplicationHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(CancelApplicationCommand request, CancellationToken cancellationToken)
        {
            var application = await _context.Applications
                .Where(a => a.Id == request.ApplicationId)
                .Include(a => a.Transitions)
                .FirstOrDefaultAsync();

            var result = application.Transition(ApplicationStatuses.CLOSED, request.EffectiveTime, ApplicationResolution.CANCELLED);

            return (result.IsSuccess) ? Result.Ok() : Result.Fail("");
        }
    }
}
