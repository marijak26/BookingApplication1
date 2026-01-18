using Booking.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service.Extensions
{
    public static class CountrySeeder
    {
        public static async Task SeedCountriesAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var countryService = scope.ServiceProvider.GetRequiredService<ICountryService>();

            var existing = countryService.GetAllCountriesFromDb();
            if (existing.Any())
                return;

            var countries = await countryService.GetCountriesFromApi();
            countryService.SeedCountries(countries);
        }
    }
}
