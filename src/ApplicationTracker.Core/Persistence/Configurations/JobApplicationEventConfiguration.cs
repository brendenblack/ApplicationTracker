using ApplicationTracker.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Persistence.Configurations
{
    public class JobApplicationEventConfiguration : IEntityTypeConfiguration<JobApplicationNote>
    {
        public void Configure(EntityTypeBuilder<JobApplicationNote> builder)
        {
        //    builder.HasDiscriminator(e => e.EventType)
        //        //.HasValue<JobApplicationEvent>(EventTypes.OTHER)
                //.HasValue<StatusTransition>(EventTypes.TRANSITION);

            //builder.Property(a => a.Notes)
            //    .HasConversion(
            //        model => JsonConvert.SerializeObject(model, Formatting.None),
            //        db => JsonConvert.DeserializeObject<List<string>>(db))
            //    .Metadata.SetValueComparer(ConfigurationsHelper.ListComparer);
        }
    }
}
