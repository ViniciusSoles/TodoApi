using Microsoft.AspNetCore.Mvc;
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
        }
    
