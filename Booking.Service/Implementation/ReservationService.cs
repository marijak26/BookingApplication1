using Booking.Domain.DomainModels;
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

        public ReservationService(IRepository<Reservation> reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public List<Reservation> GetAllReservations()
        {
            return _reservationRepository.GetAll(
                selector: x => x,
                include: x => x.Include(r => r.AccommodationInReservations)
                               .ThenInclude(a => a.Accommodation)
                               .Include(r => r.User)
            ).ToList();
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
                            .ThenInclude(a => a.Accommodation)
                )
                .ToList();
        }

        public Reservation GetReservation(Guid id)
        {
            var reservation = _reservationRepository.Get(
                selector: x => x,
                predicate: x => x.Id.Equals(id),
                include: x => x.Include(r => r.AccommodationInReservations)
                               .ThenInclude(a => a.Accommodation)
                               .Include(r => r.User)
            );

            if (reservation == null)
                throw new Exception("Reservation not found");

            return reservation;
        }

        public void CancelReservation(Guid reservationId)
        {
            var reservation = _reservationRepository.Get(
                selector: x => x,
                predicate: x => x.Id == reservationId
            );

            if (reservation == null)
                throw new Exception("Reservation not found");

            if (reservation.Status != ReservationStatus.Confirmed)
                throw new Exception("Only confirmed reservations can be cancelled");

            reservation.Status = ReservationStatus.Cancelled;

            _reservationRepository.Update(reservation);
        }


    }
}
