using AdminApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace AdminApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true
            };
            var client = new HttpClient(clientHandler);
            string URL = "http://localhost:5087/api/Admin/GetAllReservations";

            HttpResponseMessage response = await client.GetAsync(URL);

            var result = await response.Content.ReadFromJsonAsync<List<Reservation>>();

            return View(result);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true
            };
            var client = new HttpClient(clientHandler);


            string URL = "http://localhost:5087/api/Admin/GetReservationDetails";

            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(URL, content);

            var result = await response.Content.ReadFromJsonAsync<Reservation>();


            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
