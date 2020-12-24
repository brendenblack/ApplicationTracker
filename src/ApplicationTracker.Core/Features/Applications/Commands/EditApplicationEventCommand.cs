using ApplicationTracker.Core.Domain;
using ApplicationTracker.Core.Domain.Exceptions;
using ApplicationTracker.Core.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Features.Applications.Commands.UpsertInterview
{
    public class EditApplicationEventCommand : IRequest<int>
    {
        public int Id { get; set; }

        public DateTime Time { get; set; }

        public string Medium { get; set; }

        public List<string> Interviewers { get; set; }

        public TimeSpan Duration { get; set; } = TimeSpan.FromHours(1);
    }

    public class EditApplicationEventHandler : IRequestHandler<EditApplicationEventCommand, int>
    {
        private readonly ILogger<EditApplicationEventHandler> _logger;
        private readonly IApplicationDbContext _context;

        public EditApplicationEventHandler(ILogger<EditApplicationEventHandler> logger, IApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<int> Handle(EditApplicationEventCommand request, CancellationToken cancellationToken)
        {
            JobApplicationNote model = await _context.ApplicationNotes.FindAsync(request.Id);
            // TODO
            if (model == null)
            {
                throw new NotFoundException(nameof(JobApplicationNote), request.Id);
            }




            throw new NotImplementedException();
        }
    }
}
