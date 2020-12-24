using ApplicationTracker.Core.Persistence;
using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Features.Applications.Commands
{
    public class ScheduleInterviewCommand : IRequest<Result>
    {
        public int ApplicationId { get; set; }

        public List<string> Interviewers { get; set; } = new List<string>();

        public DateTime StartTime { get; set; }
        public TimeSpan ScheduledDuration { get; set; }

        public string Medium { get; set; }

        public string Description { get; set; }
    }

    public class ScheduleInterviewHandler : IRequestHandler<ScheduleInterviewCommand, Result>
    {
        private readonly IApplicationDbContext _context;

        public ScheduleInterviewHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(ScheduleInterviewCommand request, CancellationToken cancellationToken)
        {
            var application = await _context.Applications.FindAsync(request.ApplicationId, cancellationToken);
            //application.ScheduleInterview(request.StartTime, request.Medium, request.Interviewers, request.ScheduledDuration);
            throw new NotImplementedException();
        }
    }
}
