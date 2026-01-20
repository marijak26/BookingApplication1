using System.Diagnostics.Metrics;

namespace AdminApplication.Models
{
    public class AccommodationHost : BaseEntity
    {
        public string? FullName { get; set; }
        public string? ContactEmail { get; set; }
        public Guid CountryId { get; set; }
        public Country? Country { get; set; }
        public virtual ICollection<Accommodation>? Accommodations { get; set; }
    }
}
