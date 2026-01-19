using Booking.Domain.DomainModels;
using Booking.Domain.DTO;
using Booking.Domain.Identity;
using Booking.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<BookingApplicationUser> _userManager;
        private readonly IReservationService _reservationService;
        public AdminController(UserManager<BookingApplicationUser> userManager, IReservationService reservationService)
        {
            _userManager = userManager;
            _reservationService = reservationService;
        }

        [HttpGet("[action]")]

        public List<Reservation> GetAllReservations()
        {
            return _reservationService.GetAllReservations();
        }

        [HttpPost("[action]")]
        public Reservation GetReservationDetails(BaseEntity model)
        {
            return _reservationService.GetReservation(model.Id);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            var userList = new List<BookingApplicationUserDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new BookingApplicationUserDTO
                {
                    Id = Guid.Parse(user.Id),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    RoleName = roles.FirstOrDefault() ?? "User"
                });
            }

            return Ok(userList);
        }


        [HttpPost("[action]")]
        public bool AssignRole(BookingApplicationUserDTO model)
        {
            if (model == null || model.Id == Guid.Empty || string.IsNullOrEmpty(model.RoleName))
                return false;

            var user = _userManager.FindByIdAsync(model.Id.ToString()).Result;
            if (user == null) return false;

            var currentRoles = _userManager.GetRolesAsync(user).Result;
            if (currentRoles.Any())
                _userManager.RemoveFromRolesAsync(user, currentRoles).Wait();

            var result = _userManager.AddToRoleAsync(user, model.RoleName).Result;

            return result.Succeeded;
        }

        [HttpGet("[action]")]
        public async Task<string> GetUserRole(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return "Unknown";

            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault() ?? "User";
        }


    }
}
