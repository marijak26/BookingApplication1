using Booking.Domain.DomainModels;
using Booking.Domain.DTO;
using Booking.Repository.Interface;
using Booking.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Booking.Service.Implementation
{
    public class AccommodationService : IAccommodationService
    {
        private readonly IRepository<Accommodation> _accommodationRepository;
        private readonly IRepository<AccommodationInReservationCart> _accommodationInCartRepository;
        private readonly IReservationCartService _reservationCartService;
        private readonly IAccommodationHostService _accommodationHostService;
        private readonly IRepository<AccommodationInReservation> _accommodationInReservationRepository;


        public AccommodationService(
            IRepository<Accommodation> accommodationRepository,
            IRepository<AccommodationInReservationCart> accommodationInCartRepository,
            IReservationCartService reservationCartService,
            IAccommodationHostService accommodationHostService,
            IRepository<AccommodationInReservation> accommodationInReservationRepository)
        {
            _accommodationRepository = accommodationRepository;
            _accommodationInCartRepository = accommodationInCartRepository;
            _reservationCartService = reservationCartService;
            _accommodationHostService = accommodationHostService;
            _accommodationInReservationRepository = accommodationInReservationRepository;
        }

        public List<Accommodation> GetAll()
        {
            return _accommodationRepository.GetAll(
                selector: x => x,
                include: x => x.Include(a => a.Host)
                       .ThenInclude(h => h.Country))
                .ToList();
        }

        public List<AccommodationHost> GetAllHosts()
        {
            return _accommodationHostService.GetAll();
        }

        public Accommodation? GetById(Guid id)
        {
            return _accommodationRepository.Get(
                selector: x => x,
                include: x => x.Include(a => a.Host)
                               .ThenInclude(h => h.Country),
                predicate: x => x.Id.Equals(id));
        }

        public Accommodation Insert(Accommodation accommodation)
        {
            accommodation.Id = Guid.NewGuid();
            return _accommodationRepository.Insert(accommodation);
        }

        public Accommodation Update(Accommodation accommodation)
        {
            return _accommodationRepository.Update(accommodation);
        }

        public Accommodation DeleteById(Guid id)
        {
            var accommodation = GetById(id);

            if (accommodation == null)
            {
            throw new Exception("Accommodation not found");
            }

            _accommodationRepository.Delete(accommodation);
            return accommodation;
        }

        public AddToReservationCartDTO GetSelectedAccommodation(Guid id)
        {
            var accommodation = GetById(id);

            if (accommodation == null)
            {
                throw new Exception("Accommodation not found");
            }

            var fromDate = DateTime.Today.AddDays(1);
            var toDate = fromDate.AddDays(1);

            return new AddToReservationCartDTO
            {
                SelectedAccommodationId = accommodation.Id,
                SelectedAccommodationName = accommodation.Name,
                FromDate = fromDate,
                ToDate = toDate
            };
        }

        public ReservationResultDTO AddAccommodationToReservationCart(Guid accommodationId, Guid userId, DateTime fromDate, DateTime toDate)
        {
            if (fromDate >= toDate)
            {
                return new ReservationResultDTO { Success = false, Message = "Invalid dates" };
            }

            if (!IsAccommodationAvailable(accommodationId, fromDate, toDate))
            {
                return new ReservationResultDTO { Success = false, Message = "Accommodation is not available for the selected dates" };
            }

            var cart = _reservationCartService.GetOrCreateCartForUser(userId);

            var newItem = new AccommodationInReservationCart
            {
                Id = Guid.NewGuid(),
                AccommodationId = accommodationId,
                ReservationCartId = cart.Id,
                FromDate = fromDate,
                ToDate = toDate
            };

            _accommodationInCartRepository.Insert(newItem);

            return new ReservationResultDTO { Success = true, Message = "Accommodation added to cart" };
        }

        public List<Accommodation> GetByCountry(Guid countryId)
        {
            return _accommodationRepository.GetAll(
                selector: x => x,
                include: x => x.Include(a => a.Host)
                               .ThenInclude(h => h.Country),
                predicate: x => x.Host.CountryId == countryId)
                .ToList();
        }

        public bool IsAccommodationAvailable(Guid accommodationId, DateTime from, DateTime to)
        {
            if (from >= to)
            {
                return false;
            }

            var hasReservationConflict = _accommodationInReservationRepository
                .GetAll(
                    selector: x => x,
                    predicate: x =>
                        x.AccommodationId == accommodationId &&
                        x.FromDate < to &&
                        x.ToDate > from)
                .Any();

            if (hasReservationConflict)
            {
                return false;
            }

            var hasCartConflict = _accommodationInCartRepository
                .GetAll(
                    selector: x => x,
                    predicate: x =>
                        x.AccommodationId == accommodationId &&
                        x.FromDate < to &&
                        x.ToDate > from)
                .Any();

            return !hasCartConflict;
        }

        public List<CalendarEventDTO> GetAccommodationCalendar(Guid accommodationId)
        {
            var reservations = _accommodationInReservationRepository
                .GetAll(
                    selector: x => x,
                    predicate: x => x.AccommodationId == accommodationId)
                .ToList();

            return reservations.Select(r => new CalendarEventDTO
            {
                title = "Reserved",
                start = r.FromDate.ToString("yyyy-MM-dd"),
                end = r.ToDate.ToString("yyyy-MM-dd")})
                .ToList();
        }
    }
}
