using ApplicationTracker.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Persistence.Configurations
{
    public class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
    {
        public void Configure(EntityTypeBuilder<JobApplication> builder)
        {
            //builder.Property(a => a.Notes)
            //    .HasConversion(
            //        model => JsonConvert.SerializeObject(model, Formatting.None),
            //        db => JsonConvert.DeserializeObject<IReadOnlyCollection<string>>(db))
            //    .Metadata.SetValueComparer(new ValueComparer<IReadOnlyCollection<string>>((a, b) => a.Equals(b), c => c.GetHashCode()));

            builder.HasMany(a => a.Notes)
                .WithOne(e => e.Application)
                .HasForeignKey(e => e.ApplicationId);

            builder.OwnsOne(a => a.Location);
        }
    }
}
