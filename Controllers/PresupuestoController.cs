using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using tl2_tp6_2024_Isas321.Models;
using tl2_tp6_2024_Isas321.Repositorios;

namespace tl2_tp6_2024_Isas321.Controllers;

public class PresupuestoController : Controller
{
    private readonly ILogger<PresupuestoController> _logger;
    private readonly IPresupuestoRepositorio _presupuestoRepositorio;

    // Constructor para inyectar el logger y el repositorio
    public PresupuestoController(ILogger<PresupuestoController> logger, IPresupuestoRepositorio presupuestoRepositorio)
    {
        _logger = logger;
        _presupuestoRepositorio = presupuestoRepositorio;
    }

    public IActionResult Index()
    {
        List<Presupuesto> presupuestos = _presupuestoRepositorio.ObtenerPresupuestoCompleto();
        return View(presupuestos);
    }

        // Acción para mostrar el formulario (GET)
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        // Acción para procesar el formulario (POST)
        [HttpPost]
        public IActionResult Crear(Presupuesto presupuesto)
        {
            // Validar el modelo
            if (!ModelState.IsValid)
            {
                return View(presupuesto);
            }

            // Crear un presupuesto vacío con los datos ingresados
            var idPresupuesto = _presupuestoRepositorio.CrearPresupuestoVacio(presupuesto);

            // Redirigir al usuario a una página de éxito o lista
            TempData["MensajeExito"] = "Presupuesto creado exitosamente.";
            return RedirectToAction("Detalle", new { id = idPresupuesto });
        }

        // Acción para mostrar detalles del presupuesto creado
        public IActionResult Detalle(int id)
        {
            var presupuesto = _presupuestoRepositorio.ObtenerPorId(id);
            if (presupuesto == null)
            {
                return NotFound();
            }
            return View(presupuesto);
        }

}
