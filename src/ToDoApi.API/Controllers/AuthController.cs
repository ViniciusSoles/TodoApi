using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;
using Serilog;
using ToDoApi.Application.DTOs;
using ToDoApi.Application.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

namespace ToDoApi.API.Controllers;

public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<ActionResult<TokenResponseDto>> Register([FromBody] RegisterDto dto)
    {
        var result = await _service.RegisterAsync(dto);

        if (result.IsFailed)
            return BadRequest(new ProblemDetails
            {
                Title = "Erro no cadastro.",
                Detail = result.Errors.First().Message,
                Status = StatusCodes.Status400BadRequest
            });

        return Ok(result.Value);
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseDto>> Login([FromBody] LoginDto dto)
    {
        var result = await _service.LoginAsync(dto);

        if (result.IsFailed)
            return Unauthorized(new ProblemDetails
            {
                Title = "Credenciais inválidas.",
                Detail = result.Errors.First().Message,
                Status = StatusCodes.Status401Unauthorized
            });

        return Ok(result.Value);
    }


    [HttpPost("refresh")]
    public async Task<ActionResult<TokenResponseDto>> Refresh([FromBody] string refreshToken)
    {
        var result = await _service.RefreshAsync(refreshToken);

        if (result.IsFailed)
            return Unauthorized(new ProblemDetails
            {
                Title = "Token inválido.",
                Detail = result.Errors.First().Message,
                Status = StatusCodes.Status401Unauthorized
            });

        return Ok(result.Value);
    }

    [HttpPost("revoke")]
    [Authorize]
    public async Task<ActionResult> Revoke([FromBody] string refreshToken)
    {
        var result = await _service.RevokeAsync(refreshToken);

        if (result.IsFailed)
            return BadRequest(new ProblemDetails
            {
                Title = "Erro ao revogar token.",
                Detail = result.Errors.First().Message,
                Status = StatusCodes.Status400BadRequest
            });

        return NoContent();
    }
}
