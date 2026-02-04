using Booking.Domain.DomainModels;
using Booking.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace Booking.Web.Controllers
{
    public class AccommodationHostsController : Controller
    {
        private readonly IAccommodationHostService _hostService;
        private readonly ICountryService _countryService;

        public AccommodationHostsController(
            IAccommodationHostService hostService,
            ICountryService countryService)
        {
            _hostService = hostService;
            _countryService = countryService;
        }

        // GET: AccommodationHosts
        public IActionResult Index()
        {
            return View(_hostService.GetAll());
        }

        // GET: AccommodationHosts/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null) return NotFound();

            var host = _hostService.GetById(id.Value);
            if (host == null) return NotFound();

            return View(host);
        }

        // GET: AccommodationHosts/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        // POST: AccommodationHosts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([Bind("FullName,ContactEmail,CountryId,CityId,Id")] AccommodationHost host)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdowns(host.CountryId, host.CityId);
                return View(host);
            }

            _hostService.Insert(host);
            return RedirectToAction(nameof(Index));
        }

        // GET: AccommodationHosts/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var host = _hostService.GetById(id.Value);
            if (host == null) return NotFound();

            LoadDropdowns(host.CountryId, host.CityId);
            return View(host);
        }

        // POST: AccommodationHosts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid id, [Bind("FullName,ContactEmail,CountryId,CityId,Id")] AccommodationHost host)
        {
            if (id != host.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                LoadDropdowns(host.CountryId, host.CityId);
                return View(host);
            }

            _hostService.Update(host);
            return RedirectToAction(nameof(Index));
        }

        // GET: AccommodationHosts/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var host = _hostService.GetById(id.Value);
            if (host == null) return NotFound();

            return View(host);
        }

        // POST: AccommodationHosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _hostService.DeleteById(id);
            return RedirectToAction(nameof(Index));
        }

        private void LoadDropdowns(Guid? selectedCountryId = null, Guid? selectedCityId = null)
        {
            var countries = _countryService.GetAllCountriesFromDb()
                                           .Select(c => new { c.Id, c.Name })
                                           .OrderBy(c => c.Name)
                                           .ToList();

            ViewData["CountryId"] = new SelectList(countries, "Id", "Name", selectedCountryId);

            List<City> cities = new List<City>();
            if (selectedCountryId.HasValue)
            {
                cities = _countryService.GetAllCountriesFromDb()
                            .Where(c => c.Id == selectedCountryId.Value)
                            .SelectMany(c => c.Cities)
                            .OrderBy(c => c.Name)
                            .ToList();
            }

            ViewData["CityId"] = new SelectList(cities, "Id", "Name", selectedCityId);
        }

    }
}
