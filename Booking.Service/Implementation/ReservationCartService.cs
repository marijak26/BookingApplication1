using Booking.Domain.DomainModels;
using Booking.Domain.DTO;
using Booking.Domain.Email;
using Booking.Domain.Enum;
using Booking.Repository.Interface;
using Booking.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Booking.Service.Implementation
{
    public class ReservationCartService : IReservationCartService
    {
        private readonly IRepository<ReservationCart> _reservationCartRepository;
        private readonly IRepository<AccommodationInReservationCart> _accommodationInReservationCartRepository;
        private readonly IRepository<Reservation> _reservationRepository;
        private readonly IRepository<AccommodationInReservation> _accommodationInReservationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAvailabilityService _availabilityService;
        private readonly IEmailService _emailService;

        public ReservationCartService(
            IRepository<ReservationCart> reservationCartRepository,
            IRepository<AccommodationInReservationCart> accommodationInReservationCartRepository,
            IRepository<Reservation> reservationRepository,
            IRepository<AccommodationInReservation> accommodationInReservationRepository,
            IUserRepository userRepository,
            IAvailabilityService availabilityService,
            IEmailService emailService)
        {
            _reservationCartRepository = reservationCartRepository;
            _accommodationInReservationCartRepository = accommodationInReservationCartRepository;
            _reservationRepository = reservationRepository;
            _accommodationInReservationRepository = accommodationInReservationRepository;
            _userRepository = userRepository;
            _availabilityService = availabilityService;
            _emailService = emailService;
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
            {
                return new ReservationCartDTO 
                { 
                    Accommodations = new List<AccommodationInReservationCart>(), 
                    TotalPrice = 0 
                };
            }

            var accommodations = cart.Accommodations?.ToList() ?? new List<AccommodationInReservationCart>();
            double totalPrice = accommodations.Sum(x => x.Nights * x.Accommodation.PricePerNight);

            ReservationCartDTO model = new ReservationCartDTO
            {
                Accommodations = accommodations,
                TotalPrice = totalPrice
            };

            return model;
        }

        public void DeleteAccommodationFromReservationCart(Guid accommodationInCartId)
        {
            var item = _accommodationInReservationCartRepository.Get(
                selector: x => x,
                predicate: x => x.Id.Equals(accommodationInCartId));

            if (item == null)
            {
                throw new Exception("Item not found");
            }

            _accommodationInReservationCartRepository.Delete(item);
        }

        public ReservationResultDTO ConfirmReservation(Guid cartItemId, Guid userId)
        {
            var cartItem = _accommodationInReservationCartRepository.Get(
                selector: x => x,
                predicate: x => x.Id == cartItemId,
                include: x => x.Include(a => a.Accommodation)
                                .Include(a => a.ReservationCart)
                                .ThenInclude(c => c.User)
            );

            if (cartItem == null)
            {
                return new ReservationResultDTO 
                { 
                    Success = false, 
                    Message = "Item not found" 
                };
            }

            if (!_availabilityService.IsAccommodationAvailable(
                    cartItem.AccommodationId,
                    cartItem.FromDate,
                    cartItem.ToDate))
            {
                return new ReservationResultDTO
                {
                    Success = false,
                    Message = $"Accommodation {cartItem.Accommodation.Name} is no longer available for the selected dates"
                };
            }

            int nights = (cartItem.ToDate - cartItem.FromDate).Days;
            double totalPrice = nights * cartItem.Accommodation.PricePerNight;

            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                UserId = userId.ToString(),
                Status = ReservationStatus.Confirmed,
                TotalPrice = totalPrice
            };

            _reservationRepository.Insert(reservation);

            _accommodationInReservationRepository.Insert(
                new AccommodationInReservation
                {
                    Id = Guid.NewGuid(),
                    ReservationId = reservation.Id,
                    AccommodationId = cartItem.AccommodationId,
                    FromDate = cartItem.FromDate,
                    ToDate = cartItem.ToDate
                });

            _accommodationInReservationCartRepository.Delete(cartItem);

            var user = cartItem.ReservationCart.User;
            if (!string.IsNullOrEmpty(user.Email))
            {
                var message = new EmailMessage
                {
                    MailTo = user.Email,
                    Subject = "Reservation Confirmed",
                    Content = $"Your reservation for {cartItem.Accommodation.Name} is confirmed.\n" +
                              $"Check-In Date: {cartItem.FromDate:yyyy-MM-dd}\n" +
                              $"Check-Out Date: {cartItem.ToDate:yyyy-MM-dd}\n" +
                              $"Total price: €{totalPrice:F2}"
                };

                _emailService.SendEmailAsync(message);
            }

            return new ReservationResultDTO
            {
                Success = true,
                Message = "Reservation confirmed",
                ReservationId = reservation.Id
            };
        }

        public ReservationResultDTO ConfirmWholeCart(Guid userId)
        {
            var cart = _reservationCartRepository.Get(
                selector: x => x,
                predicate: x => x.UserId == userId.ToString(),
                include: x => x.Include(c => c.Accommodations)
                               .ThenInclude(a => a.Accommodation)
                               .Include(c => c.User));

            if (cart == null || !cart.Accommodations.Any())
                return new ReservationResultDTO 
                { 
                    Success = false, 
                    Message = "Cart is empty" 
                };

            var unavailableItems = cart.Accommodations
                .Where(i => !_availabilityService.IsAccommodationAvailable(i.AccommodationId, i.FromDate, i.ToDate))
                .ToList();

            if (unavailableItems.Any())
            {
                string names = string.Join(", ", unavailableItems.Select(i => i.Accommodation.Name));
                return new ReservationResultDTO
                {
                    Success = false,
                    Message = $"These accommodations are no longer available: {names}"
                };
            }

            double totalPrice = 0;
            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                UserId = userId.ToString(),
                Status = ReservationStatus.Confirmed,
                AccommodationInReservations = new List<AccommodationInReservation>()
            };

            StringBuilder emailContent = new StringBuilder();
            emailContent.AppendLine("Your whole reservation cart is confirmed.\nDetails:");

            foreach (var item in cart.Accommodations)
            {
                int nights = (item.ToDate - item.FromDate).Days;
                double price = nights * item.Accommodation.PricePerNight;
                totalPrice += price;

                reservation.AccommodationInReservations.Add(new AccommodationInReservation
                {
                    Id = Guid.NewGuid(),
                    AccommodationId = item.AccommodationId,
                    FromDate = item.FromDate,
                    ToDate = item.ToDate
                });

                emailContent.AppendLine($"{item.Accommodation.Name}: {nights} nights, €{price:F2}");
            }

            reservation.TotalPrice = totalPrice;
            _reservationRepository.Insert(reservation);

            cart.Accommodations.Clear();
            _reservationCartRepository.Update(cart);

            if (!string.IsNullOrEmpty(cart.User.Email))
            {
                EmailMessage message = new EmailMessage
                {
                    MailTo = cart.User.Email,
                    Subject = "Reservation Confirmed",
                    Content = emailContent.AppendLine($"Total: ${totalPrice:F2}").ToString()
                };

                _emailService.SendEmailAsync(message);
            }

            return new ReservationResultDTO
            {
                Success = true,
                Message = "All accommodations successfully reserved",
                ReservationId = reservation.Id
            };
        }

        public void ClearCart(Guid userId)
        {
            var cart = _reservationCartRepository.Get(
                selector: x => x,
                predicate: x => x.UserId == userId.ToString(),
                include: x => x.Include(z => z.Accommodations)
            );

            if (cart == null)
            {
                return;
            }

            cart.Accommodations.Clear();
            _reservationCartRepository.Update(cart);
        }
    }
}
