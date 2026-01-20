using System.ComponentModel.DataAnnotations.Schema;

namespace AdminApplication.Models
{
    public class AccommodationInReservation : BaseEntity
    {
        public Guid AccommodationId { get; set; }
        public Accommodation? Accommodation { get; set; }
        public Guid ReservationId { get; set; }
        public Reservation? Reservation { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        [NotMapped]
        public int Nights => (ToDate - FromDate).Days;
    }
}
