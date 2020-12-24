using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTracker.Core.Domain
{
    public class Offer : AuditableEntity
    {
        protected Offer() { }

        public Offer(JobApplication application)
        {
            this.Application = application;
            this.ApplicationId = application.Id;
        }

        public int Id { get; private set; }

        public int ApplicationId { get; private set; }

        public virtual JobApplication Application { get; private set; }

        public DateTime Extended { get; set; }

        public DateTime Expires { get; set; }

        public bool IsAccepted { get; set; }

        public DateTime? Responded { get; set; }

        public OfferStatus Status { get; set; } = OfferStatus.PENDING;
        
        //public decimal Salary { get; set; }

        public string Details { get; set; }
    }

    public enum OfferStatus
    {
        PENDING,
        ACCEPTED,
        DECLINED
    };
}
