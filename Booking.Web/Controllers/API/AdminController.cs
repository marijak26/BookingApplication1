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

        [HttpPost("[action]")]
        public bool ImportAllUsers(List<UserRegistrationDto> model)
        {
            bool status = true;

            foreach (var item in model)
            {
                var userCheck = _userManager.FindByEmailAsync(item.Email).Result;
                if (userCheck == null)
                {
                    var user = new BookingApplicationUser
                    {
                        FirstName = "Test Name",
                        LastName = "Test LastName",
                        UserName = item.Email,
                        NormalizedUserName = item.Email.ToUpper(),
                        Email = item.Email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        PhoneNumber = "",
                        ReservationCart = new ReservationCart()
                    };

                    var result = _userManager.CreateAsync(user, item.Password).Result;

                    if (result.Succeeded)
                    {
                        _userManager.AddToRoleAsync(user, "User").Wait();
                    }

                    status = status & result.Succeeded;
                }
            }

            return status;
        }

        [HttpPost("[action]")]
        public bool AssignRole(AssignRoleDto model)
        {
            var user = _userManager.FindByIdAsync(model.UserId.ToString()).Result;
            if (user == null) return false;

            var currentRoles = _userManager.GetRolesAsync(user).Result;
            _userManager.RemoveFromRolesAsync(user, currentRoles).Wait();

            var result = _userManager.AddToRoleAsync(user, model.RoleName).Result;
            return result.Succeeded;
        }
    }
}
