using Booking.Domain.Identity;
using Booking.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly DbSet<BookingApplicationUser> entites;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
            this.entites = _context.Set<BookingApplicationUser>();
        }

        public BookingApplicationUser GetUserById(string id)
        {
            return entites.First(ent => ent.Id == id);
        }
    }
}
