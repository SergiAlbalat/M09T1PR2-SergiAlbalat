using ITBGameJam2025Client.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ITBGameJam2025Client.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<LoginModel> _logger;
        [BindProperty]
        public LoginDTO Login { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public LoginModel(IHttpClientFactory httpClient, ILogger<LoginModel> logging)
        {
            _httpClient = httpClient;
            _logger = logging;
        }

        public void OnGet()
        {
        }

        /// <summary>
        /// Method for logging in the user
        /// </summary>
        /// <returns>A cookie with the user session token and the index page if did correctly</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            try
            {
                var client = _httpClient.CreateClient("GamesApi");
                var response = await client.PostAsJsonAsync("api/Auth/login", Login);

                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(token))
                    {
                        HttpContext.Session.SetString("AuthToken", token);
                        _logger.LogInformation("Login susccesfull");
                        return RedirectToPage("/Index");
                    }
                }
                else
                {
                    _logger.LogInformation("Login failed");
                    ErrorMessage = "Credencials incorrectes o acc�s no autoritzat.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durant el login");
                ErrorMessage = "Error inesperat. Torna-ho a intentar.";
            }
            return Page();
        }
    }
}
