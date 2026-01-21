using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.DTO
{
    public class CountryApiDTO
    {
        public List<CountryData> data { get; set; } = new List<CountryData>();

        public class CountryData
        {
            public string country { get; set; } = string.Empty;
            public List<string> cities { get; set; } = new List<string>();
        }
    }
}
