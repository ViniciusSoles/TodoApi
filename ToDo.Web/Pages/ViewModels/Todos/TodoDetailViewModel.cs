namespace ToDo.Web.Pages.ViewModels.Todos
{
    public class TodoDetailViewModel
    {
      public int Id { get; set; }
      public string Title { get; set; }
      public string? Description { get; set; }
      public bool IsCompleted { get; set; }
      public string CreatedAtFormatted { get; set; }
      public string? CompletedAtFormatted { get; set; } // null se não concluída
    }
}
