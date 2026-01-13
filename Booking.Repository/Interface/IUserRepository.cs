using Booking.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Repository.Interface
{
    public interface IUserRepository
    {
        BookingApplicationUser GetUserById(string id);
    }
}
