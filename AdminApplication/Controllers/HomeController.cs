using AdminApplication.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace AdminApplication.Controllers
{
    public class HomeController : BaseApiController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        private IActionResult CheckAdminAccess()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "AdminAuth");
            }

            if (!IsAdmin())
            {
                return View("NotAuthorized");
            }

            return null;
        }

        public async Task<IActionResult> Index()
        {
            var check = CheckAdminAccess();
            if (check != null)
            {
                return check;
            }

            var client = GetClientWithToken(_httpClientFactory.CreateClient("BookingApi"));
            var response = await client.GetAsync("/api/Admin/GetAllReservations");

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
            if (check != null)
            {
                return check;
            }

            var client = GetClientWithToken(_httpClientFactory.CreateClient("BookingApi"));
            var model = new { Id = id };
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/Admin/GetReservationDetails", content);
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
            if (check != null)
            {
                return check;
            }

            var client = GetClientWithToken(_httpClientFactory.CreateClient("BookingApi"));

            var response = await client.GetAsync("/api/Admin/GetAllUsers");
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
            if (check != null)
            {
                return check;
            }

            if (model == null)
            {
                TempData["Error"] = "Invalid user data.";
                return RedirectToAction("AssignRole");
            }

            var client = GetClientWithToken(_httpClientFactory.CreateClient("BookingApi"));
            var response = await client.PostAsJsonAsync("/api/Admin/AssignRole", model);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Failed to assign role.";
            }
            else
            {
                TempData["Success"] = "Role assigned successfully.";
            }

            return RedirectToAction("AssignRole");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> ExportReservations()
        {
            string fileName = "Reservations.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All Reservations");

                worksheet.Cell(1, 1).Value = "Reservation Id";
                worksheet.Cell(1, 2).Value = "Costumer Email";

                var client = GetClientWithToken(_httpClientFactory.CreateClient("BookingApi"));
                string URL = "/api/Admin/GetAllReservations";

                HttpResponseMessage response = await client.GetAsync(URL);

                var result = await response.Content.ReadFromJsonAsync<List<Reservation>>();

                for (int i = 1; i <= result.Count; i++)
                {
                    var currentReservation = result[i - 1];

                    int maxAccommodations = result.Max(r => r.AccommodationInReservations.Count);

                    worksheet.Cell(i + 1, 1).Value = currentReservation.Id.ToString();
                    worksheet.Cell(i + 1, 2).Value = currentReservation.User.Email;

                    for (int j = 0; j < currentReservation.AccommodationInReservations.Count(); j++)
                    {
                        var currentAccommodationInReservation = currentReservation.AccommodationInReservations.ToList().ElementAt(j);

                        worksheet.Cell(1, j + 3).Value = "Accommodation-" + (j + 1);
                        worksheet.Cell(i + 1, j + 3).Value = currentAccommodationInReservation.Accommodation.Name;
                    }
                    worksheet.Cell(1, maxAccommodations + 3).Value = "Total Price";
                    worksheet.Cell(i + 1, maxAccommodations + 3).Value = currentReservation.TotalPrice;
                    worksheet.Cell(i + 1, maxAccommodations + 3).Style.NumberFormat.Format = "€#,##0";
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }
            }
        }
    }
}
