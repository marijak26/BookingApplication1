using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.DomainModels
{
    public class AccommodationHost : BaseEntity
    {
        public string? FullName { get; set; }
        public string? ContactEmail { get; set; }

        public Guid CountryId { get; set; }
        public Country? Country { get; set; }

        public virtual ICollection<Accommodation>? Accommodations { get; set; }
    }
}
