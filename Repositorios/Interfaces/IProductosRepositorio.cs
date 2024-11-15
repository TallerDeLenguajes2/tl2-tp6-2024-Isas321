using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiWebAPI.Models;

namespace MiWebAPI.Repositorios
{
  public interface IProductosRepositorio
  {
    public void Create(Producto producto);
    public List<Producto> GetAll();
    public Producto GetProductoPorId(int id);
    public bool Remove(int id);
    public void ActualizarNombrePorId(int id, string nuevoNombre);

  }
}