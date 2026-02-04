using AdminApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace AdminApplication.Controllers
{
    public class AdminAuthController : BaseApiController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminAuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginDto model)
        {
            ClearAdminToken();

            var client = _httpClientFactory.CreateClient("BookingApi");
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/Auth/Login", content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            var token = result["token"];

            Console.WriteLine("Admin JWT: " + token);

            SetAdminToken(token);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            ClearAdminToken();
            return RedirectToAction("Login");
        }
    }
}
