using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITBGameJam2025Api.Model
{
    public class Game
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Aquest camp és obligatori")]
        public string? Title {  get; set; }
        [Required(ErrorMessage = "Aquest camp és obligatori")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Aquest camp és obligatori")]
        public string? Developer { get; set; }
        public string? Image {  get; set; }
    }
}
