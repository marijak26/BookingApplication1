using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service.Interface
{
    public interface IAvailabilityService
    {
        bool IsAccommodationAvailable(Guid accommodationId, DateTime from, DateTime to);
    }
}
