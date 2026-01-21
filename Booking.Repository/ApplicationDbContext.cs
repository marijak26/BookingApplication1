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

        public DbSet<Accommodation> Accommodations { get; set; }
        public DbSet<AccommodationHost> Hosts { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationCart> ReservationCarts { get; set; }
        public DbSet<AccommodationInReservation> AccommodationInReservations { get; set; }
        public DbSet<AccommodationInReservationCart> AccommodationInReservationCarts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<City>()
                .HasOne(c => c.Country)
                .WithMany(c => c.Cities)
                .HasForeignKey(c => c.CountryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AccommodationHost>()
               .HasOne(h => h.City)
               .WithMany(c => c.Hosts)
               .HasForeignKey(h => h.CityId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AccommodationHost>()
                .HasOne(h => h.Country)
                .WithMany(c => c.Hosts)
                .HasForeignKey(h => h.CountryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Accommodation>()
                .HasOne(a => a.Host)
                .WithMany(h => h.Accommodations)
                .HasForeignKey(a => a.HostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ReservationCart>()
                .HasOne(rc => rc.User)
                .WithOne()
                .HasForeignKey<ReservationCart>(rc => rc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AccommodationInReservation>()
                .HasOne(ar => ar.Accommodation)
                .WithMany(a => a.AccommodationInReservations)
                .HasForeignKey(ar => ar.AccommodationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AccommodationInReservation>()
                .HasOne(ar => ar.Reservation)
                .WithMany(r => r.AccommodationInReservations)
                .HasForeignKey(ar => ar.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AccommodationInReservationCart>()
                .HasKey(x => x.Id);

            builder.Entity<AccommodationInReservationCart>()
                .HasOne(ac => ac.Accommodation)
                .WithMany(a => a.AccommodationInReservationCarts)
                .HasForeignKey(ac => ac.AccommodationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AccommodationInReservationCart>()
                .HasOne(ac => ac.ReservationCart)
                .WithMany(rc => rc.Accommodations)
                .HasForeignKey(ac => ac.ReservationCartId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
