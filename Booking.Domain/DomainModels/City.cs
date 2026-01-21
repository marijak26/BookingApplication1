using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.DomainModels
{
    public class City : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public Guid CountryId { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<AccommodationHost>? Hosts { get; set; }

    }
}
