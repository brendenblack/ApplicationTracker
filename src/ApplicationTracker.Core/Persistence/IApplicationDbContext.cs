using ApplicationTracker.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<JobSearch> JobSearches { get; }

        DbSet<JobApplication> Applications { get; } 

        DbSet<JobApplicationNote> ApplicationNotes { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
