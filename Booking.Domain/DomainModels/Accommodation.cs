using Booking.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.DomainModels
{
    public class Accommodation : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double PricePerNight { get; set; }
        public bool IsRented { get; set; }

        public AccommodationCategory Category { get; set; }

        public Guid HostId { get; set; }
        public AccommodationHost? Host { get; set; }
        public virtual ICollection<AccommodationInReservation>? AccommodationInReservations { get; set; }
    }
}
