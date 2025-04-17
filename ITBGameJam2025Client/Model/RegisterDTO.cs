using System.ComponentModel.DataAnnotations;

namespace ITBGameJam2025Client.Model
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Aquest camp és obligatori")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Aquest camp és obligatori")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Aquest camp és obligatori")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Aquest camp és obligatori")]
        public string Surname { get; set; }
    }
}
