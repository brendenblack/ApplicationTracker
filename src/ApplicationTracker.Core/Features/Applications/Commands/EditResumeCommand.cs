using ApplicationTracker.Core.Common.Errors;
using ApplicationTracker.Core.Domain;
using ApplicationTracker.Core.Persistence;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Features.Applications.Commands
{
    public class EditResumeCommand : IRequest<Result>
    {
        public int ApplicationId { get; set; }

        public string Contents { get; set; }

        public Formats Format { get; set; }
    }

    public class EditResumeHandler : IRequestHandler<EditResumeCommand, Result>
    {
        private readonly ILogger<EditResumeHandler> _logger;
        private readonly IApplicationDbContext _context;

        public EditResumeHandler(ILogger<EditResumeHandler> logger, IApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Result> Handle(EditResumeCommand request, CancellationToken cancellationToken)
        {
            var application = await _context.Applications.FindAsync(request.ApplicationId, cancellationToken);
            if (application == null)
            {
                return Result.Fail(new NotFoundError(nameof(JobApplication), request.ApplicationId));
            }

            // TODO: preserve history
            application.Resume = request.Contents;
            application.ResumeFormat = request.Format;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }
    }
}
