using System.ComponentModel.DataAnnotations;


namespace tl2_tp6_2024_Isas321.Models
{
  public class EditarPresupuestoViewModel
  {
      public int IdPresupuesto { get; set; }
      
      [Required(ErrorMessage = "Debe seleccionar un cliente.")]
      public int ClienteId { get; set; }
      
      [Required(ErrorMessage = "Debe ingresar una fecha.")]
      public DateTime FechaCreacion { get; set; }
      
      public List<Cliente> Clientes { get; set; }
  }
}