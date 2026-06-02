using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToDo.Web.Pages.ViewModels;
using ToDo.Web.Pages.ViewModels.Todos;
using ToDoApi.Application.DTOs;
using ToDoApi.Application.Interfaces;


namespace ToDoApi.Web.Pages.Todos;

public class IndexModel : PageModel
{
    private readonly ITodoService _service;

    public TodoPageViewModel ViewModel { get; set; } = new();

    [BindProperty]
    public CreateTodoViewModel Input { get; set; }

    public IndexModel(ITodoService service)
    {
        _service = service;
    }

    public async Task OnGetAsync()
    {
        var result = await _service.GetAllAsync();

        if (result.IsFailed)
            return;

        var todos = result.Value.Select(t => new TodoListViewModel
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            IsCompleted = t.IsCompleted,
            CreatedAtFormatted = t.CreatedAt.ToString("dd/MM/yyyy"),
            StatusLabel = t.IsCompleted ? "Concluída" : "Pendente",
            StatusCssClass = t.IsCompleted ? "badge-success" : "badge-warning"
        }).ToList();

        ViewModel = new TodoPageViewModel
        {
            Todos = todos,
            TotalPendentes = todos.Count(t => !t.IsCompleted),
            TotalConcluidos = todos.Count(t => t.IsCompleted),
            NovoTodo = new CreateTodoViewModel()
        };
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await OnGetAsync();
            return Page();
        }

        await _service.CreateAsync(new CreateTodoDto
        {
            Title = Input.Title,
            Description = Input.Description
        });

        return RedirectToPage();
    }
}