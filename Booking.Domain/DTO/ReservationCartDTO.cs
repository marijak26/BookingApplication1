using Booking.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.DTO
{
    public class ReservationCartDTO
    {
        public List<AccommodationInReservationCart>? Accommodations { get; set; }
        public double TotalPrice { get; set; }
    }
}
