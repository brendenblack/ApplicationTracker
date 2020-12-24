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
    public class RecordOfferCommand : IRequest<Result>
    {
        public int ApplicationId { get; set; }

        public decimal Salary { get; set; }

        public DateTime StartDate { get; set; }

        public string Description { get; set; }
    }

    public class RecordOfferHandler : IRequestHandler<RecordOfferCommand, Result>
    {
        private readonly ILogger<RecordOfferHandler> _logger;
        private readonly IApplicationDbContext _context;

        public RecordOfferHandler(ILogger<RecordOfferHandler> logger, IApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Result> Handle(RecordOfferCommand request, CancellationToken cancellationToken)
        {
            var application = await _context.Applications
                .Where(a => a.Id == request.ApplicationId)
                .Include(a => a.Transitions)
                .FirstOrDefaultAsync();

            if (application == null)
            {
                return Result.Fail($"Unable to find an application with id {request.ApplicationId}");
            }

            // perform a catch up transition
            if (application.CurrentStatus == ApplicationStatuses.INPROGRESS)
            {
                _logger.LogWarning($"Submitting application {application.Id} from {application.CurrentStatus}, suggests workflow is being manipulated unexpectedly.");
                new InProgressApplication(application).Submit();
            }

            if (application.CurrentStatus != ApplicationStatuses.SUBMITTED)
            {
                return Result.Fail($"Cannot add an offer to an application in {application.CurrentStatus}");
            }

            application.RecordEmployerContact(request.Description);
            application.AddOffer(DateTime.Now, request.Description);

            throw new NotImplementedException();
        }
    }
}
