using Booking.Domain.DomainModels;
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
    public class ReservationCartsController : Controller
    {
        private readonly IReservationCartService _reservationCartService;


        public ReservationCartsController(IReservationCartService reservationCartService)
        {
            _reservationCartService = reservationCartService;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var model = _reservationCartService.GetByUserIdWithIncludedAccommodations(Guid.Parse(userId));
            return View(model);
        }


        public IActionResult Delete(Guid id)
        {
            _reservationCartService.DeleteAccommodationFromReservationCart(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Confirm()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _reservationCartService.ConfirmReservation(Guid.Parse(userId));
            return RedirectToAction("Index", "Reservations");
        }

        public IActionResult Cancel()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            _reservationCartService.CancelReservation(Guid.Parse(userId));

            return RedirectToAction("Index", "Reservations");
        }



    }
}
