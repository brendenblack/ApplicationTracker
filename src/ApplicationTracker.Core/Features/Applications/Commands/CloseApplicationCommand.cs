using ApplicationTracker.Core.Domain;
using ApplicationTracker.Core.Persistence;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Features.Applications.Commands
{
    public enum CloseTypes
    {
        REJECTED
    }

    public class CloseApplicationCommand : IRequest<Result>
    {
        public int ApplicationId { get; set; }

        public ApplicationResolution ResolutionType { get; set; }

        public string Description { get; set; }

        public string EmployerNote { get; set; }

        public DateTime EffectiveTime { get; set; } = DateTime.Now;
    }

    public class CloseApplicationHandler : IRequestHandler<CloseApplicationCommand, Result>
    {
        private readonly ILogger<CloseApplicationHandler> _logger;
        private readonly IApplicationDbContext _context;

        public CloseApplicationHandler(ILogger<CloseApplicationHandler> logger, IApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Result> Handle(CloseApplicationCommand request, CancellationToken cancellationToken)
        {
            var application = await _context.Applications
                .Where(a => a.Id == request.ApplicationId)
                .Include(a => a.Transitions)
                .FirstOrDefaultAsync(cancellationToken);

            var transitionManager = new TransitionManager(application);

            transitionManager.Close(request.ResolutionType, request.EffectiveTime);

            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                application.AddNote(request.Description);
            }

            if (request.ResolutionType == ApplicationResolution.REJECTED)
            {
                application.RecordEmployerContact(request.EmployerNote, application.OrganizationName, "Re: Rejection", request.EffectiveTime);
            }
            else
            {
                _logger.LogWarning("An employer note was provided but the resolution was not 'rejected'. The note has not been persisted.");
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }
    }
}
