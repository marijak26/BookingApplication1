using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.DomainModels
{
    public class AccommodationInReservationCart : BaseEntity
    {
        public Guid AccommodationId { get; set; }
        public Accommodation? Accommodation { get; set; }

        public Guid ReservationCartId { get; set; }
        public ReservationCart? ReservationCart { get; set; }

        public int Nights { get; set; }
    }
}
