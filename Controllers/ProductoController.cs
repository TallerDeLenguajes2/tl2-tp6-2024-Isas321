using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using tl2_tp6_2024_Isas321.Models;
using tl2_tp6_2024_Isas321.Repositorios;

namespace tl2_tp6_2024_Isas321.Controllers;

public class ProductoController : Controller
{
    private readonly ILogger<ProductoController> _logger;
    private readonly IProductoRepositorio _productoRepositorio;

    // Constructor para inyectar el logger y el repositorio
    public ProductoController(ILogger<ProductoController> logger, IProductoRepositorio productoRepositorio)
    {
        _logger = logger;
        _productoRepositorio = productoRepositorio;
    }

    public IActionResult Index()
    {
        List<Producto> productos = _productoRepositorio.GetAll();
        return View(productos);
    }

    // Acción para mostrar el formulario de creación (GET)
    [HttpGet("Producto/Crear")]
    public IActionResult Crear()
    {
        return View();
    }

    // Acción para manejar el envío del formulario de creación (POST)
    [HttpPost("Producto/Crear")]
    public IActionResult Crear(Producto producto)
    {
        if (ModelState.IsValid) // Valida que los datos sean correctos
        {
            _productoRepositorio.Create(producto); // Llama al método del repositorio
            return RedirectToAction("Index", "Producto"); // Redirige a la lista de productos
        }

        return View(producto); // Si hay errores, muestra el formulario con mensajes
    }


    [HttpPost("Producto/Eliminar/{id}")]
    public IActionResult Eliminar(int id)
    {
        if (_productoRepositorio.Remove(id))
        {
            return RedirectToAction("Index");
        }
        else
        {
            ViewBag.MensajeError = "No se pudo eliminar el producto. Serás redirigido en 5 segundos.";
            return View("ErrorEliminar");
        }
    }


    [HttpGet]
    public IActionResult Editar(int id)
    {
        var producto = _productoRepositorio.GetProductoPorId(id);
        if (producto == null)
        {
            return NotFound(); 
        }
        return View(producto);
    }

    [HttpPost]
    public IActionResult Editar(int id, Producto producto)
    {
        if (!ModelState.IsValid)
        {
            return View(producto); // Si el modelo no es válido, devuelve la vista con los errores
        }

        bool actualizado = _productoRepositorio.ActualizarProducto(id, producto);

        if (!actualizado)
        {
            ModelState.AddModelError("", "No se pudo actualizar el producto.");
            return View(producto);
        }

        return RedirectToAction("Index"); 
    }
}
