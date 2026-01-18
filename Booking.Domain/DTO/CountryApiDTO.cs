using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.DTO
{
    public class CountryApiDTO
    {
        public NameDto name { get; set; }

        public class NameDto
        {
            public string common { get; set; }
        }
    }
}
