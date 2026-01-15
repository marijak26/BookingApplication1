using Booking.Domain.DomainModels;
using Booking.Domain.DTO;
using Booking.Domain.Enum;
using Booking.Repository.Interface;
using Booking.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Booking.Service.Implementation
{
    public class ReservationCartService : IReservationCartService
    {
        private readonly IRepository<ReservationCart> _reservationCartRepository;
        private readonly IRepository<AccommodationInReservationCart> _accommodationInCartRepository;
        private readonly IRepository<Reservation> _reservationRepository;
        private readonly IRepository<AccommodationInReservation> _accommodationInReservationRepository;
        private readonly IUserRepository _userRepository;

        public ReservationCartService(
            IRepository<ReservationCart> reservationCartRepository,
            IRepository<AccommodationInReservationCart> accommodationInCartRepository,
            IRepository<Reservation> reservationRepository,
            IRepository<AccommodationInReservation> accommodationInReservationRepository,
            IUserRepository userRepository)
        {
            _reservationCartRepository = reservationCartRepository;
            _accommodationInCartRepository = accommodationInCartRepository;
            _reservationRepository = reservationRepository;
            _accommodationInReservationRepository = accommodationInReservationRepository;
            _userRepository = userRepository;
        }

        public ReservationCart GetOrCreateCartForUser(Guid userId)
        {
            var cart = _reservationCartRepository.Get(
                selector: x => x,
                predicate: x => x.UserId == userId.ToString(),
                include: x => x.Include(z => z.Accommodations)
                               .ThenInclude(z => z.Accommodation)
            );

            if (cart == null)
            {
                cart = new ReservationCart
                {
                    Id = Guid.NewGuid(),
                    UserId = userId.ToString(),
                    Accommodations = new List<AccommodationInReservationCart>()
                };
                _reservationCartRepository.Insert(cart);
            }

            return cart;
        }

        public ReservationCartDTO GetByUserIdWithIncludedAccommodations(Guid userId)
        {
            var cart = _reservationCartRepository.Get(
                selector: x => x,
                predicate: x => x.UserId == userId.ToString(),
                include: x => x.Include(z => z.Accommodations)
                               .ThenInclude(z => z.Accommodation)
            );

            if (cart == null)
                return new ReservationCartDTO { Accommodations = new List<AccommodationInReservationCart>(), TotalPrice = 0 };

            var items = cart.Accommodations?.ToList() ?? new List<AccommodationInReservationCart>();
            double total = items.Sum(x => x.Nights * x.Accommodation.PricePerNight);

            return new ReservationCartDTO
            {
                Accommodations = items,
                TotalPrice = total
            };
        }

        public void DeleteAccommodationFromReservationCart(Guid accommodationInCartId)
        {
            var item = _accommodationInCartRepository.Get(selector: x => x,
                predicate: x => x.Id.Equals(accommodationInCartId));

            if (item == null)
                throw new Exception("Item not found");

            _accommodationInCartRepository.Delete(item);
        }

        public bool ConfirmReservation(Guid userId)
        {
            var cart = _reservationCartRepository.Get(
                selector: x => x,
                predicate: x => x.UserId.Equals(userId.ToString()),
                include: x => x.Include(z => z.Accommodations)
                               .ThenInclude(z => z.Accommodation)
            );

            if (cart == null || cart.Accommodations == null || !cart.Accommodations.Any())
                throw new Exception("Cart is empty or does not exist.");

            var user = _userRepository.GetUserById(userId.ToString());
            if (user == null)
                throw new Exception("User not found");

            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                User = user,
                Status = ReservationStatus.Confirmed
            };

            _reservationRepository.Insert(reservation);

            foreach (var item in cart.Accommodations)
            {
                var reservationItem = new AccommodationInReservation
                {
                    Id = Guid.NewGuid(),
                    ReservationId = reservation.Id,
                    Reservation = reservation,
                    AccommodationId = item.AccommodationId,
                    Accommodation = item.Accommodation,
                    Nights = item.Nights
                };

                _accommodationInReservationRepository.Insert(reservationItem);
            }

            reservation.TotalPrice = cart.Accommodations.Sum(x => x.Nights * x.Accommodation.PricePerNight);
            _reservationRepository.Update(reservation);

            cart.Accommodations.Clear();
            _reservationCartRepository.Update(cart);

            return true;
        }

        public bool CancelReservation(Guid userId)
        {
            var reservation = _reservationRepository.Get(
                selector: x => x,
                predicate: x => x.UserId == userId.ToString()
                             && x.Status == ReservationStatus.Confirmed
            );

            if (reservation == null)
                return false;

            reservation.Status = ReservationStatus.Cancelled;
            _reservationRepository.Update(reservation);

            return true;
        }



    }
}
