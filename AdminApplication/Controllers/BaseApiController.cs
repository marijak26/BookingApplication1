using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;

namespace AdminApplication.Controllers
{
    public class BaseApiController : Controller
    {
        private const string AdminTokenKey = "JWTokenAdmin";

        protected HttpClient GetClientWithToken()
        {
            var token = HttpContext.Session.GetString(AdminTokenKey);
            var client = new HttpClient();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

        protected bool IsLoggedIn()
        {
            var token = HttpContext.Session.GetString(AdminTokenKey);
            return !string.IsNullOrEmpty(token);
        }

        protected bool IsAdmin()
        {
            var token = HttpContext.Session.GetString(AdminTokenKey);
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            return jwtToken.Claims.Any(c =>
                (c.Type == "role" || c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                && c.Value == "Admin");
        }

        protected void ClearAdminToken()
        {
            HttpContext.Session.Remove(AdminTokenKey);
        }

        protected void SetAdminToken(string token)
        {
            HttpContext.Session.SetString(AdminTokenKey, token);
        }
    }
}
