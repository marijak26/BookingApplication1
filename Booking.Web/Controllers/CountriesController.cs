using Booking.Domain.DomainModels;
using Booking.Repository;
using Booking.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.Web.Controllers
{
    [Authorize]
    public class CountriesController : Controller
    {
        private readonly ICountryService _countryService;

        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Import()
        {
            var countries = await _countryService.GetCountriesFromApi();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Index()
        {
            var countries = _countryService.GetAllCountriesFromDb()
                               .OrderBy(c => c.Name)
                               .ToList();

            ViewData["CountryId"] = new SelectList(countries, "Id", "Name");
            return View(countries);
        }
    }
}
