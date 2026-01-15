using Booking.Domain.DomainModels;
using Booking.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service.Interface
{
    public interface IAccommodationService
    {
        List<Accommodation> GetAll();
        List<AccommodationHost> GetAllHosts();

        Accommodation? GetById(Guid id);
        Accommodation Insert(Accommodation accommodation);
        Accommodation Update(Accommodation accommodation);
        Accommodation DeleteById(Guid id);

        AddToReservationCartDTO GetSelectedAccommodation(Guid id);
        void AddAccommodationToReservationCart(Guid accommodationId, Guid userId, int nights);
    }

}
