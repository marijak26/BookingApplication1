using Booking.Domain.DomainModels;
using Booking.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service.Interface
{
    public interface IReservationCartService
    {
        ReservationCart GetOrCreateCartForUser(Guid userId);
        ReservationCartDTO GetByUserIdWithIncludedAccommodations(Guid userId);
        void DeleteAccommodationFromReservationCart(Guid accommodationInCartId);
        ReservationResultDTO ConfirmReservation(Guid cartItemId, Guid userId);
        ReservationResultDTO ConfirmWholeCart(Guid userId);
        void ClearCart(Guid userId);

    }

}
