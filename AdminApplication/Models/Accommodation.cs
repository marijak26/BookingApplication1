namespace AdminApplication.Models
{
    public class Accommodation : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double PricePerNight { get; set; }
        public bool IsRented { get; set; }
        public AccommodationCategory Category { get; set; }

        public Guid HostId { get; set; }
        public AccommodationHost? Host { get; set; }

        public string? ImageUrl { get; set; }

        public virtual ICollection<AccommodationInReservation>? AccommodationInReservations { get; set; }
    }
}
