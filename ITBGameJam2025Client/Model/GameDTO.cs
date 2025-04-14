using System.ComponentModel.DataAnnotations;

namespace ITBGameJam2025Client.Model
{
    public class GameDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Aquest camp és obligatori")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Aquest camp és obligatori")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Aquest camp és obligatori")]
        public string? Developer { get; set; }
        public string? Image { get; set; }
    }
}
