using ITBGameJam2025Api.Model;
using Microsoft.EntityFrameworkCore;

namespace ITBGameJam2025Api.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<GameVote> GameVotes { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
