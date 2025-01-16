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

    [HttpPost]
    public IActionResult Crear()
    {
        List<Presupuesto> presupuestos = _presupuestoRepositorio.ObtenerPresupuestoCompleto();
        return View(presupuestos);
    }
}
