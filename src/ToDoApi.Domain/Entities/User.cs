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






    }

