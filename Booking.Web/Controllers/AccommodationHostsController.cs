using Booking.Domain.DomainModels;
using Booking.Repository;
using Booking.Service.Implementation;
using Booking.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booking.Web.Controllers
{
    [Authorize]
    public class AccommodationHostsController : Controller
    {
        private readonly IAccommodationHostService _hostService;
        private readonly ICountryService _countryService;

        public AccommodationHostsController(IAccommodationHostService hostService, ICountryService countryService)
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
            if (id == null) { 
                return NotFound(); 
            }

            var host = _hostService.GetById(id.Value);
            if (host == null)
            {
                return NotFound();
            }

            return View(host);
        }

        // GET: AccommodationHosts/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            LoadCountriesDropdown();
            return View();
        }

        // POST: AccommodationHosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([Bind("FullName,ContactEmail,CountryId,Id")] AccommodationHost accommodationHost)
        {
            if (!ModelState.IsValid)
            {
                LoadCountriesDropdown();
                return View(accommodationHost);
            }

            _hostService.Insert(accommodationHost);
            return RedirectToAction(nameof(Index));
        }

        // GET: AccommodationHosts/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid? id)
        {
            if (id == null) 
            { 
                return NotFound(); 
            }

            var host = _hostService.GetById(id.Value);
            if (host == null)
            {
                return NotFound();
            }

            LoadCountriesDropdown(host.CountryId);
            return View(host);
        }

        // POST: AccommodationHosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid id, [Bind("FullName,ContactEmail,CountryId,Id")] AccommodationHost accommodationHost)
        {
            if (id != accommodationHost.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                LoadCountriesDropdown(accommodationHost.CountryId);
                return View(accommodationHost);
            }

            _hostService.Update(accommodationHost);
            return RedirectToAction(nameof(Index));
        }

        // GET: AccommodationHosts/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var host = _hostService.GetById(id.Value);
            if (host == null)
            {
                return NotFound();
            }

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

        private bool AccommodationHostExists(Guid id)
        {
            return _hostService.GetById(id) != null;
        }

        private void LoadCountriesDropdown(Guid? selectedCountryId = null)
        {
            var countries = _countryService.GetAllCountriesFromDb()
                                   .OrderBy(c => c.Name)
                                   .ToList();
            ViewData["CountryId"] = new SelectList(countries, "Id", "Name", selectedCountryId);
        }
    }
}
