using Booking.Domain.DomainModels;
using Booking.Domain.DTO;
using Booking.Repository.Interface;
using Booking.Service.Interface;
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
        private readonly IRepository<AccommodationHost> _accommodationHostRepository;

        public AccommodationService(
            IRepository<Accommodation> accommodationRepository,
            IRepository<AccommodationInReservationCart> accommodationInCartRepository,
            IReservationCartService reservationCartService,
            IRepository<AccommodationHost> accommodationHostRepository)
        {
            _accommodationRepository = accommodationRepository;
            _accommodationInCartRepository = accommodationInCartRepository;
            _reservationCartService = reservationCartService;
            _accommodationHostRepository = accommodationHostRepository;
        }

        public List<Accommodation> GetAll()
        {
            return _accommodationRepository.GetAll(selector: x => x).ToList();
        }

        public List<AccommodationHost> GetAllHosts()
        {
            return _accommodationHostRepository.GetAll(x => x).ToList();
        }

        public Accommodation? GetById(Guid id)
        {
            return _accommodationRepository.Get(selector: x => x,
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
                throw new Exception("Accommodation not found");

            _accommodationRepository.Delete(accommodation);
            return accommodation;
        }

        public AddToReservationCartDTO GetSelectedAccommodation(Guid id)
        {
            var accommodation = GetById(id);
            if (accommodation == null)
                throw new Exception("Accommodation not found");

            return new AddToReservationCartDTO
            {
                SelectedAccommodationId = accommodation.Id,
                SelectedAccommodationName = accommodation.Name,
                Nights = 1
            };
        }

        public void AddAccommodationToReservationCart(Guid accommodationId, Guid userId, int nights)
        {
            var cart = _reservationCartService.GetOrCreateCartForUser(userId);

            var accommodation = GetById(accommodationId);
            if (accommodation == null)
                throw new Exception("Accommodation not found");

            var existing = _accommodationInCartRepository.Get(
                selector: x => x,
                predicate: x => x.ReservationCartId == cart.Id && x.AccommodationId == accommodationId
            );

            if (existing == null)
            {
                var newItem = new AccommodationInReservationCart
                {
                    Id = Guid.NewGuid(),
                    AccommodationId = accommodation.Id,
                    ReservationCartId = cart.Id, 
                    Nights = nights,
                    Accommodation = accommodation
                };
                _accommodationInCartRepository.Insert(newItem);
            }
            else
            {
                existing.Nights += nights;
                _accommodationInCartRepository.Update(existing);
            }
        }

    }
}
