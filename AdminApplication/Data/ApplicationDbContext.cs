using AdminApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminApplication.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Accommodation> Accommodations { get; set; }
        public DbSet<AccommodationHost> Hosts { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<AccommodationInReservation> AccommodationInReservations { get; set; }
    }
}
