using FluentResults;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoApi.Application.DTOs;
using ToDoApi.Application.Interfaces;
using ToDoApi.Domain.Constants;
using ToDoApi.Domain.Entities;
using ToDoApi.Domain.Interfaces;

namespace ToDoApi.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _repository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    public async Task<Result<TokenResponseDto>> RegisterAsync(RegisterDto dto)
    {
        // verifica se email já existe
        if (await _repository.EmailExistsAsync(dto.Email))
            return Result.Fail("Email já cadastrado.");

        if (dto.Role != Roles.Admin && dto.Role != Roles.User)
            return Result.Fail("Role inválida. Deve ser 'User' ou 'Admin'.");   

        // gera o hash da senha
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User(dto.Name, dto.Email, passwordHash);
        await _repository.AddAsync(user);

        return Result.Ok(GenerateToken(user));
    }

    public async Task<Result<TokenResponseDto>> LoginAsync(LoginDto dto)
    {
        var user = await _repository.GetByEmailAsync(dto.Email);

        if (user is null)
            return Result.Fail("Credenciais inválidas.");

        // valida a senha
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Result.Fail("Credenciais inválidas.");

        return Result.Ok(GenerateToken(user));
    }

    private TokenResponseDto GenerateToken(User user)
    {
        var secretKey = _configuration["Jwt:SecretKey"]!;
        var issuer = _configuration["Jwt:Issuer"]!;
        var audience = _configuration["Jwt:Audience"]!;
        var expiration = int.Parse(_configuration["Jwt:ExpirationMinutes"]!);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // claims — o que vai dentro do token
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role)   
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiration),
            signingCredentials: creds
        );

        return new TokenResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = DateTime.UtcNow.AddMinutes(expiration)
        };
    }
}

