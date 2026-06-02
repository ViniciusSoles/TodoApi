using ToDo.Web.Pages.ViewModels.Todos;

namespace ToDo.Web.Pages.ViewModels
{
    public class TodoPageViewModel
    {
       public List<TodoListViewModel> Todos { get; set; }
       public int TotalPendentes { get; set; }
       public int TotalConcluidos { get; set; }
       public CreateTodoViewModel NovoTodo { get; set; } // ← formulário na mesma página
    }   
}
