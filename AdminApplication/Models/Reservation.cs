namespace AdminApplication.Models
{
    public class Reservation : BaseEntity
    {
        public string? UserId { get; set; }
        public BookingApplicationUserDTO? User { get; set; }
        public double TotalPrice { get; set; }
        public ReservationStatus Status { get; set; }
        public virtual ICollection<AccommodationInReservation>? AccommodationInReservations { get; set; }
    }
}
