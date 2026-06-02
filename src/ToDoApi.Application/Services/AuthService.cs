using FluentResults;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoApi.Application.DTOs.AuthDtos;
using ToDoApi.Application.DTOs.TodoDtos;
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

    public async Task<Result> RegisterAsync(RegisterDto dto)
    {
        if (await _repository.EmailExistsAsync(dto.Email))
            return Result.Fail("Email já cadastrado.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        var user = new User(dto.Name, dto.Email, passwordHash, Roles.User); // ← sempre User
        await _repository.AddAsync(user);

        return Result.Ok();
    }

    public async Task<Result<TokenResponseDto>> LoginAsync(LoginDto dto)
    {
        var user = await _repository.GetByEmailAsync(dto.Email);

        if (user is null)
            return Result.Fail("Credenciais inválidas.");

        
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Result.Fail("Credenciais inválidas.");

        return Result.Ok(await GenerateTokens(user));
    }

    private string GenerateAccessToken(User user)
    {
        var secretKey = _configuration["Jwt:SecretKey"]!;
        var issuer = _configuration["Jwt:Issuer"]!;
        var audience = _configuration["Jwt:Audience"]!;
        var expiration = int.Parse(_configuration["Jwt:AccessTokenExpirationMinutes"]!);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

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

        return new JwtSecurityTokenHandler().WriteToken(token); 
    }

    private static string GenerateRefreshToken()
    {

        return Convert.ToBase64String(
        System.Security.Cryptography.RandomNumberGenerator.GetBytes(64));
    }

    public async Task<Result> RevokeAsync(string refreshToken)
    {
        var refreshHash = HashToken(refreshToken);
        var user = await _repository.GetByRefreshTokenAsync(refreshHash);

        if (user is null)
            return Result.Fail("Refresh token inválido.");

        user.RevokeRefreshToken();
        await _repository.UpdateAsync(user);
        return Result.Ok();
    }

    public async Task<Result<TokenResponseDto>> RefreshAsync(string refreshToken)
    {
        var refreshHash = HashToken(refreshToken); 
        var user = await _repository.GetByRefreshTokenAsync(refreshHash);

        if (user is null || !user.IsRefreshTokenValid(refreshHash))
            return Result.Fail("Refresh token inválido ou expirado.");

        var tokens = await GenerateTokens(user);
            return Result.Ok(tokens);
    }

    public async Task<TokenResponseDto> GenerateTokens(User user)
    {

        var accessToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken(); 
        var refreshHash = HashToken(refreshToken); 
        var refreshExpiry = DateTime.UtcNow.AddDays(
            int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"]!));

        user.SetRefreshToken(refreshHash, refreshExpiry); 
        await _repository.UpdateAsync(user);

        return new TokenResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken, 
            AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(
                int.Parse(_configuration["Jwt:AccessTokenExpirationMinutes"]!)),
            RefreshTokenExpiresAt = refreshExpiry
        };
    }

   
    private static string HashToken(string token)
    {
        var bytes = System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(bytes);
    }


}



