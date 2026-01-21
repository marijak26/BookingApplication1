using Booking.Domain.DomainModels;
using Booking.Domain.Enum;
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
    public class ReservationsController : Controller
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        // GET: Reservations
        public IActionResult Index(string status)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var reservations = _reservationService.GetAllForUser(userId);

            if (!string.IsNullOrEmpty(status))
            {
                if (status == "active")
                {
                    reservations = reservations.Where(r => r.Status == ReservationStatus.Confirmed).ToList();
                }
                else if (status == "cancelled")
                {
                    reservations = reservations.Where(r => r.Status == ReservationStatus.Cancelled).ToList();
                }
            }

            return View(reservations);
        }

        // GET: Reservations/Details/5
        public IActionResult Details(Guid id)
        {
            var reservation = _reservationService.GetReservation(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        public IActionResult Cancel(Guid id)
        {
            _reservationService.CancelReservation(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
