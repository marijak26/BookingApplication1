using Booking.Domain.DomainModels;
using Booking.Domain.DTO;
using Booking.Repository.Interface;
using Booking.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Booking.Service.Implementation
{
    public class CountryService : ICountryService
    {
        private readonly IRepository<Country> _countryRepository;
        private readonly HttpClient _httpClient;

        public CountryService(IRepository<Country> countryRepository, HttpClient httpClient)
        {
            _countryRepository = countryRepository;
            _httpClient = httpClient;
        }

        public async Task<List<Country>> GetCountriesFromApi()
        {
            var response = await _httpClient.GetAsync("https://restcountries.com/v3.1/all?fields=name");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var apiCountries = JsonSerializer.Deserialize<List<CountryApiDTO>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return apiCountries
                .Where(x => !string.IsNullOrEmpty(x.name.common))
                .Select(x => new Country
                {
                    Id = Guid.NewGuid(),
                    Name = x.name.common
                })
                .OrderBy(x => x.Name)
                .ToList();
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
