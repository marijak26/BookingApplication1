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
using System.Security.Claims;
using System.Threading.Tasks;

namespace Booking.Web.Controllers
{
    [Authorize]
    public class ReservationCartsController : Controller
    {
        private readonly IReservationCartService _reservationCartService;

        public ReservationCartsController(IReservationCartService reservationCartService)
        {
            _reservationCartService = reservationCartService;
        }

        // GET: ReservationCarts
        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userReservationCart = _reservationCartService.GetByUserIdWithIncludedAccommodations(Guid.Parse(userId));
            return View(userReservationCart);
        }

        public IActionResult Delete(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _reservationCartService.DeleteAccommodationFromReservationCart(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Confirm(Guid cartItemId)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = _reservationCartService.ConfirmReservation(cartItemId, userId);

            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction("Index");
            }

            TempData["Success"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ConfirmCart()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = _reservationCartService.ConfirmWholeCart(userId);

            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction("Index");
            }

            TempData["Success"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Clear()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _reservationCartService.ClearCart(userId);

            return RedirectToAction("Index");
        }
    }
}
