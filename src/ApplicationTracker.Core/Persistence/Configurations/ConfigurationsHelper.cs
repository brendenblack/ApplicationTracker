using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Persistence.Configurations
{
    public class ConfigurationsHelper
    {
        public static ValueComparer<IReadOnlyCollection<string>> ReadOnlyCollectionComparer => 
            new ValueComparer<IReadOnlyCollection<string>>((a, b) => a.Equals(b), c => c.GetHashCode());

        public static ValueComparer<List<string>> ListComparer =>
            new ValueComparer<List<string>>((a, b) => a.Equals(b), c => c.GetHashCode());
    }
}
