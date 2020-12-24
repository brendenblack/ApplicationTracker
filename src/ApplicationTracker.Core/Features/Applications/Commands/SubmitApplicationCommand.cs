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
    public class SubmitApplicationCommand : IRequest<Result<int>>
    {
        public int ApplicationId { get; set; }

        /// <summary>
        /// An HTML string representing the contents of the resumé submitted with this application.
        /// </summary>
        public string Resume { get; set; }

        /// <summary>
        /// An HTML string representing the contents of the resumé submitted with this application.
        /// </summary>
        public string CoverLetter { get; set; }

        /// <summary>
        /// When This application was submitted to the employer.
        /// </summary>
        public DateTime SubmittedOn { get; set; }
    }

    public class SubmitApplicationHandler : IRequestHandler<SubmitApplicationCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;

        public SubmitApplicationHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Handle(SubmitApplicationCommand request, CancellationToken cancellationToken)
        {
            var application = await _context.Applications
                .Include(a => a.Transitions)
                .FirstOrDefaultAsync(a => a.Id == request.ApplicationId, cancellationToken);

            if (application == null)
            {
                return Result.Fail("Unable to find application.");
            }

            if (!string.IsNullOrWhiteSpace(request.Resume))
            {
                application.Resume = request.Resume;
            }

            if (!string.IsNullOrWhiteSpace(request.CoverLetter))
            {
                application.CoverLetter = request.CoverLetter;
            }

            var result = new InProgressApplication(application).Submit(request.SubmittedOn);

            if (result.IsFailed)
            {
                return Result.Fail("An error occurred while submitting this application.").WithErrors(result.Errors);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Ok(application.Id);
        }
    }
}
