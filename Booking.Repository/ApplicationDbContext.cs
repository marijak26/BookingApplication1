using Booking.Domain.DomainModels;
using Booking.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Booking.Repository
{
    public class ApplicationDbContext : IdentityDbContext<BookingApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Accommodation> Accommodations { get; set; }
        public virtual DbSet<AccommodationHost> Hosts { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<AccommodationInReservation> AccommodationInReservations { get; set; }
        }
}
