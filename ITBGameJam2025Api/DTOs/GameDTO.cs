using System.ComponentModel.DataAnnotations;

namespace ITBGameJam2025Api.DTOs
{
    public class GameDTO
    {
        [Required(ErrorMessage = "Aquest camp és obligatori")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Aquest camp és obligatori")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Aquest camp és obligatori")]
        public string? Developer { get; set; }
        public string? Image { get; set; }
    }
}
