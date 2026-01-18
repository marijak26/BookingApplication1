namespace AdminApplication.Models
{
    public class Country : BaseEntity
    {
        public string? Name { get; set; }
        public virtual ICollection<AccommodationHost>? Hosts { get; set; }
    }

}
