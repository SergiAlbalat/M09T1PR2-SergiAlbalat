using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net.Http;

namespace ITBGameJam2025Client.Pages
{
    public class UsersXatModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;

        public bool LoggedIn { get; set; } = false;
        public string UserName { get; set; }

        public UsersXatModel(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task OnGet()
        {
            LoggedIn = Tools.TokenHelper.IsTokenSession(HttpContext.Session.GetString("AuthToken"));
            if (!LoggedIn)
            {
                GoIndex();
            }
            else
            {
                var client = _httpClient.CreateClient("GamesApi");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("AuthToken"));
                var response = await client.GetAsync("api/Auth/GetName");
                UserName = await response.Content.ReadAsStringAsync();
            }
        }

        private IActionResult GoIndex()
        {
            return RedirectToPage("index");
        }
    }
}
