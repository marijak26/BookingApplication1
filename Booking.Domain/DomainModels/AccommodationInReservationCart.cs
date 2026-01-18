using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        [NotMapped]
        public int Nights => (ToDate - FromDate).Days;
    }
}
