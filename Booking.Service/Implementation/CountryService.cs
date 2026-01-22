using Booking.Domain.DomainModels;
using Booking.Domain.DTO;
using Booking.Repository.Interface;
using Booking.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Booking.Service.Implementation
{
    public class CountryService : ICountryService
    {
        private readonly IRepository<Country> _countryRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly HttpClient _httpClient;

        public CountryService(IHttpClientFactory httpClientFactory, IRepository<Country> countryRepository, IRepository<City> cityRepository)
        {
            _httpClient = httpClientFactory.CreateClient();
            _countryRepository = countryRepository;
            _cityRepository = cityRepository;
        }

        public async Task<List<Country>> GetCountriesFromApi()
        {
            var apiResponse = await _httpClient
                .GetFromJsonAsync<CountryApiDTO>("https://countriesnow.space/api/v0.1/countries");

            if (apiResponse?.data == null || !apiResponse.data.Any())
                return new List<Country>();

            var existingCountryNames = _countryRepository
                .GetAll(x => x.Name)
                .ToList();

            var newCountries = new List<Country>();

            foreach (var dto in apiResponse.data)
            {
                if (string.IsNullOrWhiteSpace(dto.country))
                    continue;

                if (existingCountryNames.Contains(dto.country))
                    continue;

                var countryId = Guid.NewGuid();

                var country = new Country
                {
                    Id = countryId,
                    Name = dto.country,
                    Cities = dto.cities
                        .Where(c => !string.IsNullOrWhiteSpace(c))
                        .Take(5)
                        .Select(cityName => new City
                        {
                            Id = Guid.NewGuid(),
                            Name = cityName,
                            CountryId = countryId
                        })
                        .ToList()
                };

                newCountries.Add(country);
            }

            if (newCountries.Any())
            {
                _countryRepository.InsertMany(newCountries);
            }

            return newCountries;
        }

        public List<Country> GetAllCountriesFromDb()
        {
            return _countryRepository.GetAll(
                selector: x => x,
                include: q => q.Include(c => c.Cities)).ToList();
        }

        public Country? GetById(Guid id)
        {
            return _countryRepository.Get(
                selector: x => x,
                predicate: x => x.Id == id
            );
        }
    }
}
