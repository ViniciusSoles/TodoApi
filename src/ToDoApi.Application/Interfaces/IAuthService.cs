using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApi.Application.DTOs;
using ToDoApi.Domain.Entities;

namespace ToDoApi.Application.Interfaces;

public interface IAuthService
{
    Task<Result<TokenResponseDto>> RegisterAsync(RegisterDto dto);
    Task<Result<TokenResponseDto>> LoginAsync(LoginDto dto);
    Task<Result> RevokeAsync(string refreshToken);  
    Task<Result<TokenResponseDto>> RefreshAsync(string refreshToken);

    Task<TokenResponseDto> GenerateTokens(User user);  
}
