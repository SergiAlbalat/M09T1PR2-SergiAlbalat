using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITBGameJam2025Api.Model
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Aquest camp és obligatori")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Aquest camp és obligatori")]
        public string? Password { get; set; }
    }
}
