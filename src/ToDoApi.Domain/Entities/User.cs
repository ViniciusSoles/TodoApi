using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApi.Domain.Entities;

    public class User
    {

      public int Id { get; private set; }
      public string Name { get; private set; }
      public string Email { get; private set; }
      public string PasswordHash { get; private set; }
      public string Role { get; private set; }
      public string? RefreshToken { get; private set; }           
      public DateTime? RefreshTokenExpiresAt { get; private set; }


    protected User() { }

      public User(string name, string email, string passwordHash,string role = "User")
      {
         if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome é obrigatório.");

         if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email é obrigatório.");

          Name = name;
          Email = email;
          PasswordHash = passwordHash;
          Role = role;  

    }

    public void SetRefreshToken(string token, DateTime expiresAt)
    {
        RefreshToken = token;
        RefreshTokenExpiresAt = expiresAt;
    }

    public void RevokeRefreshToken()  // ← adiciona esse
    {
        RefreshToken = null;
        RefreshTokenExpiresAt = null;
    }

    public bool IsRefreshTokenValid(string token) =>
        RefreshToken == token &&
        RefreshTokenExpiresAt > DateTime.UtcNow;
    }


















