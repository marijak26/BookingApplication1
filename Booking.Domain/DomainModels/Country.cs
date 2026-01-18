using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.DomainModels
{
    public class Country : BaseEntity
    {
        [Display(Name = "Name")]
        public string? Name { get; set; }

        public virtual ICollection<AccommodationHost>? Hosts { get; set; }
    }
}
