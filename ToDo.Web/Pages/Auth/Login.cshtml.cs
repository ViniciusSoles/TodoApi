using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToDo.Web.Pages.ViewModels.Auth;
using ToDoApi.Application.DTOs;
using ToDoApi.Application.Interfaces;

namespace ToDo.Web.Pages.Auth;

public class LoginModel : PageModel
{
    private readonly IAuthService _service;

    [BindProperty]
    public LoginViewModel Input { get; set; }

    public string? ErrorMessage { get; set; }

    public LoginModel(IAuthService service)
    {
        _service = service;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var result = await _service.LoginAsync(new LoginDto
        {
            Email = Input.Email,
            Password = Input.Password
        });

        if (result.IsFailed)
        {
            ErrorMessage = result.Errors.First().Message;
            return Page();
        }

        // guarda o token na sessão
        HttpContext.Session.SetString("AccessToken", result.Value.AccessToken);

        return RedirectToPage("/Todos/Index");
    }
}
