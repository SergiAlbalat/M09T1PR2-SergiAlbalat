using ITBGameJam2025Client.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ITBGameJam2025Client.Pages
{
    public class GameDetailsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public GameDTO Game { get; set; } = new GameDTO();

        public GameDetailsModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public void OnGet()
        {
        }

        public async Task OnGetGame(int id)
        {
            var client = _httpClientFactory.CreateClient("GamesApi");
            try
            {
                var response = await client.GetAsync($"api/Games/{id}");
                if (response == null || !response.IsSuccessStatusCode)
                {
                    _logger.LogError("Error al carregar les dades de Games");
                }
                else
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Game = JsonSerializer.Deserialize<GameDTO>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public IActionResult OnPostVote(int gameId)
        {
            return RedirectToPage("Index");
        }
    }
}
