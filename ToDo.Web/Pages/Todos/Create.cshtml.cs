using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToDo.Web.Pages.ViewModels.Todos;
using ToDoApi.Application.DTOs;
using ToDoApi.Application.Interfaces;


namespace ToDoApi.Web.Pages.Todos.CreateModel;

public class CreateModel : PageModel
{
    private readonly ITodoService _service;

    [BindProperty]
    public CreateTodoViewModel Input { get; set; }

    public string? ErrorMessage { get; set; }

    public CreateModel(ITodoService service)
    {
        _service = service;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var result = await _service.CreateAsync(new CreateTodoDto
        {
            Title = Input.Title,
            Description = Input.Description
        });

        if (result.IsFailed)
        {
            ErrorMessage = result.Errors.First().Message;
            return Page();
        }

        return RedirectToPage("/Todos/Index");
    }
}