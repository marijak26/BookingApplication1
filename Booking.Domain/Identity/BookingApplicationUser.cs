using Booking.Domain.DomainModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Identity
{
    public class BookingApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public ReservationCart? ReservationCart { get; set; }
        public virtual ICollection<Accommodation>? Accommodations { get; set; }
    }
}
