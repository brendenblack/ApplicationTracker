using ApplicationTracker.Core.Persistence;
using ApplicationTracker.Core.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ApplicationTracker.Core.Features.Applications.Commands.UpsertApplication
{
    public class UpsertApplicationCommand : IRequest<int>
    {
        public int ParentSearchId { get; set; }

        public int Id { get; set; }

        [Required]
        public string OrganizationName { get; set; }

        [Required]
        public string JobTitle { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime AppliedOn { get; set; }

        public string Source { get; set; }
    }

    public class UpsertApplicationHandler : IRequestHandler<UpsertApplicationCommand, int>
    {
        private readonly ILogger<UpsertApplicationHandler> _logger;
        private readonly IApplicationDbContext _context;

        public UpsertApplicationHandler(ILogger<UpsertApplicationHandler> logger, IApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<int> Handle(UpsertApplicationCommand request, CancellationToken cancellationToken)
        {
            var parent = await _context.JobSearches.FindAsync(request.ParentSearchId);

            if (parent == null)
            {
                // TODO
                return 0;
            }

            JobApplication application;
            if (request.Id > 0)
            {
                _logger.LogDebug("Updating existing job application with id {}", request.Id);
                application = await _context.Applications
                    .Where(a => a.Id == request.Id)
                    .FirstAsync();
            }
            else
            {
                _logger.LogDebug("Creating new job application.");
                application = new JobApplication(parent, request.OrganizationName, request.JobTitle);
                _context.Applications.Add(application);
            }

            application.OrganizationName = request.OrganizationName;
            application.JobTitle = request.JobTitle;
            application.JobDescription = request.Description;

            await _context.SaveChangesAsync(cancellationToken);

            return application.Id;
            
        }
    }
}
