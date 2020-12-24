using ApplicationTracker.Core.Common.Errors;
using ApplicationTracker.Core.Persistence;
using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Features.Applications.Commands
{
    public class AddOfferCommand : IRequest<Result<int>>
    {
        public int ApplicationId { get; set; }

        public DateTime ReceivedOn { get; set; }
    }

    public class AddOfferHandler : IRequestHandler<AddOfferCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;

        public AddOfferHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Handle(AddOfferCommand request, CancellationToken cancellationToken)
        {
            var application = await _context.Applications.FindAsync(request.ApplicationId, cancellationToken);
            
            if (application == null)
            {
                return Result.Fail(new NotFoundError(nameof(application), request.ApplicationId));
            }

            //var result = application.AddOffer(request.ReceivedOn);

            //await _context.SaveChangesAsync(cancellationToken);

            //if (result.IsFailed)
            //{
            //    // TODO
            //}

            //return Result.Ok(result.Value.Id);
            throw new NotImplementedException();
        }
    }
}
