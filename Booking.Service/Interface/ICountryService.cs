using Booking.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service.Interface
{
    public interface ICountryService
    {
        Task<List<Country>> GetCountriesFromApi();
        List<Country> GetAllCountriesFromDb();
        void SeedCountries(List<Country> countries);
        Country? GetById(Guid id);

    }
}
