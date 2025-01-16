using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tl2_tp6_2024_Isas321.Models;

namespace tl2_tp6_2024_Isas321.Repositorios{
  public interface IPresupuestoRepositorio
  {
    public int CrearPresupuestoVacio(Presupuesto presupuesto);
    public List<Presupuesto> ObtenerPresupuestoCompleto();
    public Presupuesto ObtenerPorId(int id);
    public bool AgregarProductoYcantidad(int idPresupuesto, Producto producto, int cantidad);
    public bool Eliminar(int id);
  }
}