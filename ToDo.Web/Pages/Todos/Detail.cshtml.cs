using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToDo.Web.Pages.ViewModels.Todos;
using ToDoApi.Application.Interfaces;


namespace ToDoApi.Web.Pages.Todos.DetailModel;

public class DetailModel : PageModel
{
    private readonly ITodoService _service;

    public TodoDetailViewModel ViewModel { get; set; }

    public DetailModel(ITodoService service)
    {
        _service = service;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var result = await _service.GetByIdAsync(id);

        if (result.IsFailed)
            return RedirectToPage("/Todos/Index");

        var t = result.Value;

        ViewModel = new TodoDetailViewModel
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            IsCompleted = t.IsCompleted,
            CreatedAtFormatted = t.CreatedAt.ToString("dd/MM/yyyy HH:mm"),
            CompletedAtFormatted = t.CompletedAt?.ToString("dd/MM/yyyy HH:mm")
        };

        return Page();
    }

    public async Task<IActionResult> OnPostCompleteAsync(int id)
    {
        await _service.CompleteAsync(id);
        return RedirectToPage("/Todos/Index");
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        await _service.DeleteAsync(id);
        return RedirectToPage("/Todos/Index");
    }
}