using Booking.Domain.DomainModels;
using Booking.Repository.Interface;
using Booking.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service.Implementation
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly IRepository<AccommodationInReservation> _reservationRepo;

        public AvailabilityService(
            IRepository<AccommodationInReservation> reservationRepo)
        {
            _reservationRepo = reservationRepo;
        }

        public bool IsAccommodationAvailable(Guid accommodationId, DateTime from, DateTime to)
        {
            return !_reservationRepo.GetAll(x => x)
                .Any(r =>
                    r.AccommodationId == accommodationId &&
                    from < r.ToDate &&
                    to > r.FromDate
                );
        }
    }

}
