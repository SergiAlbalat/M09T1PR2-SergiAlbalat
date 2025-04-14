using ITBGameJam2025Client.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ITBGameJam2025Client.Pages
{
    public class GamesModel : PageModel
    {
        private readonly ILogger<GamesModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public List<GameDTO> Games { get; set; } = new List<GameDTO>();
        public GamesModel(ILogger<GamesModel> logger, IHttpClientFactory httpClientFactory)
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
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
