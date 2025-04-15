using System.ComponentModel.DataAnnotations;

namespace ITBGameJam2025Client.Model
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Aquest camp és obligatori")]
        [EmailAddress(ErrorMessage = "El format del correu electronic és incorrecte")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Aquest camp és obligatori")]
        public string Password { get; set; }
    }
}
