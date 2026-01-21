using Booking.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service.Interface
{
    public interface IAccommodationHostService
    {
        List<AccommodationHost> GetAll();
        AccommodationHost GetById(Guid id);
        void Insert(AccommodationHost host);
        void Update(AccommodationHost host);
        void DeleteById(Guid id);
    }
}
