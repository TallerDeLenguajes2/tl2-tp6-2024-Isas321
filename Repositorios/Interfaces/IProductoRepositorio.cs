using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using  tl2_tp6_2024_Isas321.Models;

namespace tl2_tp6_2024_Isas321.Repositorios
{
  public interface IProductoRepositorio
  {
    public void Create(Producto producto);
     List<Producto> GetAll(); // Asegúrate de que el tipo de retorno sea List<Producto>
    // public Producto GetProductoPorId(int id);
    public bool Remove(int id);
    // public void ActualizarNombrePorId(int id, string nuevoNombre);

  }
}