using Booking.Domain.DomainModels;
using Booking.Domain.DTO;
using Booking.Domain.Enum;
using Booking.Repository;
using Booking.Service.Interface;
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

        public AccommodationsController(IAccommodationService accommodationService)
        {
            _accommodationService = accommodationService;
        }

        // GET: Accommodations
        public IActionResult Index()
        {
            return View(_accommodationService.GetAll());
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
        public IActionResult Create([Bind("Name,Description,PricePerNight,IsRented,Category,HostId,Id")] Accommodation accommodation)
        {
            if (ModelState.IsValid)
            {
                accommodation.Id = Guid.NewGuid();
                _accommodationService.Insert(accommodation);
                return RedirectToAction(nameof(Index));
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



        // GET: Accommodations/Edit/5
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
        public IActionResult Edit(Guid id, [Bind("Name,Description,PricePerNight,IsRented,Category,HostId,Id")] Accommodation accommodation)
        {
            if (id != accommodation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _accommodationService.Update(accommodation);
                return RedirectToAction(nameof(Index));
            }

            ViewData["Category"] = Enum.GetValues(typeof(AccommodationCategory))
                                       .Cast<AccommodationCategory>()
                                       .Select(c => new SelectListItem
                                       {
                                           Text = c.ToString(),
                                           Value = ((int)c).ToString(),
                                           Selected = c == accommodation.Category
                                       }).ToList();

            var hosts = _accommodationService.GetAllHosts().ToList();
            ViewData["HostId"] = new SelectList(hosts, "Id", "FullName", accommodation.HostId);

            return View(accommodation);
        }

        // GET: Accommodations/Delete/5
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
        public IActionResult AddToCart(AddToReservationCartDTO model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _accommodationService.AddAccommodationToReservationCart(
                model.SelectedAccommodationId,
                Guid.Parse(userId),
                model.Nights);

            return RedirectToAction("Index", "ReservationCarts");

        }
    }
}
