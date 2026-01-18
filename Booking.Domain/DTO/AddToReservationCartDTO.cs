using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.DTO
{
    public class AddToReservationCartDTO
    {
        public Guid SelectedAccommodationId { get; set; }
        public string? SelectedAccommodationName { get; set; }

        [Required]
        [Display(Name = "From")]
        public DateTime FromDate { get; set; }

        [Required]
        [Display(Name = "To")]
        public DateTime ToDate { get; set; }
    }
}
