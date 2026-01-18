using Booking.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.DomainModels
{
    public class Accommodation : BaseEntity
    {
        [Display(Name = "Name")]
        public string? Name { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Price Per Night")]
        public double PricePerNight { get; set; }

        [Display(Name = "Is Rented")]
        public bool IsRented { get; set; }

        [Display(Name = "Category")]
        public AccommodationCategory Category { get; set; }

        public Guid HostId { get; set; }

        [Display(Name = "Host")]
        public AccommodationHost? Host { get; set; }

        public string? ImageUrl { get; set; }


        public virtual ICollection<AccommodationInReservation>? AccommodationInReservations { get; set; }
        public virtual ICollection<AccommodationInReservationCart>? AccommodationInReservationCarts { get; set; }

    }
}
