using Microsoft.AspNetCore.SignalR;

namespace ITBGameJam2025Api.HUBs
{
    public class XatHub : Hub
    {
        /// <summary>
        /// Method for sending a message to all clients
        /// </summary>
        /// <param name="usuari">User that sends the message</param>
        /// <param name="missatge">The content of the message</param>
        /// <returns></returns>
        public async Task EnviaMissatge(string usuari, string missatge)
        {
            await Clients.All.SendAsync("RepMissatge", usuari, missatge);
        }
    }
}
