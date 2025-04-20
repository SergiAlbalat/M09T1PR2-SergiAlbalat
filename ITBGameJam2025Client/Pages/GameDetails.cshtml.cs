using ITBGameJam2025Client.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ITBGameJam2025Client.Pages
{
    public class GameDetailsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public bool LoggedIn { get; set; }
        public bool Voted { get; set; } = false;
        public int Votes { get; set; }

        public GameDTO Game { get; set; } = new GameDTO();

        public GameDetailsModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGet(int id)
        {
            LoggedIn = Tools.TokenHelper.IsTokenSession(HttpContext.Session.GetString("AuthToken"));
            var client = _httpClientFactory.CreateClient("GamesApi");
            try
            {
                var gameInfo = await client.GetAsync($"api/Games/{id}");
                if (gameInfo == null || !gameInfo.IsSuccessStatusCode)
                {
                    _logger.LogError("Error al carregar les dades de Games");
                }
                else
                {
                    var json = await gameInfo.Content.ReadAsStringAsync();
                    Game = JsonSerializer.Deserialize<GameDTO>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            var votes = await client.GetAsync($"api/GameVotes/GetVotes/{id}");
            Votes = Convert.ToInt32(await votes.Content.ReadAsStringAsync());
            if (LoggedIn)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("AuthToken"));
                var response = await client.GetAsync("api/GameVotes/GetVotes");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    List<GameVoteDTO> gameVotes = JsonSerializer.Deserialize<List<GameVoteDTO>>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    GameVoteDTO? gameVote = gameVotes.FirstOrDefault(g => g.GameId == id);
                    if (gameVote != null)
                    {
                        Voted = true;
                    }
                    else
                    {
                        Voted = false;
                    }
                }
            }
        }

        public async Task<IActionResult> OnPostVote(int gameId)
        {
            var client = _httpClientFactory.CreateClient("GamesApi");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("AuthToken"));
            var response = await client.PostAsJsonAsync("api/GameVotes/Vote", gameId);
            return RedirectToPage("GameDetails", new { id = gameId });
        }

        public async Task<IActionResult> OnPostUnVote(int gameId)
        {
            var client = _httpClientFactory.CreateClient("GamesApi");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("AuthToken"));
            var response = await client.PostAsJsonAsync($"api/GameVotes/Unvote", gameId);
            return RedirectToPage("GameDetails", new { id = gameId });
        }
    }
}
