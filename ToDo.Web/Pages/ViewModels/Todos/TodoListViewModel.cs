namespace ToDo.Web.Pages.ViewModels.Todos
{

    public class TodoListViewModel
    {
      public int Id { get; set; }
      public string Title { get; set; }
      public string? Description { get; set; }
      public bool IsCompleted { get; set; }
      public string CreatedAtFormatted { get; set; } // ← formatado pra exibição
      public string StatusLabel { get; set; }        // "Concluída" ou "Pendente"
      public string StatusCssClass { get; set; }     // "badge-success" ou "badge-warning"
    }

}
