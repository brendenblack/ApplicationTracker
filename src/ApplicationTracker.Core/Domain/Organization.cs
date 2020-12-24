using ApplicationTracker.Core.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Domain
{
    /// <summary>
    /// Models an organization (i.e. a company) that is hiring.
    /// </summary>
    public class Organization
    {
        private Organization() { }

        public Organization(string name)
        {
            Name = name;
        }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public List<Location> OfficeLocations { get; } = new List<Location>();
    }
}
