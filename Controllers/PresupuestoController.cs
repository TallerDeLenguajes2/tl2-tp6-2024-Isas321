using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly IProductoRepositorio _productoRepositorio;
        private readonly IClienteRepositorio _clienteRepositorio;

        // Constructor para inyectar logger, repositorio de presupuestos y productos
        public PresupuestoController(
            ILogger<PresupuestoController> logger,
            IPresupuestoRepositorio presupuestoRepositorio,
            IProductoRepositorio productoRepositorio,
            IClienteRepositorio clienteRepositorio)
        {
            _logger = logger;
            _presupuestoRepositorio = presupuestoRepositorio;
            _productoRepositorio = productoRepositorio;
            _clienteRepositorio = clienteRepositorio;
        }


        [HttpGet]
        public IActionResult AgregarProducto(int idPresupuesto)
        {
            try
            {
                var productos = _productoRepositorio.GetAll();
                if (productos == null)
                {
                    TempData["Error"] = "No se pudieron cargar los productos.";
                    return RedirectToAction("Index");
                }

                ViewBag.Productos = productos;
                ViewBag.IdPresupuesto = idPresupuesto; // Asegura que este valor no sea nulo
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al cargar productos: {ex.Message}");
                TempData["Error"] = "Ocurrió un error al cargar los productos.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult AgregarProducto(int idPresupuesto, int idProducto, int cantidad)
        {
            try
            {
                // Validar cantidad
                if (cantidad <= 0)
                {
                    ModelState.AddModelError("", "La cantidad debe ser mayor a 0.");
                    return RedirectToAction("AgregarProducto", new { idPresupuesto });
                }

                // Obtener producto por ID
                var producto = _productoRepositorio.GetProductoPorId(idProducto);
                if (producto == null)
                {
                    ModelState.AddModelError("", "Producto no válido.");
                    return RedirectToAction("AgregarProducto", new { idPresupuesto });
                }

                // Agregar producto al presupuesto
                var resultado = _presupuestoRepositorio.AgregarProductoYcantidad(idPresupuesto, producto, cantidad);
                if (!resultado)
                {
                    ModelState.AddModelError("", "No se pudo agregar el producto al presupuesto.");
                    return RedirectToAction("AgregarProducto", new { idPresupuesto });
                }

                TempData["Success"] = "Producto agregado correctamente al presupuesto.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al agregar producto: {ex.Message}");
                TempData["Error"] = "Ocurrió un error al agregar el producto.";
                return RedirectToAction("Index");
            }
        }


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
        [HttpGet]
        public IActionResult Crear()
        {
            // Crear el ViewModel
            var viewModel = new CrearPresupuestoViewModel
            {
                Clientes = _clienteRepositorio.ObtenerTodos() // Obtener lista de clientes
            };

            return View(viewModel); // Pasar el ViewModel a la vista
        }



        [HttpPost]
        public IActionResult Crear(CrearPresupuestoViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                // Si el modelo no es válido, recarga los clientes para que la lista de clientes no se pierda
                var clientes = _clienteRepositorio.ObtenerTodos();
                ViewBag.Clientes = new SelectList(clientes, "IdCliente", "Nombre");
                return View(viewModel);
            }

            try
            {
                // Crear el presupuesto utilizando los datos del ViewModel

                var cliente = _clienteRepositorio.ObtenerPorId(viewModel.ClienteId);
                var fecha = viewModel.FechaCreacion;

                var presupuesto = new Presupuesto(0, cliente, fecha, new List<PresupuestoDetalle>());

                // Guardar el presupuesto en la base de datos (este código depende de tu implementación)
                _presupuestoRepositorio.CrearPresupuestoVacio(presupuesto);

                TempData["Success"] = "Presupuesto creado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error inesperado: {ex.Message}";
                return View(viewModel);
            }
        }

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

        public IActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var presupuesto = _presupuestoRepositorio.ObtenerPorId(id);
            
            if (presupuesto == null)
            {
                return NotFound();
            }

            var viewModel = new EditarPresupuestoViewModel
            {
                IdPresupuesto = presupuesto.IdPresupuesto,
                ClienteId = presupuesto.Cliente.ClienteId,
                FechaCreacion = presupuesto.FechaCreacion,
                Clientes = _clienteRepositorio.ObtenerTodos() // Obtener lista de clientes
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Editar(EditarPresupuestoViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                // Si hay errores, recargamos la lista de clientes
                viewModel.Clientes = _clienteRepositorio.ObtenerTodos();
                return View(viewModel);
            }

            try
            {
                var nuevoCliente = _clienteRepositorio.ObtenerPorId(viewModel.ClienteId);
                var resultado = _presupuestoRepositorio.EditarPresupuesto(viewModel.IdPresupuesto, nuevoCliente, viewModel.FechaCreacion);

                if (resultado)
                {
                    TempData["Success"] = "Presupuesto actualizado correctamente.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "No se pudo actualizar el presupuesto.";
                    return View(viewModel);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error inesperado: {ex.Message}";
                return View(viewModel);
            }
        }
    }
}
