using Booking.Domain.Enum;
using Booking.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.DomainModels
{
    public class Reservation : BaseEntity
    {
        public string? UserId { get; set; }

        [Display(Name = "User's Full Name")]
        public BookingApplicationUser? User { get; set; }

        [Display(Name = "Total Price")]
        public double TotalPrice { get; set; }

        [Display(Name = "Reservation Status")]
        public ReservationStatus Status { get; set; } = ReservationStatus.Confirmed;

        public virtual ICollection<AccommodationInReservation>? AccommodationInReservations { get; set; }


    }
}
