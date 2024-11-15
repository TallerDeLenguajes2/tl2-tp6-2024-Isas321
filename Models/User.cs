public class User
{
    public int Id { get; set; } // Cambiado a PascalCase
    public string UserName { get; set; } // Corregido de "UserNAme"
    public string Password { get; set; }
    public AccessLevel AccessLevel { get; set; } // Corregido de "AccessLavel"
}

public enum AccessLevel
{
    Admin,
    Editor,
    Invitado,
    Empleado
}
