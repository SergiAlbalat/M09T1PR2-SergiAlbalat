using System.ComponentModel.DataAnnotations;

namespace ITBGameJam2025Api.DTOs
{
    public class GameDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Developer { get; set; }
        public string? Image { get; set; }
    }
}
