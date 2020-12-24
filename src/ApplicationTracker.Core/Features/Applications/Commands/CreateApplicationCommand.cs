using ApplicationTracker.Core.Common.Errors;
using ApplicationTracker.Core.Domain;
using ApplicationTracker.Core.Persistence;
using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Features.Applications.Commands
{
    public class CreateApplicationCommand : IRequest<Result<int>>
    {
        [Required]
        public int ParentSearchId { get; set; }

        [Required]
        public string OrganizationName { get; set; }

        [Required]
        public string JobTitle { get; set; }

        public string Description { get; set; }

        public Formats DescriptionFormat { get; set; } = Formats.PLAINTEXT;

        public string Resume { get; set; }

        public Formats ResumeFormat { get; set; } = Formats.PLAINTEXT;

        public string Source { get; set; }
    }

    public class CreateApplicationHandler : IRequestHandler<CreateApplicationCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context;

        public CreateApplicationHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
        {
            var parent = await _context.JobSearches.FindAsync(request.ParentSearchId);

            if (parent == null)
            {
                return Result.Fail(new NotFoundError(nameof(JobSearches), request.ParentSearchId));
            }

            JobApplication application = new JobApplication(parent, request.OrganizationName, request.JobTitle)
            {
                OrganizationName = request.OrganizationName,
                JobTitle = request.JobTitle,
                JobDescription = request.Description,
                JobDescriptionFormat = request.DescriptionFormat,
                Source = request.Source,
                Resume = request.Resume,
                ResumeFormat = request.ResumeFormat,
            };
    
            _context.Applications.Add(application);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Ok(application.Id);
        }
    }
}
