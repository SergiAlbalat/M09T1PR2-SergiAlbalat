using Microsoft.AspNetCore.SignalR;

namespace ITBGameJam2025Api.HUBs
{
    public class XatHub : Hub
    {
        public async Task EnviaMissatge(string usuari, string missatge)
        {
            await Clients.All.SendAsync("RepMissatge", usuari, missatge);
        }
    }
}
