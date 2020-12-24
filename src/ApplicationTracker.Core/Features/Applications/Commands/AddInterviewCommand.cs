using ApplicationTracker.Core.Common.Errors;
using ApplicationTracker.Core.Domain;
using ApplicationTracker.Core.Domain.Exceptions;
using ApplicationTracker.Core.Persistence;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Features.Applications.Commands
{
    public class AddInterviewCommand : IRequest<Result<int>>
    {
        [Required]
        public int ApplicationId { get; set; }

        [Required]
        public DateTime ScheduledFor { get; set; }

        public string Medium { get; set; }

        public List<string> Interviewers { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }
    }
    public class AddInterviewHandler : IRequestHandler<AddInterviewCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;

        public AddInterviewHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Handle(AddInterviewCommand request, CancellationToken cancellationToken)
        {
            
            var application = await _context.Applications.FindAsync(request.ApplicationId);
            if (application == null)
            {
                return Result.Fail(new NotFoundError(nameof(JobApplication), request.ApplicationId));
            }

            var interview = application.ScheduleInterview(request.ScheduledFor, request.Medium, request.Interviewers, request.Duration);

            if (!interview.IsFailed)
            {
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Ok(interview.Value.Id);
            }

            return Result.Fail("").WithReasons(interview.Reasons); 
        }

        [Obsolete]
        public async Task<int> UpdateInterview(AddInterviewCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            //var interview = await _context.ApplicationEvents.FindAsync(request.InterviewId);
            //if (interview == null)
            //{
            //    throw new NotFoundException(nameof(JobApplicationEvent), request.InterviewId);
            //}

            //interview.Date = request.ScheduledFor;
            //// TODO

            //return interview.Id;
        }

    }
}
