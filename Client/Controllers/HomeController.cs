using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy() => View();

        [Authorize]
        [HttpGet("/call-api")]
        public async Task<IActionResult> CallApi()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            if (accessToken == null) throw new InvalidOperationException("Could not find access token");
            
            var client = new HttpClient(); // you shouldn't do this. Instead use IHttpClientFactory
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync("https://localhost:5001/weatherforecast");

            return Ok(response.IsSuccessStatusCode 
                ? "API access authorized!" : $"API access failed. Status code: {response.StatusCode}");
        }
    }
}
