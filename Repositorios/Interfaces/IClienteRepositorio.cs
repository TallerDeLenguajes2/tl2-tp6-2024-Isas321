using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using  tl2_tp6_2024_Isas321.Models;

namespace tl2_tp6_2024_Isas321.Repositorios
{
  public interface IClienteRepositorio
  {
    public List<Cliente> ObtenerTodos();
    public Cliente ObtenerPorId(int id);
    public bool Crear(Cliente cliente);
    public bool Actualizar(Cliente cliente);
    public bool Eliminar(int id);
  }
}