using System.ComponentModel.DataAnnotations;

namespace ToDo.Web.Pages.ViewModels.Auth
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Nome é obrigatório.")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email é obrigatório.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória.")]
        [MinLength(8, ErrorMessage = "Mínimo 8 caracteres.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirmação é obrigatória.")]
        [Compare("Password", ErrorMessage = "Senhas não conferem.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
