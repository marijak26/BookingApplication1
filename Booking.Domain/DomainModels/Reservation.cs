using Booking.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.DomainModels
{
    public class Reservation : BaseEntity
    {
        public string? UserId { get; set; }
        public BookingApplicationUser? User { get; set; }
        public double TotalPrice { get; set; }
        public virtual ICollection<AccommodationInReservation>? AccommodationInReservations { get; set; }


    }
}
