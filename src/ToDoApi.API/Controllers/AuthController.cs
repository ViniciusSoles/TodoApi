using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using ToDoApi.Application.DTOs;
using ToDoApi.Application.Interfaces;
namespace ToDoApi.API.Controllers;

public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterDto dto)
    {
        var result = await _service.RegisterAsync(dto);

        if (result.IsFailed)
            return BadRequest(new ProblemDetails
            {
                Title = "Erro no cadastro.",
                Detail = result.Errors.First().Message,
                Status = StatusCodes.Status400BadRequest
            });

        return Created("/auth/login",new { message="Cadastro Realizado.Faça Login para continuar"});
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

        SetRefreshTokenCookie(result.Value.RefreshToken, result.Value.RefreshTokenExpiresAt);

        return Ok(new
        {
            result.Value.AccessToken,
            result.Value.AccessTokenExpiresAt
        });
    }


    private void SetRefreshTokenCookie(string refreshToken, DateTime expiresAt)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,  
            Secure = true,  
            SameSite = SameSiteMode.Strict, 
            Expires = expiresAt
        };

        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }


    [HttpPost("refresh")]
    public async Task<ActionResult<TokenResponseDto>> Refresh()
    {
  
        var refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized(new ProblemDetails
            {
                Title = "Refresh token não encontrado.",
                Status = 401
            });

        var result = await _service.RefreshAsync(refreshToken);

        if (result.IsFailed)
            return Unauthorized(new ProblemDetails
            {
                Title = "Token inválido.",
                Detail = result.Errors.First().Message,
                Status = 401
            });

        SetRefreshTokenCookie(result.Value.RefreshToken, result.Value.RefreshTokenExpiresAt);

        return Ok(new
        {
            result.Value.AccessToken,
            result.Value.AccessTokenExpiresAt
        });
    }



    [HttpPost("revoke")]
    [Authorize]
    public async Task<ActionResult> Revoke()
    {
       
        var refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
            return BadRequest(new ProblemDetails
            {
                Title = "Refresh token não encontrado.",
                Status = StatusCodes.Status400BadRequest
            });

        var result = await _service.RevokeAsync(refreshToken);

        if (result.IsFailed)
            return BadRequest(new ProblemDetails
            {
                Title = "Erro ao revogar token.",
                Detail = result.Errors.First().Message,
                Status = StatusCodes.Status400BadRequest
            });

      
        Response.Cookies.Delete("refreshToken");

        return NoContent(); 
    }


}

