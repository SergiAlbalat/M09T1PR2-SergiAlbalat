using ITBGameJam2025Client.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ITBGameJam2025Client.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public List<GameDTO> Games { get; set; } = new List<GameDTO>();
        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGet()
        {
            var client = _httpClientFactory.CreateClient("GamesApi");
            try
            {
                var response = await client.GetAsync("api/Games");
                if (response == null || !response.IsSuccessStatusCode)
                {
                    _logger.LogError("Error al carregar les dades de Games");
                }
                else
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Games = JsonSerializer.Deserialize<List<GameDTO>>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        /// <summary>
        /// Method for redirecting to the game details page
        /// </summary>
        /// <param name="id">The id of the game that the user wants details</param>
        /// <returns>The page with the details</returns>
        public IActionResult OnPostGameInfo(int id)
        {
            return RedirectToPage("GameDetails", new { id = id });
        }
    }
}
