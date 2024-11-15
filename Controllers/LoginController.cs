
using Microsoft.AspNetCore.Mvc;
using MiWebAPI.ViewModel;

public class LoginController : Controller
{
    // private readonly InMemoryUserRepository _inMemoryUserRepository;
    private readonly IUserRepository userRepository;

    // public LoginController(InMemoryUserRepository inMemoryUserRepository)
    public LoginController(IUserRepository userRepository)
    {
        // _inMemoryUserRepository = inMemoryUserRepository;
        this.userRepository = userRepository;
    }

    public IActionResult Index()
    {
        var model = new LoginViewModel
        {
            IsAuthenticated = false
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
        {
            model.ErrorMessage = "El nombre de usuario o la contraseña no pueden estar vacíos.";
            return View("Index", model); // Si algún campo está vacío, muestra la vista con el mensaje de error
        }

        // Buscar al usuario en el repositorio
        // User usuario = _inMemoryUserRepository.GetUser(model.Username, model.Password);
        User usuario = userRepository.GetUser(model.Username, model.Password);

        
        if (usuario != null)
        {
            // Si el usuario es encontrado, se guarda la información en la sesión
            HttpContext.Session.SetString("IsAuthenticated", "true");
            HttpContext.Session.SetString("User", model.Username);
            HttpContext.Session.SetString("SuccessLevel", usuario.AccessLevel.ToString());

            return RedirectToAction("Dashboard"); // Redirigir a otra acción (ejemplo: Dashboard)
        }
        else
        {
            // Si el usuario no es encontrado, asigna un mensaje de error y muestra la vista nuevamente
            model.ErrorMessage = "Nombre de usuario o contraseña incorrectos.";
            return View("Index", model);
        }
    }

    public IActionResult Logout()
    {
        // Limpiar la sesión
        HttpContext.Session.Clear();

        // Redirigir a la vista de login
        return RedirectToAction("Index");
    }
}
