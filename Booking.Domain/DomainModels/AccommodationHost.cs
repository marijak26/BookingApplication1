using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.DomainModels
{
    public class AccommodationHost : BaseEntity
    {
        [Display(Name = "Full Name")]
        public string? FullName { get; set; }

        [Display(Name = "Contact Email")]
        public string? ContactEmail { get; set; }

        public Guid? CountryId { get; set; }

        [Display(Name = "Country")]
        public Country? Country { get; set; }

        public Guid? CityId { get; set; }

        [Display(Name = "City")]
        public City? City { get; set; }

        public virtual ICollection<Accommodation>? Accommodations { get; set; }
    }
}
