using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.DomainModels
{
    public class AccommodationInReservation : BaseEntity
    {
        public Guid AccommodationId { get; set; }
        public Accommodation? Accommodation { get; set; }
        public Guid ReservationId { get; set; }
        public Reservation? Reservation { get; set; }

        public int Nights { get; set; }
    }
}
