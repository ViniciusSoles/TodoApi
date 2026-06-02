using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToDo.Web.Pages.ViewModels.Auth;
using ToDoApi.Application.DTOs;
using ToDoApi.Application.Interfaces;

namespace ToDo.Web.Pages.Auth;

public class RegisterModel : PageModel
{
    private readonly IAuthService _service;

    [BindProperty]
    public RegisterViewModel Input { get; set; }

    public string? ErrorMessage { get; set; }

    public RegisterModel(IAuthService service)
    {
        _service = service;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var result = await _service.RegisterAsync(new RegisterDto
        {
            Name = Input.Name,
            Email = Input.Email,
            Password = Input.Password
        });

        if (result.IsFailed)
        {
            ErrorMessage = result.Errors.First().Message;
            return Page();
        }

        return RedirectToPage("/Auth/Login");
    }
}