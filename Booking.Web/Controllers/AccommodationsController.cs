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
    [Authorize]
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
            var accommodations = _accommodationService.GetAll();

            if (countryId.HasValue)
            {
                accommodations = _accommodationService.GetByCountry(countryId.Value);
            }

            var countries = _countryService.GetAllCountriesFromDb()
                .OrderBy(c => c.Name)
                .ToList();
            ViewData["Countries"] = new SelectList(countries, "Id", "Name");

            return View(accommodations);
        }

        // GET: Accommodations/Details/5
        public IActionResult Details(Guid id)
        {
            var acc = _accommodationService.GetById(id);
            if (acc == null)
            {
                return NotFound();
            }
            return View(acc);
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
        public IActionResult Create([Bind("Name,Description,PricePerNight,IsRented,Category,HostId")]
        Accommodation accommodation,
        IFormFile Image)
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

            if (Image != null && Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/images/accommodations");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(Image.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }

                accommodation.ImageUrl = "/images/accommodations/" + fileName;
            }
            else
            {
                accommodation.ImageUrl = "/images/accommodations/default.jpg";
            }

            accommodation.Id = Guid.NewGuid();
            _accommodationService.Insert(accommodation);

            return RedirectToAction(nameof(Index));
        }




        // GET: Accommodations/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid id)
        {
            var acc = _accommodationService.GetById(id);
            if (acc == null)
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
            return View(acc);
        }

        // POST: Accommodations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(
    Guid id,
    [Bind("Id,Name,Description,PricePerNight,IsRented,Category,HostId")] Accommodation accommodation,
    IFormFile Image)
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

            var existing = _accommodationService.GetById(id);
            if (existing == null)
                return NotFound();

            if (Image != null && Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/accommodations");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(Image.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }

                if (!string.IsNullOrEmpty(existing.ImageUrl) && !existing.ImageUrl.Contains("default.jpg"))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existing.ImageUrl.TrimStart('/').Replace("/", "\\"));
                    if (System.IO.File.Exists(oldFilePath))
                        System.IO.File.Delete(oldFilePath);
                }

                existing.ImageUrl = "/images/accommodations/" + fileName;
            }

            existing.Name = accommodation.Name;
            existing.Description = accommodation.Description;
            existing.PricePerNight = accommodation.PricePerNight;
            existing.IsRented = accommodation.IsRented;
            existing.Category = accommodation.Category;
            existing.HostId = accommodation.HostId;

            _accommodationService.Update(existing);

            return RedirectToAction(nameof(Index));
        }


        // GET: Accommodations/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid id)
        {
            var acc = _accommodationService.GetById(id);
            if (acc == null)
            {
                return NotFound();
            }
            return View(acc);
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

        public IActionResult AddToCart(Guid id)
        {
            var model = _accommodationService.GetSelectedAccommodation(id);
            return View("AddToReservationCart", model);
        }

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
                return Unauthorized();

            var userId = Guid.Parse(userIdClaim);

            _accommodationService.AddAccommodationToReservationCart(
                model.SelectedAccommodationId,
                userId,
                model.FromDate,
                model.ToDate
            );

            return RedirectToAction("Index", "ReservationCarts");
        }

        [HttpPost]
        public IActionResult CheckAvailability(Guid accommodationId, DateTime from, DateTime to)
        {
            var available = _accommodationService
                .IsAccommodationAvailable(accommodationId, from, to);

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
