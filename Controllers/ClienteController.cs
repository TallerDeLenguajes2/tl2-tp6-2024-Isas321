using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_Isas321.Models;
using tl2_tp6_2024_Isas321.Repositorios;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace tl2_tp6_2024_Isas321.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteRepositorio _clienteRepositorio;

        public ClienteController(IClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        public IActionResult Index()
        {
            try
            {
                var clientes = _clienteRepositorio.ObtenerTodos();
                return View(clientes);
            }
            catch
            {
                TempData["Error"] = "Ocurrió un error al cargar la lista de clientes.";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Crear()
        {
            var clientes = _clienteRepositorio.ObtenerTodos();

            // Verifica que la lista no sea nula y pasa la lista de clientes a la vista
            ViewBag.Clientes = clientes != null 
                ? new SelectList(clientes, "IdCliente", "Nombre")
                : new SelectList(new List<Cliente>(), "IdCliente", "Nombre");

            return View();
        }

    [HttpPost]
    public IActionResult Crear(Cliente cliente)
    {
        if (!ModelState.IsValid)
        {
            var clientes1 = _clienteRepositorio.ObtenerTodos(); // Obtener lista de clientes
            ViewBag.Clientes = new SelectList(clientes1, "IdCliente", "Nombre"); // Recargar lista para evitar error
            return View(cliente);
        }

        try
        {
            var resultado = _clienteRepositorio.Crear(cliente);
            if (resultado)
            {
                TempData["Success"] = "Cliente creado correctamente.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Ocurrió un error al crear el cliente.";
            }
        }
        catch
        {
            TempData["Error"] = "Error inesperado al crear el cliente.";
        }

        var clientes = _clienteRepositorio.ObtenerTodos(); // Obtener lista de clientes
        ViewBag.Clientes = new SelectList(clientes, "IdCliente", "Nombre"); // Recargar lista
        return View(cliente);
    }


        public IActionResult Editar(int id)
        {
            try
            {
                var cliente = _clienteRepositorio.ObtenerPorId(id);
                if (cliente == null)
                {
                    TempData["Error"] = "Cliente no encontrado.";
                    return RedirectToAction("Index");
                }

                return View(cliente);
            }
            catch 
            {
                TempData["Error"] = "Error inesperado al buscar el cliente.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Editar(Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return View(cliente);
            }

            try
            {
                var resultado = _clienteRepositorio.Actualizar(cliente);
                if (resultado)
                {
                    TempData["Success"] = "Cliente actualizado correctamente.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Ocurrió un error al actualizar el cliente.";
                }
            }
            catch 
            {
                TempData["Error"] = "Error inesperado al actualizar el cliente.";
            }

            return View(cliente);
        }

 // Acción GET para mostrar la vista de confirmación de eliminación
        [HttpGet("Cliente/Eliminar/{id}")]
        public IActionResult Eliminar(int id)
        {
            var cliente = _clienteRepositorio.ObtenerPorId(id);
            if (cliente == null)
            {
                TempData["Error"] = "Cliente no encontrado.";
                return RedirectToAction("Index");
            }

            return View(cliente);
        }

        // Acción POST para realizar la eliminación del cliente
        [HttpPost("Cliente/Eliminar/{id}")]
        public IActionResult EliminarConfirmado(int id)
        {
            if (_clienteRepositorio.Eliminar(id))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.MensajeError = "No se pudo eliminar el cliente. Serás redirigido en 5 segundos.";
                return View("ErrorEliminar");
            }
        }
    }
}
