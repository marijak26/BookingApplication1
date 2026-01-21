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
            ClearCountriesAndCities();
            var apiResponse = await _httpClient
                .GetFromJsonAsync<CountryApiDTO>("https://countriesnow.space/api/v0.1/countries");

            if (apiResponse?.data == null || !apiResponse.data.Any())
                return new List<Country>();

            var countries = apiResponse.data
                .Where(dto => !string.IsNullOrEmpty(dto.country))
                .Select(dto =>
                {
                    var countryId = Guid.NewGuid();
                    return new Country
                    {
                        Id = countryId,
                        Name = dto.country,
                        Cities = dto.cities
                            .Where(c => !string.IsNullOrEmpty(c))
                            .Take(5)
                            .Select(cityName => new City
                            {
                                Id = Guid.NewGuid(),
                                Name = cityName,
                                CountryId = countryId
                            }).ToList()
                    };
                })
                .ToList();

            _countryRepository.InsertMany(countries);

            return countries;
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
        public void ClearCountriesAndCities()
        {
            var countries = _countryRepository.GetAll(
                x => x,
                include: q => q.Include(c => c.Cities)
            ).ToList();

            var allCities = countries.SelectMany(c => c.Cities).ToList();

            if (allCities.Any())
                _cityRepository.DeleteMany(allCities);

            if (countries.Any())
                _countryRepository.DeleteMany(countries);
        }
    }
}
