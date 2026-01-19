using AdminApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace AdminApplication.Controllers
{
    public class HomeController : BaseApiController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        private IActionResult CheckAdminAccess()
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login", "AdminAuth");

            if (!IsAdmin())
                return View("NotAuthorized");

            return null;
        }

        public async Task<IActionResult> Index()
        {
            var check = CheckAdminAccess();
            if (check != null) return check;

            var client = GetClientWithToken();
            string URL = "http://localhost:5087/api/Admin/GetAllReservations";

            var response = await client.GetAsync(URL);
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Failed to load reservations. Make sure you are logged in.";
                return RedirectToAction("Login", "AdminAuth");
            }

            var result = await response.Content.ReadFromJsonAsync<List<Reservation>>();
            return View(result);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var check = CheckAdminAccess();
            if (check != null) return check;

            var client = GetClientWithToken();
            string URL = "http://localhost:5087/api/Admin/GetReservationDetails";

            var model = new { Id = id };
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(URL, content);
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Failed to load reservation details.";
                return RedirectToAction("Index");
            }

            var result = await response.Content.ReadFromJsonAsync<Reservation>();
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> AssignRole()
        {
            var check = CheckAdminAccess();
            if (check != null) return check;

            var client = GetClientWithToken();
            string URL = "http://localhost:5087/api/Admin/GetAllUsers";

            var response = await client.GetAsync(URL);
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Failed to load users.";
                return RedirectToAction("Index");
            }

            var users = await response.Content.ReadFromJsonAsync<List<BookingApplicationUserDTO>>();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(BookingApplicationUserDTO model)
        {
            var check = CheckAdminAccess();
            if (check != null) return check;

            if (model == null)
            {
                TempData["Error"] = "Invalid user data.";
                return RedirectToAction("AssignRole");
            }

            var client = GetClientWithToken();
            var response = await client.PostAsJsonAsync("http://localhost:5087/api/Admin/AssignRole", model);

            if (!response.IsSuccessStatusCode)
                TempData["Error"] = "Failed to assign role.";
            else
                TempData["Success"] = "Role assigned successfully.";

            return RedirectToAction("AssignRole");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
