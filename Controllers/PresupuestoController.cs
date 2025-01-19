using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using tl2_tp6_2024_Isas321.Models;
using tl2_tp6_2024_Isas321.Repositorios;

namespace tl2_tp6_2024_Isas321.Controllers
{
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

        /// <summary>
        /// Muestra la lista de presupuestos.
        /// </summary>
        /// <returns>Vista con los presupuestos.</returns>
        public IActionResult Index()
        {
            try
            {
                List<Presupuesto> presupuestos = _presupuestoRepositorio.ObtenerPresupuestoCompleto();
                return View(presupuestos);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener presupuestos: {ex.Message}");
                TempData["Error"] = "Ocurrió un error al cargar los presupuestos.";
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Formulario para crear un nuevo presupuesto.
        /// </summary>
        /// <returns>Vista para crear un presupuesto.</returns>
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        /// <summary>
        /// Guarda un nuevo presupuesto.
        /// </summary>
        /// <param name="presupuesto">Datos enviados desde el formulario.</param>
        /// <returns>Redirige a Index si es exitoso o muestra errores.</returns>
        [HttpPost]
        public IActionResult Crear(Presupuesto presupuesto)
        {
            if (!ModelState.IsValid)
            {
                return View(presupuesto);
            }

            try
            {
                _presupuestoRepositorio.CrearPresupuestoVacio(presupuesto);
                TempData["Success"] = "Presupuesto creado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el presupuesto: {ex.Message}");
                TempData["Error"] = "Ocurrió un error al crear el presupuesto.";
                return View(presupuesto);
            }
        }

        /// <summary>
        /// Muestra los detalles de un presupuesto.
        /// </summary>
        /// <param name="id">ID del presupuesto.</param>
        /// <returns>Vista con los detalles del presupuesto.</returns>
        public IActionResult Detalle(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "El ID no es válido.";
                return RedirectToAction("Index");
            }

            try
            {
                var presupuesto = _presupuestoRepositorio.ObtenerPorId(id);

                if (presupuesto == null)
                {
                    TempData["Error"] = $"No se encontró un presupuesto con el ID {id}.";
                    return RedirectToAction("Index");
                }

                return View(presupuesto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener el presupuesto con ID {id}: {ex.Message}");
                TempData["Error"] = "Ocurrió un error al cargar el presupuesto.";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Elimina un presupuesto.
        /// </summary>
        /// <param name="id">ID del presupuesto.</param>
        /// <returns>Redirige a la lista tras eliminar.</returns>
        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "El ID no es válido.";
                return RedirectToAction("Index");
            }

            try
            {
                bool eliminado = _presupuestoRepositorio.Eliminar(id);

                if (!eliminado)
                {
                    TempData["Error"] = $"No se encontró un presupuesto con el ID {id}.";
                    return RedirectToAction("Index");
                }

                TempData["Success"] = $"El presupuesto con ID {id} fue eliminado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar el presupuesto con ID {id}: {ex.Message}");
                TempData["Error"] = "Ocurrió un error al eliminar el presupuesto.";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Vista de error genérico.
        /// </summary>
        /// <returns>Vista de error.</returns>
        public IActionResult Error()
        {
            return View();
        }
    }
}
