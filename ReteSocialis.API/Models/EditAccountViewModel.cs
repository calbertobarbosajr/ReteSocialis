using System.ComponentModel.DataAnnotations;

namespace ReteSocialis.API.Models
{
    public class EditAccountViewModel
    {
        [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
        public string Username { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Senha Atual")]
        public string? OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nova Senha")]
        public string? NewPassword { get; set; }
    }
}