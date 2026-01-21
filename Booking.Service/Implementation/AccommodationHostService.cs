using Booking.Domain.DomainModels;
using Booking.Repository.Interface;
using Booking.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service.Implementation
{
    public class AccommodationHostService : IAccommodationHostService
    {
        private readonly IRepository<AccommodationHost> _hostRepository;

        public AccommodationHostService(IRepository<AccommodationHost> hostRepository)
        {
            _hostRepository = hostRepository;
        }

        public List<AccommodationHost> GetAll()
        {
            return _hostRepository.GetAll(
                selector: x => x,
                include: x => x.Include(h => h.Country))
                .ToList();
        }

        public AccommodationHost GetById(Guid id)
        {
            return _hostRepository.Get(
                selector: x => x,
                predicate: x => x.Id == id,
                include: x => x.Include(h => h.Country));
        }

        public void Insert(AccommodationHost host)
        {
            host.Id = Guid.NewGuid();
            _hostRepository.Insert(host);
        }

        public void Update(AccommodationHost host)
        {
            _hostRepository.Update(host);
        }

        public void DeleteById(Guid id)
        {
            var host = GetById(id);
            if (host != null)
            {
                _hostRepository.Delete(host);
            }
        }
    }
}
