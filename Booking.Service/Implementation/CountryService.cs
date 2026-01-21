using Booking.Domain.DomainModels;
using Booking.Domain.DTO;
using Booking.Repository.Interface;
using Booking.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Booking.Service.Implementation
{
    public class CountryService : ICountryService
    {
        private readonly IRepository<Country> _countryRepository;
        private readonly HttpClient _httpClient;

        public CountryService(IHttpClientFactory httpClientFactory, IRepository<Country> countryRepository)
        {
            _httpClient = httpClientFactory.CreateClient();
            _countryRepository = countryRepository;
        }

        public async Task<List<Country>> GetCountriesFromApi()
        {
            var apiCountries = await _httpClient
                .GetFromJsonAsync<List<CountryApiDTO>>(
                    "https://restcountries.com/v3.1/all?fields=name");

            var countries = apiCountries
                .Where(x => !string.IsNullOrEmpty(x.name.common))
                .Select(x => new Country
                {
                    Id = Guid.NewGuid(),
                    Name = x.name.common
                })
                .OrderBy(x => x.Name)
                .ToList();

            _countryRepository.InsertMany(countries);

            return countries;
        }

        public void SeedCountries(List<Country> countries)
        {
            foreach (var country in countries)
            {
                var exists = _countryRepository.Get(
                    selector: x => x,
                    predicate: x => x.Name == country.Name
                );

                if (exists == null)
                {
                    _countryRepository.Insert(country);
                }
            }
        }

        public List<Country> GetAllCountriesFromDb()
        {
            return _countryRepository.GetAll(selector: x => x).ToList();
        }

        public Country? GetById(Guid id)
        {
            return _countryRepository.Get(
                selector: x => x, 
                predicate: x => x.Id == id);
        }
    }
}
