using Booking.Domain.DomainModels;
using Booking.Domain.DTO;
using Booking.Domain.Enum;
using Booking.Repository;
using Booking.Service.Implementation;
using Booking.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Booking.Web.Controllers
{
    public class AccommodationsController : Controller
    {
        private readonly IAccommodationService _accommodationService;
        private readonly ICountryService _countryService;

        public AccommodationsController(IAccommodationService accommodationService, ICountryService countryService)
        {
            _accommodationService = accommodationService;
            _countryService = countryService;
        }

        // GET: Accommodations
        public IActionResult Index(Guid? countryId)
        {
            var accommodations = _accommodationService.GetAllWithHostsAndLocations();

            if (countryId.HasValue)
            {
                accommodations = accommodations
                    .Where(a => a.Host != null && a.Host.CountryId == countryId.Value)
                    .ToList();
            }

            var grouped = accommodations
                .Where(a => a.Host != null && a.Host.Country != null && a.Host.City != null)
                .GroupBy(a => a.Host.Country.Name)
                .ToDictionary(
                    countryGroup => countryGroup.Key,
                    countryGroup => countryGroup
                        .GroupBy(a => a.Host.City.Name)
                        .ToDictionary(
                            cityGroup => cityGroup.Key,
                            cityGroup => cityGroup.ToList()
                        )
                );

            var countries = _countryService.GetAllCountriesFromDb()
                .Where(c => _accommodationService.GetByCountry(c.Id).Any())
                .OrderBy(c => c.Name)
                .ToList();

            ViewData["Countries"] = new SelectList(countries, "Id", "Name");

            return View(grouped);
        }

        // GET: Accommodations/Details/5
        public IActionResult Details(Guid id)
        {
            var accommodation = _accommodationService.GetById(id);
            if (accommodation == null)
            {
                return NotFound();
            }
            return View(accommodation);
        }

        // GET: Accommodations/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["Category"] = Enum.GetValues(typeof(AccommodationCategory))
                                       .Cast<AccommodationCategory>()
                                       .Select(c => new SelectListItem
                                       {
                                           Text = c.ToString(),
                                           Value = ((int)c).ToString()
                                       });

            var hosts = _accommodationService.GetAllHosts().ToList();
            ViewData["HostId"] = new SelectList(hosts, "Id", "FullName");

            return View();
        }

        // POST: Accommodations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([Bind("Name,Description,PricePerNight,IsRented,Category,HostId,ImageUrl")] Accommodation accommodation)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Category"] = Enum.GetValues(typeof(AccommodationCategory))
                    .Cast<AccommodationCategory>()
                    .Select(c => new SelectListItem
                    {
                        Text = c.ToString(),
                        Value = ((int)c).ToString()
                    });

                var hosts = _accommodationService.GetAllHosts().ToList();
                ViewData["HostId"] = new SelectList(hosts, "Id", "FullName");

                return View(accommodation);
            }

            _accommodationService.Insert(accommodation);

            return RedirectToAction(nameof(Index));
        }

        // GET: Accommodations/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid id)
        {
            var accommodation = _accommodationService.GetById(id);
            if (accommodation == null)
            {
                return NotFound();
            }
            ViewData["Category"] = Enum.GetValues(typeof(AccommodationCategory))
                                       .Cast<AccommodationCategory>()
                                       .Select(c => new SelectListItem
                                       {
                                           Text = c.ToString(),
                                           Value = ((int)c).ToString()
                                       });

            var hosts = _accommodationService.GetAllHosts().ToList();
            ViewData["HostId"] = new SelectList(hosts, "Id", "FullName");
            return View(accommodation);
        }

        // POST: Accommodations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid id, [Bind("Id,Name,Description,PricePerNight,IsRented,Category,HostId,ImageUrl")] Accommodation accommodation)
        {
            if (id != accommodation.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewData["Category"] = Enum.GetValues(typeof(AccommodationCategory))
                    .Cast<AccommodationCategory>()
                    .Select(c => new SelectListItem { Text = c.ToString(), Value = ((int)c).ToString() });

                var hosts = _accommodationService.GetAllHosts().ToList();
                ViewData["HostId"] = new SelectList(hosts, "Id", "FullName", accommodation.HostId);

                return View(accommodation);
            }

            _accommodationService.Update(accommodation);
            return RedirectToAction(nameof(Index));
        }

        // GET: Accommodations/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid id)
        {
            var accommodation = _accommodationService.GetById(id);
            if (accommodation == null)
            {
                return NotFound();
            }
            return View(accommodation);
        }

        // POST: Accommodations/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _accommodationService.DeleteById(id);
            return RedirectToAction(nameof(Index));
        }

        private bool AccommodationExists(Guid id)
        {
            return _accommodationService.GetById(id) != null;
        }
        [Authorize]
        public IActionResult AddToCart(Guid id)
        {
            var model = _accommodationService.GetSelectedAccommodation(id);
            return View("AddToReservationCart", model);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(AddToReservationCartDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View("AddToReservationCart", model);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(userIdClaim);

            _accommodationService.AddAccommodationToReservationCart(model.SelectedAccommodationId, userId, model.FromDate, model.ToDate);

            return RedirectToAction("Index", "ReservationCarts");
        }

        [HttpPost]
        public IActionResult CheckAvailability(Guid accommodationId, DateTime from, DateTime to)
        {
            var available = _accommodationService.IsAccommodationAvailable(accommodationId, from, to);

            return Json(new { available });
        }

        [HttpGet]
        public IActionResult Calendar(Guid id)
        {
            var events = _accommodationService.GetAccommodationCalendar(id);
            return Json(events);
        }
    }
}
