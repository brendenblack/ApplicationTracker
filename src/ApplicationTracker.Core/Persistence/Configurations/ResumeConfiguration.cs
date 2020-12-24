//using ApplicationTracker.Core.Domain;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.ChangeTracking;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ApplicationTracker.Core.Persistence.Configurations
//{
//    public class ResumeConfiguration : IEntityTypeConfiguration<Resume>
//    {
//        public void Configure(EntityTypeBuilder<Resume> builder)
//        {


//            ValueComparer<ICollection<string>> stringListComparer = new ValueComparer<ICollection<string>>(
//               (a, b) => a.Equals(b),
//               c => c.GetHashCode());

//            builder.Property(c => c.Tags)
//                .HasConversion(
//                    model => JsonConvert.SerializeObject(model, Formatting.None),
//                    db => JsonConvert.DeserializeObject<List<string>>(db))
//                .Metadata.SetValueComparer(stringListComparer);

            
//        }
//    }
//}
