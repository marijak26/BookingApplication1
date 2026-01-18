using Booking.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.DomainModels
{
    public class ReservationCart : BaseEntity
    {
        public string? UserId { get; set; }

        [Display(Name = "User")]
        public BookingApplicationUser? User { get; set; }

        public virtual ICollection<AccommodationInReservationCart>? Accommodations { get; set; }
    }
}
