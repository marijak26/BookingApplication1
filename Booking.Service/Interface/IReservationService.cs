using Booking.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service.Interface
{
    public interface IReservationService
    {
        List<Reservation> GetAllReservations();
        List<Reservation> GetAllForUser(Guid userId);
        Reservation GetReservation(Guid id);
        void CancelReservation(Guid reservationId);
    }
}
