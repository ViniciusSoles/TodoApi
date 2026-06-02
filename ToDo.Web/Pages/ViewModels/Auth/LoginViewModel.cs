using System.ComponentModel.DataAnnotations;

namespace ToDo.Web.Pages.ViewModels.Auth
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; } // ← checkbox "lembrar de mim"
    }
}
