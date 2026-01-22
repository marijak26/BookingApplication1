using Booking.Domain.DomainModels;
using Booking.Domain.Email;
using Booking.Domain.Enum;
using Booking.Repository.Interface;
using Booking.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Booking.Service.Implementation
{
    public class ReservationService : IReservationService
    {
        private readonly IRepository<Reservation> _reservationRepository;
        private readonly IEmailService _emailService;

        public ReservationService(IRepository<Reservation> reservationRepository, IEmailService emailService)
        {
            _reservationRepository = reservationRepository;
            _emailService = emailService;
        }

        public List<Reservation> GetAllReservations()
        {
            return _reservationRepository.GetAll(
                selector: x => x,
                include: x => x.Include(r => r.AccommodationInReservations)
                               .ThenInclude(a => a.Accommodation)
                               .Include(r => r.User))
                .ToList();
        }

        public List<Reservation> GetAllForUser(Guid userId)
        {
            return _reservationRepository
                .GetAll(
                    selector: x => x,
                    predicate: x => x.UserId == userId.ToString(),
                    include: x => x
                        .Include(r => r.User)
                        .Include(r => r.AccommodationInReservations)
                            .ThenInclude(a => a.Accommodation))
                .ToList();
        }

        public Reservation GetReservation(Guid id)
        {
            var reservation = _reservationRepository.Get(
                selector: x => x,
                predicate: x => x.Id.Equals(id),
                include: x => x.Include(r => r.AccommodationInReservations)
                               .ThenInclude(a => a.Accommodation)
                               .Include(r => r.User));

            if (reservation == null)
            {
                throw new Exception("Reservation not found");
            }

            return reservation;
        }

        public void CancelReservation(Guid reservationId)
        {
            var reservation = _reservationRepository.Get(
                selector: x => x,
                predicate: x => x.Id == reservationId,
                include: q => q.Include(r => r.User)
                               .Include(r => r.AccommodationInReservations)
                                .ThenInclude(ar => ar.Accommodation));


            if (reservation == null)
            {
                throw new Exception("Reservation not found");
            }

            if (reservation.Status != ReservationStatus.Confirmed)
            {
                throw new Exception("Only confirmed reservations can be cancelled");
            }

            reservation.Status = ReservationStatus.Cancelled;

            _reservationRepository.Update(reservation);

            var user = reservation.User;
            if (!string.IsNullOrEmpty(user.Email))
            {
                var accommodationNames = reservation.AccommodationInReservations.Select(ar => ar.Accommodation.Name).ToList();

                var accommodationsText = string.Join(", ", accommodationNames);

                var message = new EmailMessage
                {
                    MailTo = user.Email,
                    Subject = "Reservation Cancelled",
                    Content = $"Your reservation for {accommodationsText} has been cancelled.\n" +
                              $"Details:\n" +
                              $"Total price: €{reservation.TotalPrice:F2}"
                };

                _emailService.SendEmailAsync(message);
            }
        }
    }
}
