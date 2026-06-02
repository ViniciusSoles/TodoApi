using System.ComponentModel.DataAnnotations;

namespace ToDo.Web.Pages.ViewModels.Todos
{
    public class CreateTodoViewModel
    {
        [Required(ErrorMessage = "Título é obrigatório.")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        public string Title { get; set; }

        [MaxLength(500, ErrorMessage = "Máximo 500 caracteres.")]
        public string? Description { get; set; }
    }
}
