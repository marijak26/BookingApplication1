using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.DTO
{
    public class AddToReservationCartDTO
    {
        public Guid SelectedAccommodationId { get; set; }
        public string? SelectedAccommodationName { get; set; }
        public int Nights { get; set; }
    }
}
