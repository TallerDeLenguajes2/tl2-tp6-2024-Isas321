using System;
using System.Collections.Generic;
using System.Linq;

public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users; // Declaración del campo privado

    public InMemoryUserRepository()
    {
        _users = new List<User>
        {
            new User { Id = 1, UserName = "admin", Password = "password123", AccessLevel = AccessLevel.Admin },
            new User { Id = 2, UserName = "editor", Password = "editor123", AccessLevel = AccessLevel.Editor },
            new User { Id = 3, UserName = "invitado", Password = "guest123", AccessLevel = AccessLevel.Invitado },
            new User { Id = 4, UserName = "empleado", Password = "employee123", AccessLevel = AccessLevel.Empleado }
        };
    }

    public User GetUser(string userName, string password)
    {
        return _users
            .Where(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase) && 
                        u.Password.Equals(password, StringComparison.OrdinalIgnoreCase))
            .FirstOrDefault();
    }
}


public interface IUserRepository{
  User GetUser(string userName, string password);
}

