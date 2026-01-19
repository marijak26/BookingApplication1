using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.DTO
{
    public class ReservationResultDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Guid? ReservationId { get; set; }
    }
}
