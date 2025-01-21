// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Logging;
// using System;
// using System.Collections.Generic;
// using tl2_tp6_2024_Isas321.Models;
// using tl2_tp6_2024_Isas321.Repositorios;


// namespace tl2_tp6_2024_Isas321.Controllers
// {
//     public class PresupuestoController : Controller
//     {
//         private readonly ILogger<PresupuestoController> _logger;
//         private readonly IPresupuestoRepositorio _presupuestoRepositorio;

//         // Constructor para inyectar el logger y el repositorio
//         public PresupuestoController(ILogger<PresupuestoController> logger, IPresupuestoRepositorio presupuestoRepositorio)
//         {
//             _logger = logger;
//             _presupuestoRepositorio = presupuestoRepositorio;
//         }




//         /// <summary>
//         /// Muestra la lista de presupuestos.
//         /// </summary>
//         /// <returns>Vista con los presupuestos.</returns>
//         public IActionResult Index()
//         {
//             try
//             {
//                 List<Presupuesto> presupuestos = _presupuestoRepositorio.ObtenerPresupuestoCompleto();
//                 return View(presupuestos);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al obtener presupuestos: {ex.Message}");
//                 TempData["Error"] = "Ocurrió un error al cargar los presupuestos.";
//                 return RedirectToAction("Error");
//             }
//         }

//         /// <summary>
//         /// Formulario para crear un nuevo presupuesto.
//         /// </summary>
//         /// <returns>Vista para crear un presupuesto.</returns>
//         [HttpGet]
//         public IActionResult Crear()
//         {
//             return View();
//         }

//         /// <summary>
//         /// Guarda un nuevo presupuesto.
//         /// </summary>
//         /// <param name="presupuesto">Datos enviados desde el formulario.</param>
//         /// <returns>Redirige a Index si es exitoso o muestra errores.</returns>
//         [HttpPost]
//         public IActionResult Crear(Presupuesto presupuesto)
//         {
//             if (!ModelState.IsValid)
//             {
//                 return View(presupuesto);
//             }

//             try
//             {
//                 _presupuestoRepositorio.CrearPresupuestoVacio(presupuesto);
//                 TempData["Success"] = "Presupuesto creado correctamente.";
//                 return RedirectToAction("Index");
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al crear el presupuesto: {ex.Message}");
//                 TempData["Error"] = "Ocurrió un error al crear el presupuesto.";
//                 return View(presupuesto);
//             }
//         }

//         /// <summary>
//         /// Muestra los detalles de un presupuesto.
//         /// </summary>
//         /// <param name="id">ID del presupuesto.</param>
//         /// <returns>Vista con los detalles del presupuesto.</returns>
//         public IActionResult Detalle(int id)
//         {
//             if (id <= 0)
//             {
//                 TempData["Error"] = "El ID no es válido.";
//                 return RedirectToAction("Index");
//             }

//             try
//             {
//                 var presupuesto = _presupuestoRepositorio.ObtenerPorId(id);

//                 if (presupuesto == null)
//                 {
//                     TempData["Error"] = $"No se encontró un presupuesto con el ID {id}.";
//                     return RedirectToAction("Index");
//                 }

//                 return View(presupuesto);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al obtener el presupuesto con ID {id}: {ex.Message}");
//                 TempData["Error"] = "Ocurrió un error al cargar el presupuesto.";
//                 return RedirectToAction("Index");
//             }
//         }

//         /// <summary>
//         /// Elimina un presupuesto.
//         /// </summary>
//         /// <param name="id">ID del presupuesto.</param>
//         /// <returns>Redirige a la lista tras eliminar.</returns>
//         [HttpPost]
//         public IActionResult Eliminar(int id)
//         {
//             if (id <= 0)
//             {
//                 TempData["Error"] = "El ID no es válido.";
//                 return RedirectToAction("Index");
//             }

//             try
//             {
//                 bool eliminado = _presupuestoRepositorio.Eliminar(id);

//                 if (!eliminado)
//                 {
//                     TempData["Error"] = $"No se encontró un presupuesto con el ID {id}.";
//                     return RedirectToAction("Index");
//                 }

//                 TempData["Success"] = $"El presupuesto con ID {id} fue eliminado correctamente.";
//                 return RedirectToAction("Index");
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError($"Error al eliminar el presupuesto con ID {id}: {ex.Message}");
//                 TempData["Error"] = "Ocurrió un error al eliminar el presupuesto.";
//                 return RedirectToAction("Index");
//             }
//         }

//         /// <summary>
//         /// Vista de error genérico.
//         /// </summary>
//         /// <returns>Vista de error.</returns>
//         public IActionResult Error()
//         {
//             return View();
//         }




//         [HttpGet]
//         public IActionResult AgregarProducto(int idPresupuesto)
//         {
//             // Obtener lista de productos para el desplegable
//             ViewBag.Productos = _productoRepositorio.ObtenerTodos();

//             // Pasar el ID del presupuesto como modelo
//             return View(idPresupuesto);
//         }




//         [HttpPost]
//         public IActionResult AgregarProducto(int idPresupuesto, int idProducto, int cantidad)
//         {
//             // Validar datos
//             if (cantidad <= 0)
//             {
//                 ModelState.AddModelError("", "La cantidad debe ser mayor a 0.");
//                 return RedirectToAction("AgregarProducto", new { idPresupuesto });
//             }

//             // Obtener el producto desde el repositorio
//             var producto = _productoRepositorio.ObtenerPorId(idProducto);
//             if (producto == null)
//             {
//                 ModelState.AddModelError("", "Producto no válido.");
//                 return RedirectToAction("AgregarProducto", new { idPresupuesto });
//             }

//             // Llamar al método del repositorio
//             var resultado = _presupuestoRepositorio.AgregarProductoYcantidad(idPresupuesto, producto, cantidad);

//             if (!resultado)
//             {
//                 ModelState.AddModelError("", "No se pudo agregar el producto al presupuesto.");
//                 return RedirectToAction("AgregarProducto", new { idPresupuesto });
//             }

//             return RedirectToAction("Index");
//         }

//     }
// }





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
        private readonly IProductoRepositorio _productoRepositorio;

        // Constructor para inyectar logger, repositorio de presupuestos y productos
        public PresupuestoController(
            ILogger<PresupuestoController> logger,
            IPresupuestoRepositorio presupuestoRepositorio,
            IProductoRepositorio productoRepositorio)
        {
            _logger = logger;
            _presupuestoRepositorio = presupuestoRepositorio;
            _productoRepositorio = productoRepositorio;
        }

        // [HttpGet]
        // public IActionResult AgregarProducto(int idPresupuesto)
        // {
        //     try
        //     {
        //         // Obtener lista de productos para el desplegable
        //         var productos = _productoRepositorio.GetAll();
        //         if (productos == null)
        //         {
        //             TempData["Error"] = "No se pudieron cargar los productos.";
        //             return RedirectToAction("Index");
        //         }

        //         // Pasar los datos necesarios a la vista
        //         ViewBag.Productos = productos;
        //         ViewBag.IdPresupuesto = idPresupuesto;

        //         return View();
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError($"Error al cargar productos: {ex.Message}");
        //         TempData["Error"] = "Ocurrió un error al cargar los productos.";
        //         return RedirectToAction("Index");
        //     }
        // }
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
            return View();
        }

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

    }
}
